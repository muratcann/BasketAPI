using Basket.Model.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.Service.Interface
{
    public interface IBasketService
    {
        Task<BasketItem> AddItemIntoBasketAsync(BasketItem basketItem);
        Task<IList<BasketItem>> ChangeBasketItemQuantityAsync(int id, int quantity);
        Task<IList<BasketItem>> ClearBasketAsync(int userId);
        Task<IList<BasketItem>> DeleteBasketItemByIdAsync(int id);
        Task<IList<BasketItem>> GetBasketItemsAsync(int userId);
        Task<IList<string>> GetShoppingCartWarningsAsync(int userId, int productId, int quantity);
    }
}
