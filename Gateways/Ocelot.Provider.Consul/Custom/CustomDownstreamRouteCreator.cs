using Ocelot.Configuration;
using Ocelot.Configuration.Builder;
using Ocelot.Configuration.Creator;
using Ocelot.DownstreamRouteFinder;
using Ocelot.DownstreamRouteFinder.Finder;
using Ocelot.DownstreamRouteFinder.UrlMatcher;
using Ocelot.LoadBalancer.LoadBalancers;
using Ocelot.Responses;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ocelot.Provider.Consul.Custom
{
    public class CustomDownstreamRouteCreator : IDownstreamRouteProvider
    {
        private readonly IQoSOptionsCreator _qoSOptionsCreator;
        private readonly ConcurrentDictionary<string, OkResponse<DownstreamRouteHolder>> _cache;
        private static string apiBase = "/api/";

        public CustomDownstreamRouteCreator(IQoSOptionsCreator qoSOptionsCreator)
        {
            _qoSOptionsCreator = qoSOptionsCreator;
            _cache = new ConcurrentDictionary<string, OkResponse<DownstreamRouteHolder>>();
        }

        public Response<DownstreamRouteHolder> Get(string upstreamUrlPath, string upstreamQueryString, string upstreamHttpMethod, IInternalConfiguration configuration, string upstreamHost)
        {
            var serviceName = IsSystemApi(upstreamUrlPath) ? "BaseService" : GetServiceName(upstreamUrlPath);
            //var downstreamPath = IsSystemApi(upstreamUrlPath) ? upstreamUrlPath : GetDownstreamPath(upstreamUrlPath);
            var downstreamPath = upstreamUrlPath;

            if (HasQueryString(downstreamPath))
            {
                downstreamPath = RemoveQueryString(downstreamPath);
            }

            var downstreamPathForKeys = $"/{serviceName}{downstreamPath}";

            var loadBalancerKey = CreateLoadBalancerKey(downstreamPathForKeys, upstreamHttpMethod, configuration.LoadBalancerOptions);

            if (_cache.TryGetValue(loadBalancerKey, out var downstreamRouteHolder))
            {
                return downstreamRouteHolder;
            }

            var qosOptions = _qoSOptionsCreator.Create(configuration.QoSOptions, downstreamPathForKeys, new List<string> { upstreamHttpMethod });

            var upstreamPathTemplate = new UpstreamPathTemplateBuilder().WithOriginalValue(upstreamUrlPath).Build();

            var downstreamRouteBuilder = new DownstreamRouteBuilder()
                .WithServiceName(serviceName)
                .WithLoadBalancerKey(loadBalancerKey)
                .WithDownstreamPathTemplate(downstreamPath)
                .WithUseServiceDiscovery(true)
                .WithHttpHandlerOptions(configuration.HttpHandlerOptions)
                .WithQosOptions(qosOptions)
                .WithDownstreamScheme(configuration.DownstreamScheme)
                .WithLoadBalancerOptions(configuration.LoadBalancerOptions)
                .WithDownstreamHttpVersion(configuration.DownstreamHttpVersion)
                .WithUpstreamPathTemplate(upstreamPathTemplate);

            var rateLimitOptions = configuration.Routes != null
                ? configuration.Routes.SelectMany(t => t.DownstreamRoute).FirstOrDefault(t => t.ServiceName == serviceName) : null;

            if (rateLimitOptions != null)
            {
                downstreamRouteBuilder
                    .WithRateLimitOptions(rateLimitOptions.RateLimitOptions)
                    .WithEnableRateLimiting(true);
            }

            var downstreamRoute = downstreamRouteBuilder.Build();

            var route = new RouteBuilder()
                .WithDownstreamRoute(downstreamRoute)
                .WithUpstreamHttpMethod(new List<string>() { upstreamHttpMethod })
                .WithUpstreamPathTemplate(upstreamPathTemplate)
                .Build();

            downstreamRouteHolder = new OkResponse<DownstreamRouteHolder>(new DownstreamRouteHolder(new List<PlaceholderNameAndValue>(), route));

            _cache.AddOrUpdate(loadBalancerKey, downstreamRouteHolder, (x, y) => downstreamRouteHolder);

            return downstreamRouteHolder;
        }

        /// <summary>
        /// 确定路由是不是系统API
        /// </summary>
        /// <param name="upstreamUrlPath"></param>
        /// <returns></returns>
        private bool IsSystemApi(string upstreamUrlPath)
        {
            // 系统api列表
            List<string> systemApi = new List<string>()
            {
                "/api/abp/","/api/account/","/api/permission-management/"
            };

            foreach (var item in systemApi)
            {
                if (upstreamUrlPath.IndexOf(item) != -1)
                {
                    return true;
                }
            }
            return false;
        }

        private string CreateLoadBalancerKey(string downstreamTemplatePath, string httpMethod, LoadBalancerOptions loadBalancerOptions)
        {
            if (!string.IsNullOrEmpty(loadBalancerOptions.Type)
                && !string.IsNullOrEmpty(loadBalancerOptions.Key)
                && loadBalancerOptions.Type == nameof(CookieStickySessions))
            {
                return $"{nameof(CookieStickySessions)}:{loadBalancerOptions.Key}";
            }
            return CreateQosKey(downstreamTemplatePath, httpMethod);
        }

        private string CreateQosKey(string downstreamTemplatePath, string httpMethod)
        {
            var loadBalancerKey = $"{downstreamTemplatePath}|{httpMethod}";
            return loadBalancerKey;
        }

        private static bool HasQueryString(string downstreamPath)
        {
            return downstreamPath.Contains("?");
        }

        private static string RemoveQueryString(string downstreamPath)
        {
            return downstreamPath.Substring(0, downstreamPath.IndexOf("?"));
        }

        /// <summary>
        /// 根据上游地址获取服务名称
        /// </summary>
        /// <param name="upstreamUrlPath"></param>
        /// <returns></returns>
        private static string GetServiceName(string upstreamUrlPath)
        {
            if (upstreamUrlPath.IndexOf("/", 1) == -1)
            {
                return upstreamUrlPath.Substring(1);
            }
            if (upstreamUrlPath.IndexOf(apiBase, 0) == -1)
            {
                return upstreamUrlPath.Substring(1, upstreamUrlPath.IndexOf('/', 1)).TrimEnd('/');
            }
            else
            {
                upstreamUrlPath = upstreamUrlPath.Replace(apiBase, string.Empty);
                return upstreamUrlPath.Substring(0, upstreamUrlPath.IndexOf('/', 1)).TrimEnd('/');
            }
        }

        /// <summary>
        /// 根据上游地址获取下游
        /// </summary>
        /// <param name="upstreamUrlPath"></param>
        /// <returns></returns>
        private static string GetDownstreamPath(string upstreamUrlPath)
        {
            if (upstreamUrlPath.IndexOf('/', 1) == -1)
            {
                return "/";
            }
            if (upstreamUrlPath.IndexOf(apiBase, 0) == -1)
            {
                return upstreamUrlPath.Substring(upstreamUrlPath.IndexOf('/', 1));
            }
            else
            {
                upstreamUrlPath = upstreamUrlPath.Replace(apiBase, string.Empty);
                return upstreamUrlPath.Substring(upstreamUrlPath.IndexOf('/', 1));
            }
        }
    }
}
