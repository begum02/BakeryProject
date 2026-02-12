using BakerUI.Dto.TestimonialDto;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BakerUI.ViewComponents
{
    public class DefaultClientViewComponent : ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public DefaultClientViewComponent(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var client = _httpClientFactory.CreateClient();

            // API endpointini senin route'una göre düzenle
            var response = await client.GetAsync("https://localhost:7136/api/Testimonial");

            if (!response.IsSuccessStatusCode)
                return View(new List<ResultTestimonialDto>());

            var json = await response.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<ResultTestimonialDto>>(json)
                         ?? new List<ResultTestimonialDto>();

            // Sadece aktifleri gönder
            var activeValues = values.ToList();

            return View(activeValues);
        }
    }
}
