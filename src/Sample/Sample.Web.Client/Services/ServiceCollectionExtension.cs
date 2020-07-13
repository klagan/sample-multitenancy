namespace Sample.Web.Client.Services
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.AzureAD.UI;
    using Microsoft.AspNetCore.Authentication.OpenIdConnect;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc.Authorization;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Identity.Web;
    using Microsoft.Identity.Web.TokenCacheProviders.InMemory;
    using Microsoft.Identity.Web.UI;
    using Microsoft.IdentityModel.Tokens;
    using Models;

    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddVanillaAuthentication(
            this IServiceCollection services, IConfiguration configuration
        )
        {
            services.AddAuthentication(AzureADDefaults.AuthenticationScheme)
                .AddAzureAD(options => configuration.Bind("AzureAd", options));

            services.Configure<OpenIdConnectOptions>(AzureADDefaults.OpenIdScheme, options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    // Instead of using the default validation (validating against a single issuer value, as we do in
                    // line of business apps), we inject our own multitenant validation logic
                    ValidateIssuer = false,
            
                    // If the app is meant to be accessed by entire organizations, add your issuer validation logic here.
                    //IssuerValidator = (issuer, securityToken, validationParameters) => {
                    //    if (myIssuerValidationLogic(issuer)) return issuer;
                    //}
                };
            
                options.Events = new OpenIdConnectEvents
                {
                    OnTicketReceived = context =>
                    {
                        // If your authentication logic is based on users then add your logic here
                        return Task.CompletedTask;
                    },
                    OnAuthenticationFailed = context =>
                    {
                        context.Response.Redirect("/Error");
                        context.HandleResponse(); // Suppress the exception
                        return Task.CompletedTask;
                    },
                    // If your application needs to authenticate single users, add your user validation below.
                    //OnTokenValidated = context =>
                    //{
                    //    return myUserValidationLogic(context.Ticket.Principal);
                    //}
                };
            });

            services.AddControllersWithViews(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            });

            return services;
        }

        public static IServiceCollection AddMsalAuthentication(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
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

        public static IServiceCollection AddWebApiOptions(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            // services.Configure<WebApi1Options>(webApiOptions =>
            // {
            //     configuration.GetSection("WebApi");
            // });
            
            var options = new WebApi1Options();
            configuration.Bind("WebApi1", options);

            return services.AddSingleton(typeof(WebApi1Options), options);
        }
    }
}