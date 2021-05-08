using TestService.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;

namespace TestService
{
    [DependsOn(
        //typeof(AbpAuditLoggingDomainSharedModule),
        //typeof(AbpFeatureManagementDomainSharedModule),
        //typeof(AbpIdentityDomainSharedModule),
        //typeof(AbpIdentityServerDomainSharedModule),
        //typeof(AbpPermissionManagementDomainSharedModule),
        //typeof(AbpTenantManagementDomainSharedModule),
        typeof(AbpLocalizationModule)
        )]
    public class TestServiceDomainSharedModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            TestServiceGlobalFeatureConfigurator.Configure();
            TestServiceModuleExtensionConfigurator.Configure();
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<TestServiceDomainSharedModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<TestServiceResource>("en")
                    .AddBaseTypes(typeof(AbpValidationResource))
                    .AddVirtualJson("/Localization/TestService");

                options.DefaultResourceType = typeof(TestServiceResource);
            });

            Configure<AbpExceptionLocalizationOptions>(options =>
            {
                options.MapCodeNamespace("TestService", typeof(TestServiceResource));
            });
        }
    }
}
