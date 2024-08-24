using Microsoft.AspNetCore.Mvc;

namespace BookApp.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
