namespace Sample.Web.Client.Services
{
    using System.Linq;
    using Models;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// Representation of any web api configurations
    /// </summary>
    public class InMemoryWebApiRepository : IWebApiRepository
    {
        static WebApiOptions[] _webApiServices;

        public InMemoryWebApiRepository(IConfiguration configuration)
        {
            var apiOptions1 = new WebApiOptions();
            configuration.GetSection("WebApi1").Bind(apiOptions1);
            apiOptions1.Name = "WebApi1";
            
            var apiOptions2 = new WebApiOptions();
            configuration.GetSection("WebApi2").Bind(apiOptions2);
            apiOptions2.Name = "WebApi2";
            
            _webApiServices = new[] {apiOptions1, apiOptions2};
        }
        
        public WebApiOptions GetBy (
            string name
            )
        {
            return _webApiServices.SingleOrDefault(o => o.Name == name);
        }
    }
}
