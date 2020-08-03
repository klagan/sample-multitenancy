namespace Sample.Web.Client.Services
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Models;

    public static class ServiceCollectionExtension
    {
        /// <summary>
        /// Load the web api resource configurations for WebApi1
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddWebApiOptions(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            // TODO:: see if this works
            // services.Configure<WebApiOptions>(webApiOptions =>
            // {
            //     configuration.GetSection("WebApi");
            // });
            
            var options = new WebApiOptions();
            configuration.Bind("WebApi1", options);
            services.AddSingleton(typeof(WebApiOptions), options);
            
            options = new WebApiOptions();
            configuration.Bind("WebApi2", options);
            
            return services.AddSingleton(typeof(WebApiOptions), options);
        }
    }
}