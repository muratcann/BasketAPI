using Basket.Api.Entities;
using Basket.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Basket.Api.Services
{
    public interface IBasketService
    {
        Task<Models.Basket> GetBasket(int userId);
        Task<bool> AddBasketItem(BasketItemModel basketItem);
        Task<bool> UpdateBasketItem(BasketItem item, int quantity);
        Task<bool> UpdateBasketItem(BasketItem basketItem);
        Task<bool> DeleteBasketItem(string basketItemId);
        Task<bool> ClearBasket(int userId);
    }
}
