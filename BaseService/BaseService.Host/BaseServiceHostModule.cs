using BaseService.EntityFrameworkCore;
using MicroService.Shared.ConsulServiceRegistration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp;
using Volo.Abp.AspNetCore.MultiTenancy;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Auditing;
using Volo.Abp.Autofac;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.PermissionManagement.HttpApi;
using Volo.Abp.TenantManagement;

namespace BaseService
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(BaseServiceApplicationModule),
        typeof(BaseServiceEntityFrameworkCoreModule),
        typeof(BaseServiceHttpApiModule),
        typeof(AbpAspNetCoreMultiTenancyModule),
        typeof(AbpPermissionManagementHttpApiModule),
        typeof(AbpTenantManagementHttpApiModule),
        typeof(AbpIdentityHttpApiModule),
        typeof(AbpAspNetCoreSerilogModule)
    )]
    public class BaseServiceHostModule : AbpModule
    {
        private const string DefaultCorsPolicyName = "Default";

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();

            Configure<AbpMultiTenancyOptions>(options =>
            {
                // 默认不启用多租户
                options.IsEnabled = false;
            });

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

            context.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = configuration["AuthenticationServer:Authority"];
                    options.RequireHttpsMetadata = false;
                    options.Audience = configuration["AuthenticationServer:ApiName"];
                });

            context.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "BaseService Service API",
                    Version = "v1",
                    Description="基础服务，用于管理平台公共业务"
                });
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

            Configure<AbpDbContextOptions>(options =>
            {
                options.UseSqlServer();
            });

            context.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration["Redis:Configuration"];
            });

            context.Services.Configure<ConsulServiceOptions>(configuration.GetSection("Consul"));
            context.Services.AddTransient<ConsulServiceOptions>();
            context.Services.AddTransient<IConsulServiceRegistry, ConsulServiceRegistry>();

            Configure<AbpAuditingOptions>(options =>
            {
                options.IsEnabledForGetRequests = true;
                options.ApplicationName = "BaseService";
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
            context.Services.AddDataProtection()
                .PersistKeysToStackExchangeRedis(redis, "DataProtection-Keys");

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Languages.Add(new LanguageInfo("en", "en", "English"));
                options.Languages.Add(new LanguageInfo("zh-Hans", "zh-Hans", "简体中文"));
            });
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
            //app.UseMultiTenancy();


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
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "BaseService Service API");
                options.RoutePrefix=string.Empty;
            });

            app.UseAuditing();
            app.UseAbpSerilogEnrichers();

            app.UseConfiguredEndpoints();

            //AsyncHelper.RunSync(async () =>
            //{
            //    using (var scope = context.ServiceProvider.CreateScope())
            //    {
            //        await scope.ServiceProvider
            //            .GetRequiredService<IDataSeeder>()
            //            .SeedAsync();
            //    }
            //});

            app.UseConsulRegistry();
        }
    }
}
