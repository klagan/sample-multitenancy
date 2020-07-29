﻿using System.Diagnostics;
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
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Authentication.OpenIdConnect;
    using Microsoft.AspNetCore.Diagnostics;
    using Microsoft.Identity.Web;
    using Microsoft.IdentityModel.Tokens;
    using Services;

    // authorisation is controlled in the Startup.cs [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ITokenAcquisition _tokenRepo;
        private readonly WebApiLocator _webApiLocator;

        public HomeController(
            ITokenAcquisition tokenRepository,
            WebApiLocator webApiLocator,
            ILogger<HomeController> logger
        )
        {
            _tokenRepo = tokenRepository;
            _webApiLocator = webApiLocator;
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

        public IActionResult Unauthorised()
        {
            return View();
        }
        
        public async Task<IActionResult> CallWebApi()
        {
            // TODO: ensure configuration is populated and check for single instances of options with tenant id etc. 
            var userTenant = HttpContext.User.GetTenantId();
            var webApiOptions = _webApiLocator.Get(userTenant);
             

            // get an OBO token for calling user to call webapi1
            var accessToken = await _tokenRepo.GetAccessTokenForUserAsync(new[] {$"{webApiOptions.ClientId}/.default"});

            // TODO:: replace this crappy test code in favour of dedicated transport agent (httpclientfactory, refit etc.)
            var a = new HttpClient();
            a.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            a.BaseAddress = new Uri(webApiOptions.BaseAddress);
            
            var response = await a.GetAsync("/Weatherforecast");

            var weatherForecast = await response.Content.ConvertAsync<List<WeatherForecast>>();

            var k = await response.Content.ReadAsStringAsync();
            
            ViewBag.Payload = new TestData {AccessToken = accessToken, WeatherForecast = weatherForecast};

            return View();
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            // foreach (var cookie in Request.Cookies.Keys)
            // {
            //     Response.Cookies.Delete(cookie);
            // }
            var exceptionHandlerPathFeature =
                HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            // TODO:: look into more consistent solution than this dogs dinner
            // this will let you know if the reason it errored is because of an invalid tenant
            // another way to handle this is allow all through but protect the endpoints with action filters
            var a = exceptionHandlerPathFeature?.Error.InnerException is SecurityTokenInvalidIssuerException;
            
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}