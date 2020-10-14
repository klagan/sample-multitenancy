namespace Sample.Web.Client.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using Microsoft.Identity.Web;
    using MyAuthentication;
    using Services;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Models;

    // authorisation is controlled in the Startup.cs [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ITokenAcquisition _tokenRepo;
        private readonly IMyContextAccessor _myAccessor;
        private readonly IWebApiRepository _webApiRepository;

        public HomeController(
            ITokenAcquisition tokenRepository,
            IWebApiRepository webApiRepository,
            IMyContextAccessor myAccessor,
            ILogger<HomeController> logger
        )
        {
            _tokenRepo = tokenRepository;
            _webApiRepository = webApiRepository;
            _myAccessor = myAccessor;
            _logger = logger;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Unauthorised()
        {
            return View();
        }

        public async Task<IActionResult> CallWebApi()
        {
            // TODO: ensure configuration is populated and check for single instances of options with tenant id etc.
            var webApiOptions = _webApiRepository.GetBy("WebApi1"); // TODO:: could inject this as a dependency

            // TODO:: remove - purely test to see if we get stuff
            var myTenantDetails = _myAccessor.Tenant;

            // TODO: this fails between restarts because it needs a cache of tokens used.  current cache is in memory and cleared on restart
            // get an OBO token for calling user to call webapi1
            var accessToken = await _tokenRepo.GetAccessTokenForUserAsync(new[] {webApiOptions.PermissionScope});
            
            
            // TODO:: replace this crappy test code in favour of dedicated transport agent (httpclientfactory, refit etc.)
            var a = new HttpClient();
            a.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            a.BaseAddress = new Uri(_myAccessor.Tenant.BaseAddress);

            var response = await a.GetAsync("/Weatherforecast");

            var weatherForecast = await response.Content.ConvertAsync<List<WeatherForecast>>();

            ViewBag.Payload = new SampleData
            {
                AccessToken = accessToken, WeatherForecast = weatherForecast, StatusCode = response.StatusCode,
                Tenant = myTenantDetails
            };

            return View();
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            // // TODO:: look into more consistent solution than this dogs dinner
            // // this will let you know if the reason it errored is because of an invalid tenant
            // // another way to handle this is allow all through but protect the endpoints with action filters
            // var exceptionHandlerPathFeature =
            //     HttpContext.Features.List<IExceptionHandlerPathFeature>();
            //
            // var errorReason = exceptionHandlerPathFeature?.Error.InnerException is SecurityTokenInvalidIssuerException;

            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}
