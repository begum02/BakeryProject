using BakerUI.Dto.ServiceDto;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BakerUI.ViewComponents
{
    public class AdminServiceListViewComponent : ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AdminServiceListViewComponent(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync("https://localhost:7136/api/Service");

            ResultServiceDto? value = null;

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var list = JsonConvert.DeserializeObject<List<ResultServiceDto>>(json);
                value = list?.FirstOrDefault();   // 👈 SADECE 1 KAYIT
            }

            return View(value);
        }
    }
}
