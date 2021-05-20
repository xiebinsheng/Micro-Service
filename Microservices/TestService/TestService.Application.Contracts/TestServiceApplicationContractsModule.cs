using Volo.Abp.Application;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace TestService
{
    [DependsOn(
        typeof(TestServiceDomainSharedModule),
        typeof(AbpDddApplicationModule),
        //typeof(AbpAccountApplicationContractsModule),
        typeof(AbpFeatureManagementApplicationContractsModule)
        //typeof(AbpIdentityApplicationContractsModule),
        //typeof(AbpPermissionManagementApplicationContractsModule),
        //typeof(AbpTenantManagementApplicationContractsModule),
        //typeof(AbpObjectExtendingModule)
    )]
    public class TestServiceApplicationContractsModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<TestServiceApplicationContractsModule>();
            });
        }

        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            TestServiceDtoExtensions.Configure();
        }
    }
}
