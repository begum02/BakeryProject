using BakerUI.Dto.TestimonialDto;
using Microsoft.AspNetCore.Mvc;

namespace BakerUI.ViewComponents
{
    public class AdminTestimonialFormViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(CreateTestimonialDto model)
        {
            return View(model);
        }
    }
}
