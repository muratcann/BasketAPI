using Basket.Api.Data;
using Basket.Api.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Basket.Api.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IBasketContext _context;

        public BasketRepository(IBasketContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<BasketItem>> GetBasket(int userId)
        {
            return await _context
                            .BasketItems
                            .Find(p => p.UserId == userId)
                            .ToListAsync();
        }

        public async Task<BasketItem> GetBasketItemByProductId(int userId, int productId)
        {
            return await _context
                            .BasketItems
                            .Find(p => p.UserId == userId && p.ProductId == productId)
                            .FirstOrDefaultAsync();
        }

        public async Task<BasketItem> GetBasketItem(string basketItemId)
        {
            return await _context
                           .BasketItems
                           .Find(p => p.Id == basketItemId)
                           .FirstOrDefaultAsync();
        }

        public async Task<bool> AddBasketItem(BasketItem basketItem)
        {
            try
            {
                await _context.BasketItems.InsertOneAsync(basketItem);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateBasketItem(BasketItem basketItem)
        {
            var updateResult = await _context
                                        .BasketItems
                                        .ReplaceOneAsync(filter: g => g.Id == basketItem.Id, replacement: basketItem);

            return updateResult.IsAcknowledged
                    && updateResult.ModifiedCount > 0;
        }

        public async Task<bool> DeleteBasketItem(string basketItemId)
        {
            FilterDefinition<BasketItem> filter = Builders<BasketItem>.Filter.Eq(p => p.Id, basketItemId);

            DeleteResult deleteResult = await _context
                                                .BasketItems
                                                .DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged
                && deleteResult.DeletedCount > 0;
        }

        public async Task<bool> ClearBasket(int userId)
        {
            FilterDefinition<BasketItem> filter = Builders<BasketItem>.Filter.Eq(p => p.UserId, userId);

            DeleteResult deleteResult = await _context
                                                .BasketItems
                                                .DeleteManyAsync(filter);

            return deleteResult.IsAcknowledged
                && deleteResult.DeletedCount > 0;
        }
    }
}
