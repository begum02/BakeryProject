using BakerUI.Dto.ChefDto;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace BakerUI.Controllers
{
    public class ChefController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ChefController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // LIST
        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync("https://localhost:7136/api/Chef");

            if (!response.IsSuccessStatusCode)
                return View();

            var jsonData = await response.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<ResultChefDto>>(jsonData);

            return View(values);
        }

        [HttpGet]
        public async Task<IActionResult> ChefList()
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync("https://localhost:7136/api/Chef");

            if (!response.IsSuccessStatusCode)
                return View(new List<ResultChefDto>());

            var jsonData = await response.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<ResultChefDto>>(jsonData)
                         ?? new List<ResultChefDto>();

            return View(values);
        }

        // CREATE GET
        [HttpGet]
        public IActionResult CreateChef()
        {
            return View();
        }

        // CREATE POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateChef(CreateChefDto model)
        {
            var client = _httpClientFactory.CreateClient();

            var jsonData = JsonConvert.SerializeObject(model);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("https://localhost:7136/api/Chef", content);

            if (response.IsSuccessStatusCode)
                return RedirectToAction("ChefList");

            return View(model);
        }

        // UPDATE GET
        [HttpGet]
        public async Task<IActionResult> UpdateChef(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync("https://localhost:7136/api/Chef/" + id);

            if (!response.IsSuccessStatusCode)
                return RedirectToAction("ChefList");

            var jsonData = await response.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<UpdateChefDto>(jsonData);

            return View(values);
        }

        // UPDATE POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateChef(UpdateChefDto model)
        {
            // IsActive'i otomatik true yap
            model.IsActive = true;
            
            var client = _httpClientFactory.CreateClient();

            var jsonData = JsonConvert.SerializeObject(model);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"https://localhost:7136/api/Chef/{model.ChefId}", content);

            if (response.IsSuccessStatusCode)
                return RedirectToAction("ChefList");

            return View(model);
        }

        // DELETE
        public async Task<IActionResult> DeleteChef(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.DeleteAsync("https://localhost:7136/api/Chef/" + id);

            if (response.IsSuccessStatusCode)
                return RedirectToAction("ChefList");

            return BadRequest();
        }
    }
}
