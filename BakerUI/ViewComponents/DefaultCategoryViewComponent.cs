using BakerUI.Dto.CategoryDto;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BakerUI.ViewComponents
{
    public class DefaultCategoryViewComponent : ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public DefaultCategoryViewComponent(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var client = _httpClientFactory.CreateClient();

            // ✅ URL burada direkt yazıldı
            var response = await client.GetAsync("https://localhost:7136/api/Category");

            if (!response.IsSuccessStatusCode)
                return View(new List<ResultCategoryDto>());

            var json = await response.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<ResultCategoryDto>>(json) ?? new();

            // sadece aktif olanlar
            values = values.ToList();

            return View(values);
        }
    }
}
