using Microsoft.AspNetCore.Mvc;

namespace BookApp.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
