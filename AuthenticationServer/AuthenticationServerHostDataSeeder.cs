using IdentityServer4.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Identity;
using Volo.Abp.IdentityServer.ApiResources;
using Volo.Abp.IdentityServer.ApiScopes;
using Volo.Abp.IdentityServer.Clients;
using Volo.Abp.IdentityServer.IdentityResources;
using Volo.Abp.PermissionManagement;
using Volo.Abp.Uow;
using ApiResource = Volo.Abp.IdentityServer.ApiResources.ApiResource;
using ApiScope = Volo.Abp.IdentityServer.ApiScopes.ApiScope;
using Client = Volo.Abp.IdentityServer.Clients.Client;

namespace AuthenticationServer.Host
{
    public class AuthenticationServerHostDataSeeder : IDataSeedContributor, ITransientDependency
    {
        private readonly IClientRepository _clientRepository;
        private readonly IApiScopeRepository _apiScopeRepository;
        private readonly IApiResourceRepository _apiResourceRepository;
        private readonly IIdentityResourceDataSeeder _identityResourceDataSeeder;
        private readonly IGuidGenerator _guidGenerator;
        private readonly IPermissionDataSeeder _permissionDataSeeder;

        public AuthenticationServerHostDataSeeder(
            IClientRepository clientRepository,
            IApiScopeRepository apiScopeRepository,
            IApiResourceRepository apiResourceRepository,
            IIdentityResourceDataSeeder identityResourceDataSeeder,
            IGuidGenerator guidGenerator,
            IPermissionDataSeeder permissionDataSeeder)
        {
            _clientRepository = clientRepository;
            _apiScopeRepository = apiScopeRepository;
            _apiResourceRepository = apiResourceRepository;
            _identityResourceDataSeeder = identityResourceDataSeeder;
            _guidGenerator = guidGenerator;
            _permissionDataSeeder = permissionDataSeeder;
        }

        [UnitOfWork]
        public virtual async Task SeedAsync(DataSeedContext context)
        {
            await _identityResourceDataSeeder.CreateStandardResourcesAsync();
            await CreateApiResourcesAsync();
            await CreateApiScopesAsync();
            await CreateClientsAsync();
        }

        #region Api Resources
        /*
         * 资源是您想要使用IdentityServer保护的资源 ， 您的用户的身份数据或API。
         * 每个资源都有一个唯一的名称 ，客户端使用这个名称来指定他们想要访问的资源。
         * 用户身份数据标识信息，比如姓名或邮件地址等。
         * API资源，表示客户端想要调用的功能 ，通常被建模为Web API，但不一定。
         */

        private async Task CreateApiResourcesAsync()
        {
            //通用的用户身份信息
            var commonApiUserClaims = new[]
            {
                "email",
                //"email_verified",
                "name",
                "phone_number",
                //"phone_number_verified",
                "role"
            };

            //基础服务
            await CreateApiResourceAsync("BaseService", commonApiUserClaims);
            //内部网关
            await CreateApiResourceAsync("InternalGateway", commonApiUserClaims);
            //外部公共网关
            await CreateApiResourceAsync("PublicWebSiteGateway", commonApiUserClaims);
            //测试服务
            await CreateApiResourceAsync("TestService", commonApiUserClaims);
        }

        private async Task<ApiResource> CreateApiResourceAsync(string name, IEnumerable<string> claims)
        {
            var apiResource = await _apiResourceRepository.FindByNameAsync(name);
            if (apiResource == null)
            {
                apiResource = await _apiResourceRepository.InsertAsync(
                    new ApiResource(
                        _guidGenerator.Create(),
                        name,
                        name + " API"
                    ),
                    autoSave: true
                );
            }

            foreach (var claim in claims)
            {
                if (apiResource.FindClaim(claim) == null)
                {
                    apiResource.AddUserClaim(claim);
                }
            }

            return await _apiResourceRepository.UpdateAsync(apiResource);
        }

        #endregion

        #region Api Scopes

        private async Task CreateApiScopesAsync()
        {
            await CreateApiScopeAsync("BaseService");
            await CreateApiScopeAsync("InternalGateway");
            await CreateApiScopeAsync("PublicWebSiteGateway");
            await CreateApiScopeAsync("TestService");
        }

