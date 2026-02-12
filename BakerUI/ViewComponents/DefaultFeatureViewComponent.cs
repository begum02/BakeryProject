using BakerUI.Dto.FeatureDto;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BakerUI.ViewComponents
{
    public class DefaultFeatureViewComponent: ViewComponent
    {

        private readonly IHttpClientFactory _httpClientFactory;

        public DefaultFeatureViewComponent(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync("https://localhost:7136/api/Feature");

            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<ResultFeatureDto>>(jsonData);
                return View(values);
            }
            return View();
        }
    }
}
