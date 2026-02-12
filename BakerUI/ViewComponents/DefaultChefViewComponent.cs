using BakerUI.Dto.ChefDto;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BakerUI.ViewComponents
{
    public class DefaultChefViewComponent : ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public DefaultChefViewComponent(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            try
            {
                var client = _httpClientFactory.CreateClient();

                // endpointini kendi port/route'una göre düzenle
                var response = await client.GetAsync("https://localhost:7136/api/Chef");

                if (!response.IsSuccessStatusCode)
                    return View(new List<ResultChefDto>());

                var json = await response.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<ResultChefDto>>(json) ?? new List<ResultChefDto>();

                var activeValues = values.ToList();

                return View(activeValues);
            }
            catch
            {
                return View(new List<ResultChefDto>());
            }
        }
    }
}
