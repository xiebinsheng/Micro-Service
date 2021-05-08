using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;

namespace TestService
{
    [DependsOn(
        typeof(TestServiceDomainSharedModule)
        //typeof(AbpAuditLoggingDomainModule)
        //typeof(AbpBackgroundJobsDomainModule),
        //typeof(AbpFeatureManagementDomainModule),
        //typeof(AbpIdentityDomainModule),
        //typeof(AbpPermissionManagementDomainIdentityModule),
        //typeof(AbpIdentityServerDomainModule),
        //typeof(AbpPermissionManagementDomainIdentityServerModule),
        //typeof(AbpSettingManagementDomainModule),
        //typeof(AbpTenantManagementDomainModule)
        //typeof(AbpEmailingModule) //TODO：暂时去除，目前没有需要用到邮件的场景
    )]
    public class TestServiceDomainModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            // 注释原因，在TestServiceHttpApiHostModule中注入
            //Configure<AbpMultiTenancyOptions>(options =>
            //{
            //    //TODO:考虑使用全局的多租户配置，目前先设置不启用，待研究
            //    //options.IsEnabled = MultiTenancyConsts.IsEnabled;
            //    options.IsEnabled = false;
            //});

            //TODO：暂时去除，目前没有需要用到邮件的场景，后续可以研究一下
            //context.Services.Replace(ServiceDescriptor.Singleton<IEmailSender, NullEmailSender>());
        }
    }
}
