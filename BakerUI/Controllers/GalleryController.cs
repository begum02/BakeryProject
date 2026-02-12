using BakerUI.Dto.GalleryDto;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace BakerUI.Controllers
{
    public class GalleryController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public GalleryController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // LIST
        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync("https://localhost:7136/api/Gallery");

            if (!response.IsSuccessStatusCode)
                return View();

            var jsonData = await response.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<ResultGalleryDto>>(jsonData);

            return View(values);
        }

        public async Task<IActionResult> GalleryList()
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync("https://localhost:7136/api/Gallery");

            if (!response.IsSuccessStatusCode)
                return View("Index", new List<ResultGalleryDto>());

            var jsonData = await response.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<ResultGalleryDto>>(jsonData) ?? new();

            return View(values);
        }

        // CREATE GET
        [HttpGet]
        public IActionResult CreateGallery()
        {
            return View();
        }

        // CREATE POST
        [HttpPost]
        public async Task<IActionResult> CreateGallery(CreateGalleryDto model)
        {
            var client = _httpClientFactory.CreateClient();

            var jsonData = JsonConvert.SerializeObject(model);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("https://localhost:7136/api/Gallery", content);

            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            return View(model);
        }

        // UPDATE GET
        [HttpGet]
        public async Task<IActionResult> UpdateGallery(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync("https://localhost:7136/api/Gallery/" + id);

            if (!response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            var jsonData = await response.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<UpdateGalleryDto>(jsonData);

            return View(values);
        }

        // UPDATE POST
        [HttpPost]
        public async Task<IActionResult> UpdateGallery(int id, UpdateGalleryDto model)
        {
            var client = _httpClientFactory.CreateClient();

            var jsonData = JsonConvert.SerializeObject(model);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var response = await client.PutAsync("https://localhost:7136/api/Gallery/" + id, content);

            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            return View(model);
        }

        // DELETE
        public async Task<IActionResult> DeleteGallery(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.DeleteAsync("https://localhost:7136/api/Gallery/" + id);

            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            return BadRequest();
        }
    }
}
