using Basket.Model.Dto;
using Basket.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Basket.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IBasketService _iBasketService;
        public ShoppingCartController(IBasketService iBasketService)
        {
            _iBasketService = iBasketService;
        }

        // GET: api/ShoppingCart/GetBasketItems
        [HttpGet("GetBasketItems/{userId}")]
        public async Task<IActionResult> GetBasketItems(int userId)
        {
            IList<BasketItem> basketItems = await _iBasketService.GetBasketItemsAsync(userId);
            return Ok(basketItems);
        }


        // POST api/ShoppingCart
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] BasketItem basketItem)
        {
            var shoppingCartWarnings = await _iBasketService.GetShoppingCartWarningsAsync(basketItem.UserId, basketItem.ProductId, basketItem.Quantity);

            if (shoppingCartWarnings.Any())
                return BadRequest(shoppingCartWarnings.ToArray());

            await _iBasketService.AddItemIntoBasketAsync(basketItem);
            return Created($"ShoppingCart", basketItem);
        }

        // PUT api/ShoppingCart/ChangeItemQuantity/5/4
        [HttpPut("ChangeItemQuantity/{basketItemId}/{quantity}")]
        public async Task<IActionResult> ChangeItemQuantity(int basketItemId, int quantity)
        {
            IList<BasketItem> basketItems = await _iBasketService.ChangeBasketItemQuantityAsync(basketItemId, quantity);
            if (basketItems == null)
                return NotFound("Item not found in the basket, please check the basketItemId");
            return Ok(basketItems);
        }

        // DELETE api/ShoppingCart/ClearBasket/1
        [HttpDelete("ClearBasket/{userId}")]
        public async Task<IActionResult> ClearBasket(int userId)
        {
            IList<BasketItem> basketItems = await _iBasketService.ClearBasketAsync(userId);
            return Ok(basketItems);
        }

        // DELETE api/ShoppingCart/DeleteItemFromBasket/5
        [HttpDelete("DeleteItemFromBasket/{basketItemId}")]
        public async Task<IActionResult> DeleteItemFromBasket(int basketItemId)
        {
            IList<BasketItem> basketItems = await _iBasketService.DeleteBasketItemByIdAsync(basketItemId);
            if (basketItems == null)
                return NotFound("Item not found in the basket, please check the basketItemId");
            return Ok(basketItems);
        }
    }
}
