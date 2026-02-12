using BakerUI.Dto.ServiceDto;
using Microsoft.AspNetCore.Mvc;

namespace BakerUI.ViewComponents
{
    public class AdminServiceFormViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View(new CreateServiceDto());
        }
    }
}
