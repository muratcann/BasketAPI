using Basket.Api.Entities;
using Basket.Api.Models;
using Basket.Api.Rules.Exeption;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Basket.Api.Rules.Validations
{
    public abstract class RuleApprover
    {
        protected RuleApprover successor;
        public abstract string Message { get; }

        public void SetSuccessor(RuleApprover successor)
        {
            this.successor = successor;
        }

        public abstract void ProcessRequest(Product product, BasketItemModel basketItem, User user);

        public void ThrowRuleException()
        {
            throw new BusinessRuleValidationException(this);
        }
    }
}
