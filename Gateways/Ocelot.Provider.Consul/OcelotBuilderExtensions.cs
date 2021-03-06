namespace Ocelot.Provider.Consul
{
    using Configuration.Repository;
    using DependencyInjection;
    using Microsoft.Extensions.Caching.StackExchangeRedis;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Middleware;
    using Ocelot.DownstreamRouteFinder.Finder;
    using Ocelot.Provider.Consul.Custom;
    using ServiceDiscovery;

    public static class OcelotBuilderExtensions
    {
        public static IOcelotBuilder AddConsul(this IOcelotBuilder builder)
        {
            builder.Services.AddSingleton<ServiceDiscoveryFinderDelegate>(ConsulProviderFactory.Get);
            builder.Services.AddSingleton<IConsulClientFactory, ConsulClientFactory>();

            builder.Services.AddSingleton<IDownstreamRouteProvider, CustomDownstreamRouteCreator>();
            builder.Services.AddSingleton<IDownstreamRouteProviderFactory,CustomDownstreamRouteProviderFactory>();
            builder.Services.AddTransient<RedisCache>();

            builder.Services.RemoveAll(typeof(IFileConfigurationPollerOptions));
            builder.Services.AddSingleton<IFileConfigurationPollerOptions, ConsulFileConfigurationPollerOption>();
            return builder;
        }

        public static IOcelotBuilder AddConfigStoredInConsul(this IOcelotBuilder builder)
        {
            builder.Services.AddSingleton<OcelotMiddlewareConfigurationDelegate>(ConsulMiddlewareConfigurationProvider.Get);
            builder.Services.AddHostedService<FileConfigurationPoller>();
            builder.Services.AddSingleton<IFileConfigurationRepository, ConsulFileConfigurationRepository>();
            return builder;
        }
    }
}
