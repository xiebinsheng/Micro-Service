using Consul;
using System;

namespace MicroService.Shared.ConsulServiceRegistration
{
    public class ConsulServiceRegistry : IConsulServiceRegistry
    {
        public void Register(ConsulServiceOptions settings)
        {
            // 1. 创建Consul客户端链接
            var consulClient = new ConsulClient(config =>
            {
                // 1.1 建立客户端和服务端链接
                config.Address = new Uri(settings.ConsulAddress);
            });

            // 2. 创建Consul服务注册对象
            var registration = new AgentServiceRegistration()
            {
                ID = settings.ServiceId,
                Name = settings.ServiceName,
                Address = settings.ServiceAddress,
                Port = settings.ServicePort,
                Tags = settings.ServiceTags,
                Check = new AgentServiceCheck
                {
                    // 2.1 Consul健康检查时间
                    Timeout = TimeSpan.FromSeconds(settings.HealthCheckTimeOut),
                    // 2.2 服务停止5秒后注销服务
                    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),
                    // 2.3 Consul健康检查地址
                    HTTP = settings.HealthCheckAddress,
                    // 2.4 Consul健康检查间隔时间
                    Interval = TimeSpan.FromSeconds(settings.HealthCheckInterval),
                    // 2.5 是否跳过传输层安全协议（Transport Layer Security），默认true
                    TLSSkipVerify = true
                }
            };

            // 3. 注册服务
            consulClient.Agent.ServiceRegister(registration).Wait();

            // 4. 关闭链接
            consulClient.Dispose();
        }

        /// <summary>
        /// 注销服务
        /// </summary>
        /// <param name="settings"></param>
        public void Deregister(ConsulServiceOptions settings)
        {
            // 1. 创建Consul客户端链接
            var consulClient = new ConsulClient(config =>
            {
                // 1.1 建立客户端和服务端链接
                config.Address = new Uri(settings.ConsulAddress);
            });

            // 2. 注销服务
            consulClient.Agent.ServiceDeregister(settings.ConsulAddress);

            // 3. 关闭链接
            consulClient.Dispose();
        }
    }
}
