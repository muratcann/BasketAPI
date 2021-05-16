using Basket.Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Basket.Api.Models
{
    public class Basket
    {
        public List<BasketItem> Items { get; set; } = new List<BasketItem>();

        public decimal TotalPrice
        {
            get
            {
                decimal totalprice = 0;
                foreach (var item in Items)
                {
                    totalprice += item.Price * item.Quantity;
                }
                return totalprice;
            }
        }

        public decimal TotalDiscount
        {
            get
            {
                decimal totaldiscount = 0;
                foreach (var item in Items)
                {
                    totaldiscount += item.Discount * item.Quantity;
                }
                return totaldiscount;
            }
        }
    }
}
