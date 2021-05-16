using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Basket.Api.Rules
{
    public interface IBusinessRule
    {
        bool IsBroken();
        string Message();
    }
}
