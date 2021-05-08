using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Modularity;

namespace TestService
{
    [DependsOn(
        typeof(TestServiceApplicationContractsModule),
        typeof(AbpAspNetCoreMvcModule)
        //typeof(AbpAccountHttpApiModule),
        //typeof(AbpIdentityHttpApiModule),
        //typeof(AbpPermissionManagementHttpApiModule),
        //typeof(AbpTenantManagementHttpApiModule),
        //typeof(AbpFeatureManagementHttpApiModule)
        )]
    public class TestServiceHttpApiModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(TestServiceHttpApiModule).Assembly);
            });
        }
        //public override void ConfigureServices(ServiceConfigurationContext context)
        //{
        //    Configure<AbpAspNetCoreMvcOptions>(options =>
        //    {
        //        options.ConventionalControllers.Create(typeof(TestServiceApplicationContractsModule).Assembly);
        //    });
        //}
        //public override void ConfigureServices(ServiceConfigurationContext context)
        //{
        //    ConfigureLocalization();
        //}

        //private void ConfigureLocalization()
        //{
        //    Configure<AbpLocalizationOptions>(options =>
        //    {
        //        options.Resources
        //            .Get<TestServiceResource>()
        //            .AddBaseTypes(
        //                typeof(AbpUiResource)
        //            );
        //    });
        //}
    }
}
