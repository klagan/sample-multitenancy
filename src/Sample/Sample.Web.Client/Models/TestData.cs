namespace Sample.Web.Client.Models
{
    using System.Collections.Generic;

    public class TestData
    {
        public string AccessToken { get; set; }
        
        public IList<WeatherForecast> WeatherForecast { get; set; }
    }
}