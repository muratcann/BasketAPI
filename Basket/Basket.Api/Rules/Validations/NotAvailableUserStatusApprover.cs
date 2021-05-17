using Basket.Api.Entities;
using Basket.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Basket.Api.Rules.Validations
{
    public class NotAvailableUserStatusApprover : RuleApprover
    {
        public override string Message => "User status not available to purchase";
        public override void ProcessRequest(Product product, BasketItemModel basketItem, User user)
        {
            if (user.Status != 1)
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
