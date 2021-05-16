using Basket.Api.Entities;
using Basket.Api.Models;
using Basket.Api.Repositories;
using Basket.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Basket.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketService _service;
        public BasketController(IBasketService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        [HttpPost("AddBasketItem")]
        [ProducesResponseType(typeof(BasketItem), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<BasketItem>> AddBasketItem([FromBody] BasketItemModel basketItem)
        {
            await _service.AddBasketItem(basketItem);

            return CreatedAtRoute("GetBasket", new { userId = basketItem.UserId }, basketItem);
        }

        [HttpGet("GetBasket/{userId}", Name = "GetBasket")]
        [ProducesResponseType(typeof(IEnumerable<BasketItem>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<BasketItem>>> GetBasket(int userId)
        {
            var basket = await _service.GetBasket(userId);
            return Ok(basket);
        }

        [HttpPut("UpdateBasketItem")]
        [ProducesResponseType(typeof(BasketItem), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateBasketItem([FromBody] BasketItem basketItem)
        {
            return Ok(await _service.UpdateBasketItem(basketItem));
        }

        [HttpDelete("DeleteBasketItemById/{id:length(24)}")]
        [ProducesResponseType(typeof(BasketItem), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteBasketItemById(string id)
        {
            return Ok(await _service.DeleteBasketItem(id));
        }

        [HttpDelete("ClearBasket/{userId}")]
        [ProducesResponseType(typeof(BasketItem), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ClearBasketItems(int userId)
        {
            return Ok(await _service.ClearBasket(userId));
        }
    }
}
