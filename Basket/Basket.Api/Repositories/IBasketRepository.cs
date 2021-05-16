using Basket.Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Basket.Api.Repositories
{
    public interface IBasketRepository
    {
        Task<IEnumerable<BasketItem>> GetBasket(int userId);
        Task<BasketItem> GetBasketItemByProductId(int userId, int productId);
        Task<bool> AddBasketItem(BasketItem basketItem);
        Task<bool> UpdateBasketItem(BasketItem basketItem);
        Task<bool> DeleteBasketItem(string basketItemId);
        Task<bool> ClearBasket(int userId);
    }
}
