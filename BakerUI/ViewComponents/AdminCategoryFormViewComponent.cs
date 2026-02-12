using BakerUI.Dto.CategoryDto;
using Microsoft.AspNetCore.Mvc;

namespace BakerUI.ViewComponents
{
    public class AdminCategoryFormViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View(new CreateCategoryDto());
        }
    }
}
