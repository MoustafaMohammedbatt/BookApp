using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BookApp.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            var statusCodeResult = HttpContext.Response.StatusCode;

            return statusCodeResult switch
            {
                404 => View("NotFound"), 
                _ => View("Error")
            };
        }


        [Route("Error")]
        public IActionResult Error()
        {
            var exception = HttpContext.Features.Get<IExceptionHandlerFeature>();
            if (exception != null)
            {
                Debug.WriteLine(exception.Error);
            }
            return View();
        }
    }
}
