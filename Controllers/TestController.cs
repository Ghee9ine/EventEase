using Microsoft.AspNetCore.Mvc;

namespace EventEase.Controllers
{
    public class TestController : Controller
    {
        public IActionResult Index()
        {
            return Content("Test controller works! Routing is fine.");
        }
    }
}
