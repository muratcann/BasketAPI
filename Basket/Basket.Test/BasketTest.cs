using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Basket.Api.Services;
using Basket.Test.SeedWork;
using Basket.Api.Controllers;
using Basket.Api.Models;
using Basket.Api.Repositories.EntityFramework;
using Basket.Api.Repositories;
using Microsoft.Extensions.Caching.Distributed;
using Basket.Api.Data;
using Basket.Api.Rules.Exeption;

namespace Basket.Test
{
    [TestClass]
    public class BasketTest
    {
        private readonly IBasketService _service;
        private readonly EFDbContext _eFDbContext;
        private readonly IRepositoryReadOnly _repositoryReadOnly;
        private readonly IBasketRepository _basketRepository;
        private readonly IDistributedCache _redisCache;

        public BasketTest()
        {
            var serviceProvider = SetupFactory.Create();
            _eFDbContext = serviceProvider.GetService<EFDbContext>();
            _repositoryReadOnly = serviceProvider.GetService<IRepositoryReadOnly>();
            _basketRepository = serviceProvider.GetService<IBasketRepository>();
            _redisCache = serviceProvider.GetService<IDistributedCache>();

            _service = new BasketService(_basketRepository, _repositoryReadOnly, _redisCache);
            //TestDataCreator.AddTestData(_eFDbContext);
            TestDataCreator testDataCreator = new TestDataCreator(_redisCache);
            testDataCreator.AddTestData(_eFDbContext);
        }

        [TestMethod]
        public async Task AddBasketItem_Test()
        {
            BasketItemModel basketItem = new BasketItemModel
            {
                ProductId = 1,
                Quantity = 1,
                UserId = 1
            };
            var result = await _service.AddBasketItem(basketItem);

            Assert.IsNotNull(result);
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public async Task AddBasketItem_WithNonProduct_Test()
        {
            try
            {
                BasketItemModel basketItem = new BasketItemModel
                {
                    ProductId = 100,
                    Quantity = 1,
                    UserId = 1
                };
                await _service.AddBasketItem(basketItem);
            }
            catch (BusinessRuleValidationException ex)
            {
                Assert.AreEqual("Product not found", ex.Message);
            }
        }

        [TestMethod]
        public async Task AddBasketItem_WithNotAvailableProductStatus_Test()
        {
            try
            {
                BasketItemModel basketItem = new BasketItemModel
                {
                    ProductId = 4,
                    Quantity = 1,
                    UserId = 1
                };
                await _service.AddBasketItem(basketItem);
            }
            catch (BusinessRuleValidationException ex)
            {
                Assert.AreEqual("Product status not available to sale", ex.Message);
            }
        }

        [TestMethod]
        public async Task AddBasketItem_WithNoProductStock_Test()
        {
            try
            {
                BasketItemModel basketItem = new BasketItemModel
                {
                    ProductId = 1,
                    Quantity = 10000,
                    UserId = 1
                };
                await _service.AddBasketItem(basketItem);
            }
            catch (BusinessRuleValidationException ex)
            {
                Assert.AreEqual("There is no product stock", ex.Message);
            }
        }

        [TestMethod]
        public async Task AddBasketItem_WithNotReachedOrderMinimumQuantityTest()
        {
            try
            {
                BasketItemModel basketItem = new BasketItemModel
                {
                    ProductId = 3,
                    Quantity = 2,
                    UserId = 1
                };
                await _service.AddBasketItem(basketItem);
            }
            catch (BusinessRuleValidationException ex)
            {
                Assert.AreEqual("Order minimum quantity not reached", ex.Message);
            }
        }

        [TestMethod]
        public async Task AddBasketItem_WithExceededOrderMaximumQuantityTest()
        {
            try
            {
                BasketItemModel basketItem = new BasketItemModel
                {
                    ProductId = 2,
                    Quantity = 222,
                    UserId = 1
                };
                await _service.AddBasketItem(basketItem);
            }
            catch (BusinessRuleValidationException ex)
            {
                Assert.AreEqual("Order maximum quantity exceeded", ex.Message);
            }
        }

        [TestMethod]
        public async Task AddBasketItem_WithNonUserTest()
        {
            try
            {
                BasketItemModel basketItem = new BasketItemModel
                {
                    ProductId = 1,
                    Quantity = 1,
                    UserId = 1121
                };
                await _service.AddBasketItem(basketItem);
            }
            catch (BusinessRuleValidationException ex)
            {
                Assert.AreEqual("User not found", ex.Message);
            }
        }

        [TestMethod]
        public async Task AddBasketItem_WithNonAvailableUserStatusTest()
        {
            try
            {
                BasketItemModel basketItem = new BasketItemModel
                {
                    ProductId = 1,
                    Quantity = 1,
                    UserId = 2
                };
                await _service.AddBasketItem(basketItem);
            }
            catch (BusinessRuleValidationException ex)
            {
                Assert.AreEqual("User status not available to purchase", ex.Message);
            }
        }

        [TestMethod]
        public async Task GetBasket_Test()
        {
            int userId = 1;
            var result = await _service.GetBasket(userId);

            Assert.IsNotNull(result);
        }




        /*
        [TestMethod]
        public async Task AddBasketItemWithCouponTest()
        {

        }
        */
    }
}
