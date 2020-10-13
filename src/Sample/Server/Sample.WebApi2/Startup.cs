namespace Sample.WebApi1
{
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.IdentityModel.Tokens;
    using System.Linq;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.AzureAD.UI;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    public class Startup
    {
        public Startup(
            IConfiguration configuration
        )
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        
        private string[] AcceptedIssusers = new[] {"82d75a56-f939-4164-b05a-2a3c5328b458", "100d1e66-3613-4505-91d6-b6c20c6370f9"};

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(
            IServiceCollection services
        )
        {
            // we authenticate with bearer tokens (AAD Bearer)
            // the details of our service are in the configuration (clientid/tenantid)
            services
                .AddAuthentication(AzureADDefaults.BearerAuthenticationScheme)
                .AddAzureADBearer(options => Configuration.Bind("AzureAd", options));
            
            // configure the bearer authentication handler
            services.Configure<JwtBearerOptions>(AzureADDefaults.JwtBearerAuthenticationScheme, options =>
            {
                // validateIssuer to false means ignore who issued you the token
                // useful for global accepted multi tenancy
                // but for more security we can provide a whitelist of issuers we could accept requests from instead
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    IssuerValidator = (
                        issuer,
                        token,
                        parameters
                    ) =>
                    {
                        var validIssuers = AcceptedIssusers
                            .Select(tid => $"https://login.microsoftonline.com/{tid}/v2.0");

                        if (validIssuers.Contains(issuer))
                        {
                            return issuer;
                        }

                        throw new SecurityTokenInvalidIssuerException(
                            "The sign-in user's account does not belong to one of the tenants that this Web Api1 accepts users from.");
                    }
                };

                // // This is a Microsoft identity platform web API.
                // options.Authority += "/v2.0";
                //
                // // The web API accepts as audiences both the Client ID (options.Audience) and api://{ClientID}.
                // options.TokenValidationParameters.ValidAudiences = new []
                // {
                //     options.Audience,
                //     $"api://{options.Audience}"
                // };

                // Instead of using the default validation (validating against a single tenant,
                // as we do in line-of-business apps),
                // we inject our own multitenant validation logic (which even accepts both v1 and v2 tokens).
                // options.TokenValidationParameters.IssuerValidator = AadIssuerValidator.GetIssuerValidator(options.Authority).Validate;;
            });
            
            
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env
        )
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}