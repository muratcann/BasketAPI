using Basket.Api.Rules;
using Basket.Api.Rules.Exeption;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Basket.Api.Entities
{
    public class Entity
    {
        protected static void CheckRule(IBusinessRule rule)
        {
            if (!rule.IsBroken())
            {
                throw new BusinessRuleValidationException(rule);
            }
        }
    }
}
