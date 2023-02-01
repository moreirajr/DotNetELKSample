using Serilog.Core;
using Serilog.Events;

namespace DotNetELKSample.Serilog.Enrichers
{
    public class ErrorEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            foreach (var property in logEvent.Properties)
                propertyFactory.CreateProperty(property.Key, property.Value);
        }
    }
}
