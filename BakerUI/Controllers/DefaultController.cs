using Microsoft.AspNetCore.Mvc;

namespace BakerUI.Controllers
{
    public class DefaultController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
