using Microsoft.AspNetCore.Mvc;

namespace BookApp.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
