using BakerUI.Dto.TestimonialDto;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace BakerUI.Controllers
{
    public class TestimonialController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public TestimonialController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // LIST
        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync("https://localhost:7136/api/Testimonial");

            if (!response.IsSuccessStatusCode)
                return View();

            var jsonData = await response.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<ResultTestimonialDto>>(jsonData);

            return View(values);
        }

        // ADMIN LIST
        [HttpGet]
        public async Task<IActionResult> TestimonialList()
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync("https://localhost:7136/api/Testimonial");

            if (!response.IsSuccessStatusCode)
                return View(new List<ResultTestimonialDto>());

            var jsonData = await response.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<ResultTestimonialDto>>(jsonData);

            return View(values ?? new List<ResultTestimonialDto>());
        }

        // CREATE GET
        [HttpGet]
        public IActionResult CreateTestimonial()
        {
            return View();
        }

        // CREATE POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTestimonial(CreateTestimonialDto model)
        {
            var client = _httpClientFactory.CreateClient();

            var jsonData = JsonConvert.SerializeObject(model);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("https://localhost:7136/api/Testimonial", content);

            if (response.IsSuccessStatusCode)
                return RedirectToAction("TestimonialList");

            return View(model);
        }

        // UPDATE GET
        [HttpGet]
        public async Task<IActionResult> UpdateTestimonial(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync("https://localhost:7136/api/Testimonial/" + id);

            if (!response.IsSuccessStatusCode)
                return RedirectToAction("TestimonialList");

            var jsonData = await response.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<UpdateTestimonialDto>(jsonData);

            return View(values);
        }

        // UPDATE POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateTestimonial(UpdateTestimonialDto model)
        {
            // IsActive'i otomatik true yap
            model.IsActive = true;
            
            var client = _httpClientFactory.CreateClient();

            var jsonData = JsonConvert.SerializeObject(model);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"https://localhost:7136/api/Testimonial/{model.TestimonialId}", content);

            if (response.IsSuccessStatusCode)
                return RedirectToAction("TestimonialList");

            return View(model);
        }

        // DELETE
        public async Task<IActionResult> DeleteTestimonial(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.DeleteAsync("https://localhost:7136/api/Testimonial/" + id);

            if (response.IsSuccessStatusCode)
                return RedirectToAction("TestimonialList");

            return BadRequest();
        }
    }
}
