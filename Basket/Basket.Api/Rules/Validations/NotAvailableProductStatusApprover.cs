using Basket.Api.Entities;
using Basket.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Basket.Api.Rules.Validations
{
    public class NotAvailableProductStatusApprover : RuleApprover
    {
        public override string Message => "Product status not available to sale";
        public override void ProcessRequest(Product product, BasketItemModel basketItem, User user)
        {
            if (product.Status != 1)
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
