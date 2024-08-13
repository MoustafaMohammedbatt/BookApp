using Microsoft.AspNetCore.Mvc;

namespace BookApp.Controllers
{
    public class ReceptionistController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
