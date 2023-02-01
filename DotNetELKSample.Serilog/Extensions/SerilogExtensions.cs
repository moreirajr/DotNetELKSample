using Microsoft.Extensions.Logging;
using Serilog.Context;
using System.Collections.Generic;

namespace DotNetELKSample.Serilog.Extensions
{
    public static class SerilogExtensions
    {
        public static void EnrichProperty<T>(this ILogger<T> logger, Dictionary<string, object> enrichProperties)
        {
            if (enrichProperties != default)
                foreach (var property in enrichProperties)
                    LogContext.PushProperty(property.Key, property.Value);
        }

        public static void EnrichProperty<T>(this ILogger<T> logger, string property, object value)
        {
            LogContext.PushProperty(property, value);
        }
    }
}
