using Serilog.Core;
using Serilog.Events;

namespace TekhneCafe.Api.LoggerEnrichers
{
    public class UserEnricher : ILogEventEnricher
    {
        public UserEnricher()
        {

        }

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var (username, value) = logEvent.Properties.FirstOrDefault(x => x.Key == "UserId");
            if (value != null)
            {
                var enrichProperty = propertyFactory.CreateProperty(username, value);
                logEvent.AddPropertyIfAbsent(enrichProperty);
            }
        }
    }
}
