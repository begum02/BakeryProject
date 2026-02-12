using BakerUI.Dto.AboutDto;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Baker.WebUI.ViewComponents
{
    public class DefaultAboutViewComponent : ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public DefaultAboutViewComponent(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync("https://localhost:7136/api/About");

                if (!response.IsSuccessStatusCode)
                    return View(new ResultAboutDto()); // boş model dön

                var jsonData = await response.Content.ReadAsStringAsync();

                // API liste dönüyor -> ilk kaydı seç
                var list = JsonConvert.DeserializeObject<List<ResultAboutDto>>(jsonData) ?? new List<ResultAboutDto>();

                // Eğer IsActive alanın varsa burada filtreleyebilirsin:
                // var about = list.FirstOrDefault(x => x.IsActive);

                var about = list.FirstOrDefault();

                return View(about ?? new ResultAboutDto());
            }
            catch
            {
                return View(new ResultAboutDto());
            }
        }
    }
}
