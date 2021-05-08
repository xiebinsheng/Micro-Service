using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Linq;

namespace MicroService.Shared.ConsulServiceRegistration
{
    /// <summary>
    /// 服务注册发现扩展
    /// </summary>
    public static class ConsulApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseConsulRegistry(this IApplicationBuilder app)
        {
            // 1. 从IOC容器获取Consul服务注册配置
            var serviceNode = app.ApplicationServices.GetRequiredService<IOptions<ConsulServiceOptions>>().Value;

            // 2. 获取应用程序生命周期
            var lifetime = app.ApplicationServices.GetRequiredService<IHostApplicationLifetime>();

            // 2.1 获取服务注册实例
            var serviceRegistry = app.ApplicationServices.GetRequiredService<IConsulServiceRegistry>();

            // 3. 获取服务地址
            var features = app.Properties["server.Features"] as FeatureCollection;
            var address = features.Get<IServerAddressesFeature>().Addresses.First();
            var uri = new Uri(address);

            // 4. 注册服务
            serviceNode.ServiceId = serviceNode.ServiceId ?? ($"{serviceNode.ServiceName}-{uri.Host.Replace(".", "_")}_{uri.Port}");
            serviceNode.ServiceAddress = uri.Host;
            serviceNode.ServicePort = uri.Port;
            serviceNode.HealthCheckAddress = $"{uri.Scheme}://{uri.Host}:{uri.Port}{serviceNode.HealthCheckAddress}";
            serviceRegistry.Register(serviceNode);

            // 5. 服务关闭时注销服务
            lifetime.ApplicationStopping.Register(() =>
            {
                //Log.Information("Shut Down BaseService.Host.");
                serviceRegistry.Deregister(serviceNode);
            });

            return app;
        }
    }
}
