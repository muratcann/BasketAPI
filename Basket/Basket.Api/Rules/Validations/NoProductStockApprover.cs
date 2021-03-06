using Basket.Api.Entities;
using Basket.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Basket.Api.Rules.Validations
{
    public class NoProductStockApprover : RuleApprover
    {
        public override string Message => "There is no product stock";
        public override void ProcessRequest(Product product, BasketItemModel basketItem, User user)
        {
            if (product.Stock < basketItem.Quantity)
            {
                ThrowRuleException();
            }
            else if (successor != null)
            {
                successor.ProcessRequest(product, basketItem, user);
            }
        }
    }
}
