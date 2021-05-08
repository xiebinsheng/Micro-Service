using Volo.Abp.Application;
using Volo.Abp.Modularity;

namespace TestService
{
    [DependsOn(
        typeof(TestServiceDomainSharedModule),
        typeof(AbpDddApplicationModule)
        //typeof(AbpAccountApplicationContractsModule),
        //typeof(AbpFeatureManagementApplicationContractsModule),
        //typeof(AbpIdentityApplicationContractsModule),
        //typeof(AbpPermissionManagementApplicationContractsModule),
        //typeof(AbpTenantManagementApplicationContractsModule),
        //typeof(AbpObjectExtendingModule)
    )]
    public class TestServiceApplicationContractsModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            TestServiceDtoExtensions.Configure();
        }
    }
}
