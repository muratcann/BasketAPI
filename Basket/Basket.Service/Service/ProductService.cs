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
    public class ProductService : IProductService
    {
        private readonly IRepositoryReadOnly _iRepository;
        public ProductService(IRepositoryReadOnly iRepository)
        {
            _iRepository = iRepository;
        }

        /// <summary>
        /// Get All List of Products from Product Table
        /// </summary>
        /// <returns>return list of products</returns>
        public async Task<IList<Product>> GetProductsAsync()
        {
            var products = await _iRepository.GetAllAsync<Product>();
            return products.ToList();
        }

        /// <summary>
        /// Get Product from product by productId
        /// </summary>
        /// <param name="productId"></param>
        /// <returns>return single product</returns>
        public async Task<Product> GetProductAsync(int productId)
        {
            var product = await _iRepository.GetOneAsync<Product>(p => p.Id == productId);
            return product;
        }
    }
}
