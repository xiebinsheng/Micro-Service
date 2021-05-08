using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.SqlServer;
using Volo.Abp.Modularity;

namespace TestService.EntityFrameworkCore
{
    [DependsOn(
        typeof(TestServiceDomainModule),
        //typeof(AbpIdentityEntityFrameworkCoreModule),
        //typeof(AbpIdentityServerEntityFrameworkCoreModule),
        typeof(AbpEntityFrameworkCoreModule),
        typeof(AbpEntityFrameworkCoreSqlServerModule)
        //typeof(AbpPermissionManagementEntityFrameworkCoreModule),
        //typeof(AbpSettingManagementEntityFrameworkCoreModule),
        //typeof(AbpBackgroundJobsEntityFrameworkCoreModule),
        //typeof(AbpAuditLoggingEntityFrameworkCoreModule)
        //typeof(AbpTenantManagementEntityFrameworkCoreModule)
        //typeof(AbpFeatureManagementEntityFrameworkCoreModule)
        )]
    public class TestServiceEntityFrameworkCoreModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            TestServiceEfCoreEntityExtensionMappings.Configure();
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpDbContextOptions>(options =>
            {
                /* The main point to change your DBMS.
                 * See also TestServiceMigrationsDbContextFactory for EF Core tooling. */
                options.UseSqlServer();
            });

            context.Services.AddAbpDbContext<TestServiceDbContext>(options =>
            {
                /* Remove "includeAllEntities: true" to create
                 * default repositories only for aggregate roots */
                options.AddDefaultRepositories(includeAllEntities: true);
            });
        }
    }
}
