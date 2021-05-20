using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace TestService
{
    [DependsOn(
        typeof(AbpHttpClientModule),     //用来创建客户端代理
        typeof(TestServiceApplicationContractsModule)  //包含应用服务接口
        //typeof(AbpAccountHttpApiClientModule),
        //typeof(AbpIdentityHttpApiClientModule),
        //typeof(AbpPermissionManagementHttpApiClientModule),
        //typeof(AbpTenantManagementHttpApiClientModule),
        //typeof(AbpFeatureManagementHttpApiClientModule)
    )]
    public class TestServiceHttpApiClientModule : AbpModule
    {
        public const string RemoteServiceName = "TestService";

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(
                typeof(TestServiceApplicationContractsModule).Assembly,
                RemoteServiceName
            );
        }
    }
}