        private async Task<ApiScope> CreateApiScopeAsync(string name)
        {
            var apiScope = await _apiScopeRepository.GetByNameAsync(name);
            if (apiScope == null)
            {
                apiScope = await _apiScopeRepository.InsertAsync(
                    new ApiScope(
                        _guidGenerator.Create(),
                        name,
                        name + " API"
                    ),
                    autoSave: true
                );
            }
            return apiScope;
        }

        #endregion

        #region Client
        /*
         * 客户端是从IdentityServer请求令牌的软件，用于验证用户（请求身份令牌）或访问资源（请求访问令牌）。 
         * 必须首先向IdentityServer注册客户端才能请求令牌。
         * 客户端可以是Web应用程序，本地移动或桌面应用程序，SPA，服务器进程等。
         */

        private async Task CreateClientsAsync()
        {
            //const string commonSecret = "E5Xd4yMqjP5kjWFKrYgySBju6JVfCzMyFp7n2QmMrME=";

            var commonScopes = new[]
            {
                "email",
                "openid",
                "profile",
                "role",
                "phone",
                "address"
            };

            await CreateClientAsync(
                "public-website-client",    //一个唯一的客户端ID
                new[] { "PublicWebSiteGateway", "BaseService", "TestService" },     //允许客户端访问的scope列表（API资源）
                new[] { "password" },     //允许与令牌服务的交互（称为授权类型）,这里我们用混合模式
                "1q2w3e*".Sha256(),     //一个密钥（非必须）
                permissions: new[] { IdentityPermissions.Users.Default, IdentityPermissions.UserLookup.Default }   //权限
                //redirectUri: "https://localhost:50002/signin-oidc",  //身份或访问令牌被发送到的url（称为重定向URI）
                //postLogoutRedirectUri: "https://localhost:50002/signout-callback-oidc"
            );

            await CreateClientAsync(
                "business-app",
                new[] { "InternalGateway", "BaseService" },
                new[] { "client_credentials" },
                "1q2w3e*".Sha256(),
                permissions: new[] { IdentityPermissions.Users.Default, IdentityPermissions.UserLookup.Default }
                //redirectUri: "https://localhost:50002/signin-oidc",
                //postLogoutRedirectUri: "https://localhost:50002/signout-callback-oidc"
            );
        }

        private async Task<Client> CreateClientAsync(
            string name,
            IEnumerable<string> scopes,
            IEnumerable<string> grantTypes,
            string secret,
            string redirectUri = null,
            string postLogoutRedirectUri = null,
            IEnumerable<string> permissions = null)
        {
            var client = await _clientRepository.FindByClientIdAsync(name);
            if (client == null)
            {
                client = await _clientRepository.InsertAsync(
                    new Client(
                        _guidGenerator.Create(),
                        name
                    )
                    {
                        ClientName = name,
                        ProtocolType = "oidc",
                        Description = name,
                        AlwaysIncludeUserClaimsInIdToken = true,
                        AllowOfflineAccess = true,
                        AbsoluteRefreshTokenLifetime = 31536000, //365 days
                        AccessTokenLifetime = 31536000, //365 days
                        AuthorizationCodeLifetime = 300,
                        IdentityTokenLifetime = 300,
                        RequireConsent = false
                    },
                    autoSave: true
                );
            }

            foreach (var scope in scopes)
            {
                if (client.FindScope(scope) == null)
                {
                    client.AddScope(scope);
                }
            }

            foreach (var grantType in grantTypes)
            {
                if (client.FindGrantType(grantType) == null)
                {
                    client.AddGrantType(grantType);
                }
            }

            if (client.FindSecret(secret) == null)
            {
                client.AddSecret(secret);
            }

            if (redirectUri != null)
            {
                if (client.FindRedirectUri(redirectUri) == null)
                {
                    client.AddRedirectUri(redirectUri);
                }
            }

            if (postLogoutRedirectUri != null)
            {
                if (client.FindPostLogoutRedirectUri(postLogoutRedirectUri) == null)
                {
                    client.AddPostLogoutRedirectUri(postLogoutRedirectUri);
                }
            }

            if (permissions != null)
            {
                await _permissionDataSeeder.SeedAsync(
                    ClientPermissionValueProvider.ProviderName,
                    name,
                    permissions
                );
            }

            return await _clientRepository.UpdateAsync(client);
        }

        #endregion



    }
}
