using Volo.Abp.AutoMapper;
using Volo.Abp.Http.Client.IdentityModel;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;

namespace TestService
{
    [DependsOn(
        typeof(TestServiceDomainModule),
        //typeof(AbpAccountApplicationModule),
        typeof(TestServiceApplicationContractsModule),
        typeof(AbpHttpClientIdentityModelModule),
        typeof(AbpIdentityHttpApiClientModule),
        typeof(AbpAutoMapperModule)
        //typeof(AbpIdentityApplicationModule),
        //typeof(AbpPermissionManagementApplicationModule),
        //typeof(AbpTenantManagementApplicationModule),
        //typeof(AbpFeatureManagementApplicationModule)
        )]
    public class TestServiceApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<TestServiceApplicationModule>();
                //options.AddProfile<TestServiceApplicationAutoMapperProfile>(validate: true);
            });
        }
    }
}
