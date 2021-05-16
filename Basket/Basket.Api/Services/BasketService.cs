using Basket.Api.Entities;
using Basket.Api.Models;
using Basket.Api.Repositories;
using Basket.Api.Repositories.EntityFramework;
using Basket.Api.Rules.Validations;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Basket.Api.Services
{
    public class BasketService : IBasketService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IRepositoryReadOnly _repository;
        private readonly IDistributedCache _redisCache;
        public BasketService(IBasketRepository basketRepository, IRepositoryReadOnly repository, IDistributedCache redisCache)
        {
            _basketRepository = basketRepository ?? throw new ArgumentNullException(nameof(basketRepository));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _redisCache = redisCache ?? throw new ArgumentNullException(nameof(redisCache));
        }

        public async Task<bool> AddBasketItem(BasketItemModel item)
        {
            //if the product already exists, it will be updated.
            var currentBasketItem = await _basketRepository.GetBasketItemByProductId(item.UserId, item.ProductId);
            if (currentBasketItem != null)
            {
                return await UpdateBasketItem(currentBasketItem, item.Quantity);
            }

            var product = await _repository.GetOneAsync<Product>(p => p.Id == item.ProductId);
            var user = await _repository.GetOneAsync<User>(p => p.Id == item.UserId);

            //if there is a coupon for the product, it will be deducted from the price.
            var couponDb = await _redisCache.GetStringAsync($"coupon_{item.ProductId}");
            Coupon coupon = null;
            if (!String.IsNullOrEmpty(couponDb))
            {
                coupon = JsonConvert.DeserializeObject<Coupon>(couponDb);
            }

            //rule validation with Chain of Responsibility
            CheckRule(product, item, user);

            var basketItem = BasketItem.CreateBasketItem(item, product, coupon);
            var userInfo = User.CreateUser(user);

            return await _basketRepository.AddBasketItem(basketItem);
        }

        private void CheckRule(Product product, BasketItemModel item, User user)
        {
            RuleApprover nonProduct = new NonProductApprover();
            RuleApprover notAvailableProductStatus = new NotAvailableProductStatusApprover();
            nonProduct.SetSuccessor(notAvailableProductStatus);
            nonProduct.ProcessRequest(product, item, user);
        }

        public async Task<Models.Basket> GetBasket(int userId)
        {
            var basketItems = await _basketRepository.GetBasket(userId);
            return new Models.Basket { Items = basketItems.ToList() };
        }

        public async Task<bool> UpdateBasketItem(BasketItem item, int quantity)
        {
            item.Quantity += quantity;
            return await _basketRepository.UpdateBasketItem(item);
        }

        public async Task<bool> UpdateBasketItem(BasketItem item)
        {
            var product = await _repository.GetOneAsync<Product>(p => p.Id == item.ProductId);

            var couponDb = await _redisCache.GetStringAsync($"coupon_{item.ProductId}");
            Coupon coupon = null;
            if (!String.IsNullOrEmpty(couponDb))
            {
                coupon = JsonConvert.DeserializeObject<Coupon>(couponDb);
            }
            item.Price = product.Price - (coupon?.Amount ?? 0);

            return await _basketRepository.UpdateBasketItem(item);
        }

        public async Task<bool> DeleteBasketItem(string basketItemId)
        {
            return await _basketRepository.DeleteBasketItem(basketItemId);
        }

        public async Task<bool> ClearBasket(int userId)
        {
            return await _basketRepository.ClearBasket(userId);
        }
    }
}
