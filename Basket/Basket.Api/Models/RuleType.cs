using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Basket.Api.Models
{
    public enum RuleType
    {
        ProductNotFound,
        ProductStatusNotSuitable,
        NoProductStock,
        OrderMinimumQuantityNotReached,
        OrderMaximumQuantityExceeded,
        UserNotFound,
        UserStatusNotSuitable
    }
}
