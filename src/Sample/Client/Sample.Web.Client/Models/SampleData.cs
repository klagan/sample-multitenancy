namespace Sample.Web.Client.Models
{
    using System.Collections.Generic;
    using System.Net;
    using MyAuthentication;

    public class SampleData
    {
        public string AccessToken { get; set; }
        
        public IList<WeatherForecast> WeatherForecast { get; set; }
        
        public HttpStatusCode StatusCode { get; set; }
        
        public Tenant Tenant { get; set; }
    }
}