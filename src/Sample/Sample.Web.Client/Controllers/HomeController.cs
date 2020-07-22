using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sample.Web.Client.Models;

namespace Sample.Web.Client.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using Microsoft.Identity.Web;
    using Services;

    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ITokenAcquisition _tokenRepo;
        private readonly WebApi1Options _webApi1Options;

        public HomeController(
            ITokenAcquisition tokenRepository,
            WebApi1Options webApi1Options,
            ILogger<HomeController> logger
        )
        {
            _tokenRepo = tokenRepository;
            _webApi1Options = webApi1Options;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {        
            return View();
        }

        public async Task<IActionResult> CallWebApi()
        {
            var accessToken = await _tokenRepo.GetAccessTokenForUserAsync(new[] {$"{_webApi1Options.ClientId}/.default"});

            // TODO:: remove this crappy test code in favour of dedicated transport agent (httpclientfactory, refit etc.)
            var a = new HttpClient();
            a.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            a.BaseAddress = new Uri(_webApi1Options.BaseAddress);
            
            var response = await a.GetAsync("/Weatherforecast");

            var weatherForecast = await response.Content.ConvertAsync<List<WeatherForecast>>();
            
            ViewBag.Payload = new TestData {AccessToken = accessToken, WeatherForecast = weatherForecast};

            return View();
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}