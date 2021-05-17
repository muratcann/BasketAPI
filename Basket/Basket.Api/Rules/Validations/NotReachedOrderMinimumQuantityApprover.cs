using Basket.Api.Entities;
using Basket.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Basket.Api.Rules.Validations
{
    public class NotReachedOrderMinimumQuantityApprover : RuleApprover
    {
        public override string Message => "Order minimum quantity not reached";
        public override void ProcessRequest(Product product, BasketItemModel basketItem, User user)
        {
            if (basketItem.Quantity < product.OrderMinimumQuantity)
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
