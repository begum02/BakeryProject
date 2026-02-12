using BakerUI.Dto.MessageDto;
using Microsoft.AspNetCore.Mvc;

namespace Baker.WebUI.ViewComponents
{
    public class DefaultMessageViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            // Formun modeli tek olmalı
            return View(new CreateMessageDto());
        }
    }
}
