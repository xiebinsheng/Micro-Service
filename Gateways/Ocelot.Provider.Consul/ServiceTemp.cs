using Ocelot.Values;
using System.Collections.Generic;

namespace Ocelot.Provider.Consul
{
    /// <summary>
    /// Ocelot.Values.Service 转为Json的中间对象
    /// </summary>
    public class ServiceTemp
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Version { get; set; }

        public IEnumerable<string> Tags { get; set; }

        public string DownstreamHost { get; set; }

        public int DownstreamPort { get; set; }

        public string Scheme { get; set; }

        public Service ToService()
        {
            ServiceHostAndPort serviceHostAndPort = new ServiceHostAndPort(this.DownstreamHost, this.DownstreamPort, this.Scheme);
            var service = new Service(this.Name, serviceHostAndPort, this.Id, this.Version, this.Tags);
            return service;
        }
    }
}
