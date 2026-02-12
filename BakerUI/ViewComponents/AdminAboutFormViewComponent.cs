using BakerUI.Dto.AboutDto;   // sende dto yolu neyse
using Microsoft.AspNetCore.Mvc;

namespace BakerUI.ViewComponents
{
    public class AdminAboutFormViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View(new CreateAboutDto());
        }
    }
}
