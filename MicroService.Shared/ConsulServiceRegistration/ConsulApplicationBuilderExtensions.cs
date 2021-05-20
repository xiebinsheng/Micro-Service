using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Serilog;
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
            var logger = new LoggerConfiguration().WriteTo.Async(c => c.File(
                    "Logs/logs1111.txt",
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 31,  //默认就是31
                    shared: true)).CreateLogger();

            // 1. 从IOC容器获取Consul服务注册配置
            var serviceNode = app.ApplicationServices.GetRequiredService<IOptions<ConsulServiceOptions>>().Value;

            logger.Information(JsonConvert.SerializeObject(serviceNode));

            // 2. 获取应用程序生命周期
            var lifetime = app.ApplicationServices.GetRequiredService<IHostApplicationLifetime>();

            // 2.1 获取服务注册实例
            var serviceRegistry = app.ApplicationServices.GetRequiredService<IConsulServiceRegistry>();

            // 3. 获取服务地址
            logger.Information(app.Properties.Count().ToString());

            foreach (var pro in app.Properties)
            {
                logger.Information(pro.Key);
            }

            var features = app.Properties["server.Features"] as FeatureCollection;
            var address = features.Get<IServerAddressesFeature>().Addresses.First();
            var uri = new Uri(address);

            logger.Information("注册前");

            // 4. 注册服务
            serviceNode.ServiceId = serviceNode.ServiceId ?? ($"{serviceNode.ServiceName}-{uri.Host.Replace(".", "_")}_{uri.Port}");
            serviceNode.ServiceAddress = uri.Host;
            serviceNode.ServicePort = uri.Port;
            serviceNode.HealthCheckAddress = $"{uri.Scheme}://{uri.Host}:{uri.Port}{serviceNode.HealthCheckAddress}";
            serviceRegistry.Register(serviceNode);

            logger.Information("完成注册");

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
