using System.Collections.Generic;

namespace Basket.Api.Logging
{
    public interface IScopeInformation
    {
        Dictionary<string, string> HostScopeInfo { get; }
    }
}
