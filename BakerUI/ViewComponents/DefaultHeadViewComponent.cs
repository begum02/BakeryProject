using Microsoft.AspNetCore.Mvc;

namespace BakerUI.ViewComponents
{
    public class DefaultHeadViewComponent: ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
