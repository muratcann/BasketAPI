using Basket.Model.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.Service.Interface
{
    public interface IProductService
    {
        Task<IList<Product>> GetProductsAsync();
        Task<Product> GetProductAsync(int productId);
    }
}
