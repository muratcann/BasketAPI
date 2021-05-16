using Basket.Api.Models;
using Basket.Api.Rules;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Basket.Api.Entities
{
    public class BasketItem : Entity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public int UserId { get; set; }

        public static BasketItem CreateBasketItem(BasketItemModel basketItem, Product product, Coupon coupon)
        {
            CheckRule(new ProductAndBasketItemRule(product, basketItem));

            var couponAmount = coupon?.Amount ?? 0;
            BasketItem newBasketItem = new BasketItem
            {
                Id = ObjectId.GenerateNewId().ToString(),
                ProductId = basketItem.ProductId,
                Quantity = basketItem.Quantity,
                Price = (product.Price - couponAmount),
                Discount = couponAmount,
                UserId = basketItem.UserId
            };

            return newBasketItem;
        }
    }
}
