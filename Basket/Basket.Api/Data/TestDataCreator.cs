using Basket.Api.Entities;
using Basket.Api.Repositories.EntityFramework;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Basket.Api.Data
{
    public class TestDataCreator
    {
        private IDistributedCache _redisCache;

        public TestDataCreator(IDistributedCache redisCache)
        {
            _redisCache = redisCache;
        }

        public void AddTestData(EFDbContext context)
        {
            //products added to inmemorydatabase with entityframework
            if (context.Products.Count() == 0)
            {
                IList<Product> products = new List<Product>();
                products.Add(new Product { Id = 1, Name = "Tişört", Price = 40.00M, Photo = "", Description = "Kırmızı Tişört", Stock = 100, OrderMinimumQuantity = 1, OrderMaximumQuantity = 100, Status = 1 });
                products.Add(new Product { Id = 2, Name = "Gömlek", Price = 70.00M, Photo = "", Description = "Beyaz Gömlek", Stock = 500, OrderMinimumQuantity = 1, OrderMaximumQuantity = 100, Status = 1 });
                products.Add(new Product { Id = 3, Name = "Kazak", Price = 60.00M, Photo = "", Description = "İnce Kazak", Stock = 70, OrderMinimumQuantity = 10, OrderMaximumQuantity = 100, Status = 1 });
                products.Add(new Product { Id = 4, Name = "Çorap", Price = 15.00M, Photo = "", Description = "Soket Çorap", Stock = 60, OrderMinimumQuantity = 1, OrderMaximumQuantity = 100, Status = 0 });
                products.Add(new Product { Id = 5, Name = "Ayakkabı", Price = 120.00M, Photo = "", Description = "Spor Ayakkabı", Stock = 0, OrderMinimumQuantity = 1, OrderMaximumQuantity = 100, Status = 1 });
                context.Products.AddRange(products);
            }

            //users added to inmemorydatabase with entityframework
            if (context.Users.Count() == 0)
            {
                IList<User> users = new List<User>();
                users.Add(new User { Id = 1, Name = "Ahmet", Email = "ahmet@gmail.com", Status = 1 });
                users.Add(new User { Id = 2, Name = "Ayşe", Email = "ayse@gmail.com", Status = 0 });
                context.Users.AddRange(users);
            }

            context.SaveChanges();

            //coupons added to redis
            IList<Coupon> coupons = new List<Coupon>();
            coupons.Add(new Coupon { Id = 2, ProductId = 2, Description = "product 2 coupon", Amount = 10 });
            foreach (var item in coupons)
            {
                _redisCache.SetStringAsync($"coupon_{item.ProductId}", JsonConvert.SerializeObject(item));
            }
        }
    }
}
