using Microsoft.Extensions.DependencyInjection;
using TestService;
using Volo.Abp.AutoMapper;
using Volo.Abp.Http.Client;
using Volo.Abp.Http.Client.IdentityModel;
using Volo.Abp.Identity;
using Volo.Abp.Json;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.TenantManagement;

namespace BaseService
{
    [DependsOn(
        typeof(BaseServiceApplicationContractsModule),
        typeof(AbpHttpClientIdentityModelModule),
        //typeof(AbpIdentityHttpApiClientModule),
        typeof(AbpHttpClientModule),
        typeof(TestServiceApplicationContractsModule),
        typeof(AbpPermissionManagementApplicationModule),
        typeof(AbpTenantManagementApplicationModule),
        typeof(AbpIdentityApplicationModule),
        typeof(AbpAutoMapperModule)
    )]
    public class BaseServiceApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(
                typeof(TestServiceApplicationContractsModule).Assembly,
                remoteServiceConfigurationName: "testService"
            );

            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<BaseServiceApplicationAutoMapperProfile>();
            });

            
        }

        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<AbpJsonOptions>(option =>
            {
                option.UseHybridSerializer = false;
            });

            //PreConfigure<AbpHttpClientBuilderOptions>(options =>
            //{
            //    options.ProxyClientBuildActions.Add((remoteServiceName, clientBuilder) =>
            //    {
            //        clientBuilder.AddTransientHttpErrorPolicy(
            //            policyBuilder => policyBuilder.WaitAndRetryAsync(3, i => TimeSpan.FromSeconds(Math.Pow(2, i)))
            //        );
            //    });
            //});
        }
    }
}
