namespace Sample.MyAuthentication
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.AzureAD.UI;
    using Microsoft.AspNetCore.Authentication.OpenIdConnect;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc.Authorization;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Microsoft.Identity.Web;
    using Microsoft.Identity.Web.TokenCacheProviders.InMemory;
    using Microsoft.Identity.Web.UI;

    public static class ServiceCollectionExtension
    {
        /// <summary>
        /// Custom authentication setup using MSAL
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddMultiTenantMsalAuthentication(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            services.AddHttpContextAccessor();
            services.TryAddTransient<IMyContextAccessor, MyContextAccessor>();
            services.TryAddTransient<ITenantRepository, TenantRepository>();
            
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
                var tenantRepository = services
                    .BuildServiceProvider()
                    .GetRequiredService<ITenantRepository>();
                
                options.TokenValidationParameters.ValidateAudience = true;
                options.TokenValidationParameters.IssuerValidator = tenantRepository.ValidateIssuers; // TODO: make dynamic 
                    
                options.Events.OnAuthenticationFailed = context =>
                {
                    context.Response.Redirect("Home/Unauthorised");
                    context.HandleResponse(); // Suppress the exception
                    //await context.HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme); // if you wanted to force an automatic signout
                    return Task.FromResult(0);
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
    }
}