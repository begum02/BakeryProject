using BakerUI.Dto.ServiceDto;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BakerUI.ViewComponents
{
    public class DefaultServiceViewComponent : ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public DefaultServiceViewComponent(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var client = _httpClientFactory.CreateClient();

            // ⭐ yeni endpoint
            var responseMessage = await client.GetAsync("https://localhost:7136/api/Service/index");

            if (!responseMessage.IsSuccessStatusCode)
                return View(new ResultServiceDto());

            var jsonData = await responseMessage.Content.ReadAsStringAsync();

            // ⭐ artık liste değil TEK model
            var value = JsonConvert.DeserializeObject<ResultServiceDto>(jsonData)
                        ?? new ResultServiceDto();

            return View(value);
        }
    }
}
