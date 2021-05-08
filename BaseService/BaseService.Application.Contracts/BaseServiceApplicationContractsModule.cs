using Volo.Abp.Account;
using Volo.Abp.Application;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.ObjectExtending;
using Volo.Abp.PermissionManagement;

namespace BaseService
{
    [DependsOn(
        typeof(BaseServiceDomainSharedModule),
        //typeof(AbpDddApplicationModule),
        //typeof(AbpAccountApplicationContractsModule),
        //typeof(AbpIdentityApplicationContractsModule),
        typeof(AbpDddApplicationModule),
        typeof(AbpPermissionManagementApplicationContractsModule),
        typeof(AbpObjectExtendingModule)
    )]
    public class BaseServiceApplicationContractsModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            BaseServiceDtoExtensions.Configure();
        }
    }
}
