using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web.Resource;

namespace Sample.WebApi1.Controllers
{
    using Microsoft.Extensions.Configuration;

    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IConfiguration _configuration;

        public WeatherForecastController(
            IConfiguration configuration,
            ILogger<WeatherForecastController> logger
        )
        {
            _configuration = configuration;
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            HttpContext.VerifyUserHasAnyAcceptedScope("user_impersonation");
            var rng = new Random();
            
            // FunctionThatDoesNothing(); if enabled, should fail for the caller as it demands a scope that doesnt exist
            
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = rng.Next(-20, 55),
                    Summary = Summaries[rng.Next(Summaries.Length)]
                })
                .ToArray();
        }

        private void FunctionThatDoesNothing()
        {
            // just testing scope demands
            HttpContext.VerifyUserHasAnyAcceptedScope("scope_that_doesnt_exist");
        }

        [AllowAnonymous]
        [HttpGet("Kam")]
        public string Kam()
        {
            return $"{_configuration["AzureAd:ClientId"]} / {Environment.GetEnvironmentVariable("ASPNETCORE_AzureAd__ClientId")}";
        }
    }
}
