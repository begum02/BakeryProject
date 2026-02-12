using BakerUI.Dto.MessageDto;   // sende dto yolu buysa
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BakerUI.ViewComponents
{
    public class AdminMessageListViewComponent : ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AdminMessageListViewComponent(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync("https://localhost:7136/api/Message");

            var values = new List<ResultMessageDto>();

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                values = JsonConvert.DeserializeObject<List<ResultMessageDto>>(json) ?? new();
            }

            return View(values);
        }
    }
}
