namespace Sample.Web.Client.Models
{
    using System.Collections.Generic;
    using System.Net;

    public class TestData
    {
        public string AccessToken { get; set; }
        
        public IList<WeatherForecast> WeatherForecast { get; set; }
        
        public HttpStatusCode StatusCode { get; set; }
    }
}