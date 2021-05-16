using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Basket.Api.Logging.Implementations
{
    public class ApiExceptionOptions
    {
        public Action<HttpContext, Exception, ApiError> AddResponseDetails { get; set; }
        public Func<Exception, LogLevel> DetermineLogLevel { get; set; }
    }
}
