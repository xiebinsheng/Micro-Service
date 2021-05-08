namespace Ocelot.Provider.Consul
{
    using global::Consul;
    using Infrastructure.Extensions;
    using Logging;
    using MicroService.Shared.Consts;
    using Microsoft.Extensions.Caching.StackExchangeRedis;
    using Newtonsoft.Json;
    using ServiceDiscovery.Providers;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Values;

    public class Consul : IServiceDiscoveryProvider
    {
        private readonly RedisCache _redisCache;
        private readonly ConsulRegistryConfiguration _config;
        private readonly IOcelotLogger _logger;
        private readonly IConsulClient _consul;
        private const string VersionPrefix = "version-";

        public Consul(
            ConsulRegistryConfiguration config,
            IOcelotLoggerFactory factory,
            IConsulClientFactory clientFactory,
            RedisCache redisCache)
        {
            _redisCache = redisCache;
            _logger = factory.CreateLogger<Consul>();
            _config = config;
            _consul = clientFactory.Get(_config);
        }


        public async Task<List<Service>> Get()
        {
            // 如果Consul有问题会导致网关异常
            //var queryResult = await _consul.Health.Service(_config.KeyOfServiceInConsul, string.Empty, true);
            if (_config.KeyOfServiceInConsul == string.Empty)
            {
                return new List<Service>();
            }
            QueryResult<ServiceEntry[]> queryResult = new QueryResult<ServiceEntry[]>();
            try
            {
                queryResult = await _consul.Health.Service(_config.KeyOfServiceInConsul, string.Empty, true);
            }
            catch (Exception ex)
            {
                _logger.LogError($"_consul.Health.Service异常{_config.KeyOfServiceInConsul}", ex);
                return await GetServiceFromCache(_config.KeyOfServiceInConsul);
            }

            var services = new List<Service>();

            foreach (var serviceEntry in queryResult.Response)
            {
                if (IsValid(serviceEntry))
                {
                    try
                    {
                        var nodes = await _consul.Catalog.Nodes();
                        if (nodes.Response == null)
                        {
                            services.Add(BuildService(serviceEntry, null));
                        }
                        else
                        {
                            var serviceNode = nodes.Response.FirstOrDefault(n => n.Address == serviceEntry.Service.Address);
                            services.Add(BuildService(serviceEntry, serviceNode));
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"_consul.Catalog.Nodes异常{serviceEntry.Service}", ex);
                        return await GetServiceFromCache(_config.KeyOfServiceInConsul);
                    }
                }
                else
                {
                    _logger.LogWarning($"Unable to use service Address: {serviceEntry.Service.Address} and Port: {serviceEntry.Service.Port} as it is invalid. Address must contain host only e.g. localhost and port must be greater than 0");
                }
            }

            await ServiceToCache(_config.KeyOfServiceInConsul, services);
            return services.ToList();
        }

        private async Task ServiceToCache(string serviceName, List<Service> services)
        {
            List<ServiceTemp> serviceExtensions = services.Select(t =>
            {
                ServiceTemp serviceExtension = new ServiceTemp
                {
                    Id = t.Id,
                    Name = t.Name,
                    Version = t.Version,
                    Tags = t.Tags,
                    DownstreamHost = t.HostAndPort.DownstreamHost,
                    DownstreamPort = t.HostAndPort.DownstreamPort,
                    Scheme = t.HostAndPort.Scheme
                };
                return serviceExtension;
            }).ToList();
            await _redisCache.SetAsync(
                MicroServiceConsts.RedisKeyServicePrefix + "-" + serviceName,
                ASCIIEncoding.Default.GetBytes(JsonConvert.SerializeObject(serviceExtensions)),
                new Microsoft.Extensions.Caching.Distributed.DistributedCacheEntryOptions());
        }

        private async Task<List<Service>> GetServiceFromCache(string serviceName)
        {
            var serviceByte = await _redisCache.GetAsync(MicroServiceConsts.RedisKeyServicePrefix + "-" + serviceName);
            if (serviceByte == null)
            {
                return new List<Service>();
            }
            List<ServiceTemp> serviceTemps = JsonConvert.DeserializeObject<List<ServiceTemp>>(Encoding.Default.GetString(serviceByte));
            return serviceTemps
                .Select(t =>
                {
                    return t.ToService();
                })
                .ToList();
        }

        private Service BuildService(ServiceEntry serviceEntry, Node serviceNode)
        {
            return new Service(
                serviceEntry.Service.Service,
                new ServiceHostAndPort(serviceNode == null ? serviceEntry.Service.Address : serviceNode.Name, serviceEntry.Service.Port),
                serviceEntry.Service.ID,
                GetVersionFromStrings(serviceEntry.Service.Tags),
                serviceEntry.Service.Tags ?? Enumerable.Empty<string>());
        }

        private bool IsValid(ServiceEntry serviceEntry)
        {
            if (string.IsNullOrEmpty(serviceEntry.Service.Address) || serviceEntry.Service.Address.Contains("http://") || serviceEntry.Service.Address.Contains("https://") || serviceEntry.Service.Port <= 0)
            {
                return false;
            }

            return true;
        }

        private string GetVersionFromStrings(IEnumerable<string> strings)
        {
            return strings
                ?.FirstOrDefault(x => x.StartsWith(VersionPrefix, StringComparison.Ordinal))
                .TrimStart(VersionPrefix);
        }
    }
}
