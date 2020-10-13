using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Sample.WebApi1.Controllers
{
    [Route("[controller]")]
    public class ConsentController : Controller
    {
        // GET
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }
    }
}