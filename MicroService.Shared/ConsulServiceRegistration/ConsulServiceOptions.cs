using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroService.Shared.ConsulServiceRegistration
{
    public class ConsulServiceOptions
    {
        /// <summary>
        /// 服务ID
        /// </summary>
        public string ServiceId { get; set; }

        /// <summary>
        /// 服务名称
        /// </summary>
        public string ServiceName { get; set; }

        /// <summary>
        /// 服务标签（版本）
        /// </summary>
        public string[] ServiceTags { get; set; }

        /// <summary>
        /// 服务地址（可以选填===默认加载启动地址）
        /// </summary>
        public string ServiceAddress { get; set; }

        /// <summary>
        /// 服务端口号（可以选填===默认加载启动路径端口）
        /// </summary>
        public int ServicePort { get; set; }

        /// <summary>
        /// 健康检查超时时间
        /// </summary>
        public int HealthCheckTimeOut { get; set; }

        /// <summary>
        /// 健康检查间隔时长
        /// </summary>
        public int HealthCheckInterval { get; set; }

        /// <summary>
        /// 服务健康检查路由
        /// </summary>
        public string HealthCheckAddress { get; set; }

        /// <summary>
        /// Consul地址
        /// </summary>
        public string ConsulAddress { get; set; }
    }
}
