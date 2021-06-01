using MicroService.Shared.ConsulServiceRegistration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using TestService.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.AspNetCore.Auditing;
using Volo.Abp.AspNetCore.ExceptionHandling;
using Volo.Abp.AspNetCore.MultiTenancy;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.AntiForgery;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Auditing;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore.SqlServer;
using Volo.Abp.Identity;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Security.Claims;
using Volo.Abp.Threading;
using Volo.Abp.VirtualFileSystem;

namespace TestService.Host
{
    //[DependsOn(
    //    typeof(AbpAutofacModule),
    //    typeof(TestServiceApplicationModule),
    //    typeof(TestServiceEntityFrameworkCoreModule),
    //    typeof(TestServiceHttpApiModule),
    //    typeof(AbpAspNetCoreMultiTenancyModule),
    //    typeof(AbpPermissionManagementHttpApiModule),
    //    typeof(AbpTenantManagementHttpApiModule),
    //    typeof(AbpIdentityHttpApiModule),
    //    typeof(AbpAspNetCoreSerilogModule)
    //)]
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(AbpAspNetCoreMvcModule),
        //typeof(AbpEventBusRabbitMqModule),
        typeof(AbpEntityFrameworkCoreSqlServerModule),
        typeof(AbpAuditLoggingEntityFrameworkCoreModule),
        //typeof(AbpVirtualFileSystemModule),
        //typeof(AbpPermissionManagementEntityFrameworkCoreModule),
        //typeof(AbpSettingManagementEntityFrameworkCoreModule),
        typeof(TestServiceApplicationModule),
        typeof(TestServiceHttpApiModule),
        typeof(TestServiceEntityFrameworkCoreModule),
        //typeof(AbpIdentityHttpApiModule),
        typeof(AbpAspNetCoreMultiTenancyModule),
        typeof(AbpAspNetCoreSerilogModule)
        //typeof(AbpTenantManagementEntityFrameworkCoreModule)
        )]
    public class TestServiceHttpApiHostModule : AbpModule
    {
        private const string DefaultCorsPolicyName = "Default";

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();
            var hostingEnvironment = context.Services.GetHostingEnvironment();

            var aaa = Convert.ToBoolean(configuration["Auditing:IsEnabledEntityAuditing"]);

            //Configure<MvcOptions>(options =>
            //{
            //    // 获取注入的abp默认的异常拦截器，并移除
            //    var abpExceptionFilter= options.Filters.FirstOrDefault(t => t is ServiceFilterAttribute attribute 
            //                                                             && attribute.ServiceType.Equals(typeof(AbpExceptionFilter)));
            //    options.Filters.Remove(abpExceptionFilter);
            //    // 添加自定义异常拦截器
            //    options.Filters.Add(typeof(CustomExceptionFilter));
            //});

            Configure<AbpExceptionHandlingOptions>(options =>
            {
#if DEBUG
                options.SendExceptionsDetailsToClients = true;
#else
                options.SendExceptionsDetailsToClients = false;
#endif
            });

            Configure<AbpMultiTenancyOptions>(options =>
            {
                options.IsEnabled = false;
            });

            // TODO:暂时禁用防止跨站请求伪造（XSRF/CSRF）攻击，待研究
            Configure<AbpAntiForgeryOptions>(options => { options.AutoValidate = false; });


            // TODO:将动态API注入移动到httpapi层
            Configure<AbpAspNetCoreMvcOptions>(options =>
            {
                options.ConventionalControllers.Create(typeof(TestServiceApplicationModule).Assembly, settings =>
                {
                    /* 
                     * 2021-04-28:add by 26075
                     * 默认路由生成惯例
                     * 1、路由始终以/api开头
                     * 2、接着是路由路径，默认值为/app，如果我们需要修改成我们服务的名称，如下：给RootPath赋值为testService
                     * https://docs.abp.io/zh-Hans/abp/latest/API/Auto-API-Controllers
                     */
                    settings.RootPath = "testService";
                    settings.UseV3UrlStyle = true;
                });
            });

            context.Services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = configuration["AuthServer:Authority"];
                    options.RequireHttpsMetadata = false;
                    options.ApiName = "TestService";
                });

            context.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "TestService API", Version = "v1" });
                options.DocInclusionPredicate((docName, description) => true);
                options.CustomSchemaIds(type => type.FullName);
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "请输入JWT令牌，例如：Bearer 12345abcdef",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                  {
                    {
                      new OpenApiSecurityScheme
                      {
                        Reference = new OpenApiReference
                          {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                          },
                          Scheme = "oauth2",
                          Name = "Bearer",
                          In = ParameterLocation.Header,
                        },
                        new List<string>()
                      }
                    });
            });

            // 注释原因：在依赖的TestServiceEntityFrameworkCoreModule中已经注入
            //Configure<AbpDbContextOptions>(options =>
            //{
            //    options.UseSqlServer();
            //});

            context.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration["Redis:Configuration"];
            });

            context.Services.Configure<ConsulServiceOptions>(configuration.GetSection("Consul"));
            context.Services.AddTransient<ConsulServiceOptions>();
            context.Services.AddTransient<IConsulServiceRegistry, ConsulServiceRegistry>();

            // 审计日志选项
            Configure<AbpAuditingOptions>(options =>
            {
                // 是否开启审计
                options.IsEnabled = Convert.ToBoolean(configuration["Auditing:IsEnabled"]);

                // 是否对GET请求进行审计
                options.IsEnabledForGetRequests = Convert.ToBoolean(configuration["Auditing:IsEnabledForGetRequests"]);

                // 审计日志记录的应用程序/服务名称
                options.ApplicationName = configuration["App:ServiceName"];

                //options.ig.Add();

                // 启用实体审计，ABP vNext默认是关闭的
                //if (Convert.ToBoolean(configuration["Auditing:IsEnabledEntityAuditing"]))
                //{
                //options.EntityHistorySelectors.AddAllEntities();
                //}
            });

            Configure<AbpAspNetCoreAuditingOptions>(options =>
            {
                options.IgnoredUrls.AddIfNotContains("/HealthCheck");
            });

            context.Services.AddCors(options =>
            {
                options.AddPolicy(DefaultCorsPolicyName, builder =>
                {
                    builder
                        .WithOrigins(
                            configuration["App:CorsOrigins"]
                                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                                .Select(o => o.RemovePostFix("/"))
                                .ToArray()
                        )
                        .WithAbpExposedHeaders()
                        .SetIsOriginAllowedToAllowWildcardSubdomains()
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });

            var redis = ConnectionMultiplexer.Connect(configuration["Redis:Configuration"]);
            context.Services.AddDataProtection().PersistKeysToStackExchangeRedis(redis, "Ms-DataProtection-Keys");

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Languages.Add(new LanguageInfo("zh-Hans", "zh-Hans", "简体中文"));
                options.Languages.Add(new LanguageInfo("en", "en", "English"));
            });

            if (hostingEnvironment.IsDevelopment())
            {
                Configure<AbpVirtualFileSystemOptions>(options =>
                {
                    options.FileSets.ReplaceEmbeddedByPhysical<TestServiceDomainSharedModule>(
                        Path.Combine(hostingEnvironment.ContentRootPath,
                            $"..{Path.DirectorySeparatorChar}TestService.Domain.Shared"));
                    options.FileSets.ReplaceEmbeddedByPhysical<TestServiceDomainModule>(
                        Path.Combine(hostingEnvironment.ContentRootPath,
                            $"..{Path.DirectorySeparatorChar}TestService.Domain"));
                    options.FileSets.ReplaceEmbeddedByPhysical<TestServiceApplicationContractsModule>(
                        Path.Combine(hostingEnvironment.ContentRootPath,
                            $"..{Path.DirectorySeparatorChar}TestService.Application.Contracts"));
                    options.FileSets.ReplaceEmbeddedByPhysical<TestServiceApplicationModule>(
                        Path.Combine(hostingEnvironment.ContentRootPath,
                            $"..{Path.DirectorySeparatorChar}TestService.Application"));
                });
            }
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();

            app.UseCorrelationId();
            app.UseVirtualFiles();
            app.UseRouting();
            app.UseCors(DefaultCorsPolicyName);
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMultiTenancy();
            app.UseAbpClaimsMap();

            //app.Use(async (ctx, next) =>
            //{
            //    var currentPrincipalAccessor = ctx.RequestServices.GetRequiredService<ICurrentPrincipalAccessor>();
            //    var map = new Dictionary<string, string>()
            //    {
            //        { "sub", AbpClaimTypes.UserId },
            //        { "role", AbpClaimTypes.Role },
            //        { "email", AbpClaimTypes.Email },
            //        { "name", AbpClaimTypes.UserName },
            //    };
            //    var mapClaims = currentPrincipalAccessor.Principal.Claims.Where(p => map.Keys.Contains(p.Type)).ToList();
            //    currentPrincipalAccessor.Principal.AddIdentity(new ClaimsIdentity(mapClaims.Select(p => new Claim(map[p.Type], p.Value, p.ValueType, p.Issuer))));
            //    await next();
            //});

            app.UseAbpRequestLocalization();
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "TestService API");
            });

            app.UseAuditing();
            app.UseAbpSerilogEnrichers();
            app.UseConfiguredEndpoints();

            AsyncHelper.RunSync(async () =>
            {
                using (var scope = context.ServiceProvider.CreateScope())
                {
                    await scope.ServiceProvider
                        .GetRequiredService<IDataSeeder>()
                        .SeedAsync();
                }
            });
            app.UseConsulRegistry();
        }
    }
}