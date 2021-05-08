using Microsoft.Extensions.DependencyInjection;
using Ocelot.Configuration;
using Ocelot.DownstreamRouteFinder.Finder;
using Ocelot.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ocelot.Provider.Consul.Custom
{
    public class CustomDownstreamRouteProviderFactory : IDownstreamRouteProviderFactory
    {
        private readonly Dictionary<string, IDownstreamRouteProvider> _providers;
        private readonly IOcelotLogger _logger;

        public CustomDownstreamRouteProviderFactory(IServiceProvider provider, IOcelotLoggerFactory factory)
        {
            _logger = factory.CreateLogger<DownstreamRouteProviderFactory>();
            _providers = provider.GetServices<IDownstreamRouteProvider>().ToDictionary(t => t.GetType().Name);
        }

        public IDownstreamRouteProvider Get(IInternalConfiguration config)
        {
            //todo - this is a bit hacky we are saying there are no routes or there are routes but none of them have
            //an upstream path template which means they are dyanmic and service discovery is on...
            if ((!config.Routes.Any()
                || config.Routes.All(t => string.IsNullOrEmpty(t.UpstreamTemplatePattern?.OriginalValue)))
                && IsServiceDiscovery(config.ServiceProviderConfiguration))
            {
                _logger.LogInformation($"Selected {nameof(DownstreamRouteCreator)} as DownstreamRouteProvider for this request");
                return _providers.Values.LastOrDefault();
            }

            return _providers[nameof(DownstreamRouteFinder)];
        }

        private bool IsServiceDiscovery(ServiceProviderConfiguration config)
        {
            if (!string.IsNullOrEmpty(config?.Host) && config?.Port > 0 && !string.IsNullOrEmpty(config.Type))
            {
                return true;
            }
            return false;
        }
    }
}
