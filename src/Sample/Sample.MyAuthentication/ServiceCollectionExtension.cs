namespace Sample.MyAuthentication
{
    using System;
    using System.Linq;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.OpenIdConnect;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc.Authorization;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Microsoft.Identity.Web;
    using Microsoft.Identity.Web.TokenCacheProviders.InMemory;
    using Microsoft.Identity.Web.UI;
    using Microsoft.IdentityModel.Tokens;

    public static class ServiceCollectionExtension
    {
        private static ITenantDataSource TenantDataSource { get; set; }

        /// <summary>
        /// Custom authentication setup using MSAL
        /// </summary>
        /// <param name="services">Service collection</param>
        /// <param name="configuration">Configuration</param>
        /// <param name="tenantDataSource">Agent to return list of valid tenant ids.  If the tenant id list returned is empty then any tenant will be allowed</param>
        /// <param name="unauthorisedPath">Action path to redirect when an invalid tenant is presented.  If you do not pass a path then the system will force a logout</param>
        /// <returns></returns>
        public static IServiceCollection AddMsalAuthentication(
            this IServiceCollection services,
            IConfiguration configuration,
            ITenantDataSource tenantDataSource,
            string unauthorisedPath = ""
        )
        {
            TenantDataSource = tenantDataSource ?? throw new ArgumentNullException(nameof(tenantDataSource));
            
            services.AddHttpContextAccessor();
            services.TryAddTransient<IMyContextAccessor, MyContextAccessor>();
            
            services
                .AddSignIn(configuration)
                // .AddSignIn(openIdConnectOptions =>
                // {
                //     configuration.Bind("AzureAd", openIdConnectOptions);
                // }, microsoftIdentityOptions =>
                // {
                //     configuration.Bind("AzureAd", microsoftIdentityOptions);
                // })
                .AddWebAppCallsProtectedWebApi(configuration)
                .AddInMemoryTokenCaches();
            
            // Restrict users to specific belonging to specific tenants
            services.Configure<OpenIdConnectOptions>(OpenIdConnectDefaults.AuthenticationScheme, options =>
            {
                options.TokenValidationParameters.ValidateAudience = TenantDataSource.GetValidTenants().Any();
                options.TokenValidationParameters.IssuerValidator = ValidateIssuers;
                    
                options.Events.OnAuthenticationFailed = async context =>
                {
                    // if path set for unauthorised calls the redirect there 
                    if (!string.IsNullOrEmpty(unauthorisedPath))
                    {
                        context.Response.Redirect("Home/Unauthorised");
                    }

                    // suppress the exception
                    context.HandleResponse(); 
                    
                    // if no path set for unauthorised calls, force an automatic sign-out
                    if (string.IsNullOrEmpty(unauthorisedPath))
                    {
                        await context.HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);
                    }

                    // return Task.FromResult(0);
                };
            });

            services.AddControllersWithViews(options =>
                {
                    var policy = new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .Build();
                    options.Filters.Add(new AuthorizeFilter(policy));
                })
                .AddMicrosoftIdentityUI();

            return services;
        }
        private static string ValidateIssuers(string issuer, SecurityToken securityToken,
            TokenValidationParameters validationParameters)
        {
            // TODO: have changed accessTokenAcceptedVersion in AAD manifest - need to check any impact
            // https://github.com/AzureAD/microsoft-authentication-library-for-js/issues/560
            
            var validIssuers = TenantDataSource
                .GetValidTenants()
                .Select(tid => $"https://login.microsoftonline.com/{tid}/v2.0"); // v2
              //.Select(tid => $"https://sts.windows.net/{tid}/"); // v1
            
            if (validIssuers.Contains(issuer))
            {
                return issuer;
            }
            
            throw new SecurityTokenInvalidIssuerException(
                "The sign-in user's account does not belong to one of the tenants that this Web App accepts users from.")
            {
                InvalidIssuer = issuer
            };
        }
    }
}