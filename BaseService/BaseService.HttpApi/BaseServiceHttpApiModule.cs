using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Modularity;

namespace BaseService
{
    [DependsOn(
        typeof(BaseServiceApplicationContractsModule),
        typeof(AbpAspNetCoreMvcModule)
    )]
    public class BaseServiceHttpApiModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            // TODO:将动态API注入移动到httpapi层
            Configure<AbpAspNetCoreMvcOptions>(options =>
            {
                options.ConventionalControllers.Create(typeof(BaseServiceApplicationModule).Assembly, settings =>
                {
                    /* 
                     * 2021-04-28:add by 26075
                     * 默认路由生成惯例
                     * 1、路由始终以/api开头
                     * 2、接着是路由路径，默认值为/app，如果我们需要修改成我们服务的名称，如下：给RootPath赋值为testService
                     * https://docs.abp.io/zh-Hans/abp/latest/API/Auto-API-Controllers
                     */
                    settings.RootPath = "baseService";
                    settings.UseV3UrlStyle = true;
                });
            });
        }

        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(BaseServiceHttpApiModule).Assembly);
            });
        }
    }
}
