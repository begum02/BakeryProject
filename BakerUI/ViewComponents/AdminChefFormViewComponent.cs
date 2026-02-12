using BakerUI.Dto.ChefDto;
using Microsoft.AspNetCore.Mvc;

namespace BakerUI.ViewComponents
{
    public class AdminChefFormViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            // Form boş model ile açılsın
            return View(new CreateChefDto());
        }
    }
}
