using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace BaseService
{
    [DependsOn(
        typeof(BaseServiceApplicationContractsModule),
        typeof(AbpHttpClientModule))]
    public class BaseServiceHttpApiClientModule : AbpModule
    {
        public const string RemoteServiceName = "BaseService";

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(
                typeof(BaseServiceApplicationContractsModule).Assembly,
                RemoteServiceName
            );
        }
    }
}
