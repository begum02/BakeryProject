using BakerUI.Dto.CategoryDto;
using BakerUI.Dto.ProductDto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace BakerUI.ViewComponents
{
    public class AdminProductFormViewComponent : ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AdminProductFormViewComponent(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync("https://localhost:7136/api/Category");

            var categories = new List<SelectListItem>();

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var categoryList = JsonConvert.DeserializeObject<List<ResultCategoryDto>>(json);

                categories = categoryList?.Select(c => new SelectListItem
                {
                    Value = c.CategoryId.ToString(),
                    Text = c.CategoryName
                }).ToList() ?? new List<SelectListItem>();
            }

            ViewBag.Categories = categories;
            return View(new CreateProductDto());
        }
    }
}
