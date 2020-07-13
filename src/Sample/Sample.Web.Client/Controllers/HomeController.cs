using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sample.Web.Client.Models;

namespace Sample.Web.Client.Controllers
{
    using Microsoft.AspNetCore.Mvc.Razor.Compilation;
    using Microsoft.Identity.Web;

    //[Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ITokenAcquisition _tokenRepo;

        public HomeController(
            ITokenAcquisition tokenRepository,
            ILogger<HomeController> logger
        )
        {
            _tokenRepo = tokenRepository;
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
            var accessToken =
                await _tokenRepo.GetAccessTokenForUserAsync(new[] {"User.Read"});

            ViewBag.Payload = new TestData {AccessToken = accessToken};
            
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