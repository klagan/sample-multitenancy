using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Sample.WebApi1
{
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.IdentityModel.Tokens;

    public class Startup
    {
        public Startup(
            IConfiguration configuration
        )
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

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
                options.TokenValidationParameters = new TokenValidationParameters {ValidateIssuer = false};
               
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