using Volo.Abp.AuditLogging;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.Identity;
using Volo.Abp.TenantManagement;

namespace BaseService
{
    [DependsOn(
        typeof(BaseServiceDomainSharedModule),
        typeof(AbpAuditLoggingDomainModule),
        //typeof(AbpIdentityDomainModule),
        //typeof(AbpIdentityServerDomainModule),
        typeof(AbpPermissionManagementDomainIdentityModule)
        //typeof(AbpPermissionManagementDomainIdentityServerModule),
        //typeof(AbpTenantManagementDomainModule)
    )]
    public class BaseServiceDomainModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {

        }
    }
}
