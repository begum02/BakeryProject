using BakerUI.Dto.TestimonialDto;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BakerUI.ViewComponents
{
    public class AdminTestimonialListViewComponent : ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AdminTestimonialListViewComponent(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IViewComponentResult> InvokeAsync(bool onlyActive = false)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync("https://localhost:7136/api/Testimonial");

            if (!response.IsSuccessStatusCode)
                return View(new List<ResultTestimonialDto>());

            var jsonData = await response.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<ResultTestimonialDto>>(jsonData)
                         ?? new List<ResultTestimonialDto>();

            if (onlyActive)
                values = values.Where(x => x.IsActive).ToList();

            // İstersen burada sıralama da yaparsın (ID büyükten küçüğe)
            values = values.OrderByDescending(x => x.TestimonialId).ToList();

            return View(values);
        }
    }
}
