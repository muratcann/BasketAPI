using Basket.Model.Dto;
using Basket.Model.Repositories;
using Basket.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.Service.Service
{
    public class BasketService : IBasketService
    {
        private readonly IRepository _iRepository;
        public BasketService(IRepository iRepository)
        {
            _iRepository = iRepository;
        }

        public async Task<BasketItem> AddItemIntoBasketAsync(BasketItem basketItem)
        {
            IEnumerable<BasketItem> basketItems = await _iRepository.GetAsync<BasketItem>(b => b.UserId == basketItem.UserId);
            //this is for primeray key generate becuase the actuall database is not not connected, once we have actuall db connected the following live of code will be remove
            if (basketItems.Count() > 0)
            {
                basketItem.Id = basketItems.Last().Id + 1;
            }
            else
            {
                basketItem.Id = 1;
            }

            _iRepository.Create<BasketItem>(basketItem);
            await _iRepository.SaveAsync();
            return basketItem;
        }

        public async Task<IList<BasketItem>> GetBasketItemsAsync(int userId)
        {
            var basketItems = await _iRepository.GetAsync<BasketItem>(b => b.UserId == userId);
            basketItems = PopulateProductIntoBasketItem(basketItems.ToList());
            return basketItems.ToList();
        }

        public async Task<IList<BasketItem>> ClearBasketAsync(int userId)
        {
            var basketItems = await _iRepository.GetAsync<BasketItem>(b => b.UserId == userId);

            foreach (var basketItem in basketItems)
            {
                _iRepository.Delete<BasketItem>(basketItem);
            }
            await _iRepository.SaveAsync();

            return await GetBasketItemsAsync(userId);
        }

        public async Task<IList<BasketItem>> DeleteBasketItemByIdAsync(int id)
        {
            var basketItem = await _iRepository.GetByIdAsync<BasketItem>(id);
            if (basketItem != null)
            {
                _iRepository.Delete<BasketItem>(basketItem);
                await _iRepository.SaveAsync();
            }

            return await GetBasketItemsAsync(basketItem.UserId);
        }

        public async Task<IList<BasketItem>> ChangeBasketItemQuantityAsync(int id, int quantity)
        {
            var basketItem = await _iRepository.GetByIdAsync<BasketItem>(id);

            if (basketItem == null)
                return null;

            basketItem.Quantity = quantity;

            _iRepository.Update<BasketItem>(basketItem);
            await _iRepository.SaveAsync();
            return await GetBasketItemsAsync(basketItem.UserId);
        }

        #region Private Helper
        /// <summary>
        /// this method will be absolute when we have the actuall database, in memory database does not supoort relation database
        /// </summary>
        private List<BasketItem> PopulateProductIntoBasketItem(List<BasketItem> basketItems)
        {
            foreach (var basketItem in basketItems)
            {
                basketItem.Product = _iRepository.GetById<Product>(basketItem.ProductId);
            }

            return basketItems;
        }

        public async Task<IList<string>> GetShoppingCartWarningsAsync(int userId, int productId, int quantity)
        {
            var warnings = new List<string>();

            var userWarnings = GetUserWarnings(userId);
            if (userWarnings.Any())
            {
                warnings.AddRange(userWarnings);
                return warnings;
            }

            var productWarnings = GetProductWarnings(productId, quantity);
            if (productWarnings.Any())
            {
                warnings.AddRange(productWarnings);
            }

            return warnings;
        }

        public IList<string> GetUserWarnings(int userId)
        {
            var warnings = new List<string>();

            var user = _iRepository.GetById<User>(userId);
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (user.Status != 1)
                warnings.Add("User status is not available for purchase");

            return warnings;
        }

        public IList<string> GetProductWarnings(int productId, int quantity)
        {
            var warnings = new List<string>();

            var product = _iRepository.GetById<Product>(productId);
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            if (product.Status != 1)
                warnings.Add("Product status is not available for purchase");

            if (product.Stock < quantity)
                warnings.Add("Stock not available.");

            if (product.OrderMinimumQuantity > quantity)
                warnings.Add($"The minimum quantity allowed for purchase is {product.OrderMinimumQuantity}");

            if (product.OrderMaximumQuantity < quantity)
                warnings.Add($"The maximum quantity allowed for purchase is {product.OrderMaximumQuantity}");

            return warnings;
        }

        #endregion
    }
}
