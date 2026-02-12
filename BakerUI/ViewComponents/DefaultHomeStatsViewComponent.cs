using BakerUI.Dto.HomeStatsDto;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Net.Http;

namespace BakerUI.ViewComponents
{
    public class DefaultHomeStatsViewComponent:ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public DefaultHomeStatsViewComponent(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            int chefCount = 0;
            int productCount = 0;

            var client = _httpClientFactory.CreateClient();

            // CHEF COUNT
            var chefResponse = await client.GetAsync("https://localhost:7136/api/Chef");
            if (chefResponse.IsSuccessStatusCode)
            {
                var chefJson = await chefResponse.Content.ReadAsStringAsync();
                chefCount = JArray.Parse(chefJson).Count;
            }

            // PRODUCT COUNT
            var productResponse = await client.GetAsync("https://localhost:7136/api/Products");
            if (productResponse.IsSuccessStatusCode)
            {
                var productJson = await productResponse.Content.ReadAsStringAsync();
                productCount = JArray.Parse(productJson).Count;
            }

            var model = new HomeStats
            {
                YearsExperience = 50,
                SkilledProfessionals = chefCount,  // ✅ chef sayısı
                TotalProducts = productCount,      // ✅ ürün sayısı
                OrdersEveryday = 9357
            };

            return View(model);
        }
    }
}
