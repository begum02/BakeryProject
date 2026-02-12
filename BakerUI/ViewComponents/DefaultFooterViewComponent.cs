using Microsoft.AspNetCore.Mvc;

namespace BakerUI.ViewComponents
{
    public class DefaultFooterViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            // Şimdilik statik.
            // İstersen buraya API bağlantısı ekleyebiliriz.
            return View();
        }
    }
}