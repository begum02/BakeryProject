using BakerUI.Dto.CategoryDto;
using BakerUI.Dto.GalleryDto;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BakerUI.ViewComponents
{
    public class DefaultGalleryViewComponent : ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public DefaultGalleryViewComponent(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var client = _httpClientFactory.CreateClient();

            // Galeri öğelerini çek
            var galleryResponse = await client.GetAsync("https://localhost:7136/api/Gallery");
            var galleryValues = new List<ResultGalleryDto>();

            if (galleryResponse.IsSuccessStatusCode)
            {
                var json = await galleryResponse.Content.ReadAsStringAsync();
                galleryValues = JsonConvert.DeserializeObject<List<ResultGalleryDto>>(json) ?? new();
            }

            // Kategorileri çek
            var categoryResponse = await client.GetAsync("https://localhost:7136/api/Category");
            var categories = new List<ResultCategoryDto>();

            if (categoryResponse.IsSuccessStatusCode)
            {
                var json = await categoryResponse.Content.ReadAsStringAsync();
                categories = JsonConvert.DeserializeObject<List<ResultCategoryDto>>(json) ?? new();
            }

            // Kategorileri ViewBag ile view'a gönder
            ViewBag.Categories = categories;

            return View(galleryValues);
        }
    }
}
