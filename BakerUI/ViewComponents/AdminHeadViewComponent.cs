using Microsoft.AspNetCore.Mvc;

namespace BakerUI.ViewComponents
{
    public class AdminHeadViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
