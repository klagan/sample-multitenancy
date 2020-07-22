namespace Sample.Web.Client.Services
{
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.AzureAD.UI;
    using Microsoft.AspNetCore.Authentication.OpenIdConnect;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
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
        /// <summary>
        /// Standard authentication set up out of the box when creating a web application.  This is used as a control against the custom extensions we are writing
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddVanillaAuthentication(
            this IServiceCollection services, IConfiguration configuration
        )
        {
            // we authenticate with AAD
            // get our client details from configuration
            services
                .AddAuthentication(AzureADDefaults.AuthenticationScheme)
                .AddAzureAD(options => configuration.Bind("AzureAd", options));

            // configure the open id connection handler including events
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

        /// <summary>
        /// Custom authentication setup using MSAL
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
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
            
            // Restrict users to specific belonging to specific tenants
            services.Configure<OpenIdConnectOptions>(OpenIdConnectDefaults.AuthenticationScheme, options =>
            {
                options.TokenValidationParameters.ValidateAudience = true;
                options.TokenValidationParameters.IssuerValidator = ValidateSpecificIssuers;
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
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="issuer"></param>
        /// <param name="securityToken"></param>
        /// <param name="validationParameters"></param>
        /// <returns></returns>
        /// <exception cref="SecurityTokenInvalidIssuerException"></exception>
        private static string ValidateSpecificIssuers(string issuer, SecurityToken securityToken,
            TokenValidationParameters validationParameters)
        {
            var validIssuers = GetAcceptedTenantIds()
                .Select(tid => $"https://login.microsoftonline.com/{tid}/v2.0")
                ;//.Select(tid => $"https://sts.windows.net/{tid}/");
            
            if (validIssuers.Contains(issuer))
            {
                return issuer;
            }
            else
            {
                throw new SecurityTokenInvalidIssuerException("The sign-in user's account does not belong to one of the tenants that this Web App accepts users from.");
            }
        }
        
        /// <summary>
        /// List of tenants we will allow to use the web application. (Should probably be a static list from a repository somewhere.)
        /// </summary>
        /// <returns></returns>
        private static string[] GetAcceptedTenantIds()
        {
            // If you are an ISV who wants to make the Web app available only to certain customers who
            // are paying for the service, you might want to fetch this list of accepted tenant ids from
            // a database.
            // Here for simplicity we just return a hard-coded list of TenantIds.
            return new[]
            {
                "82d75a56-f939-4164-b05a-2a3c5328b458",  // laganlabs.it
                "100d1e66-3613-4505-91d6-b6c20c6370f9",  // test24.uk
                "<GUID2>"
            };
        }

        /// <summary>
        /// Load the web api resource configurations for WebApi1
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddWebApi1Options(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            // TODO:: see if this works
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