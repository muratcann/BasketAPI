using Basket.Api.Entities;
using Basket.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Basket.Api.Rules.Validations
{
    public class ExceededOrderMaximumQuantityApprover : RuleApprover
    {
        public override string Message => "Order maximum quantity exceeded";
        public override void ProcessRequest(Product product, BasketItemModel basketItem, User user)
        {
            if (product == null)
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
