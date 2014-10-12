using Microsoft.AspNet.Mvc;

namespace RazorPre
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}