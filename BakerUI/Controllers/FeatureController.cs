using BakerUI.Dto.FeatureDto;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace BakerUI.Controllers
{
    public class FeatureController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private const string ApiBase = "https://localhost:7136/api/Feature";

        public FeatureController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // LIST
        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync(ApiBase);

            if (!response.IsSuccessStatusCode)
                return View(new List<ResultFeatureDto>());

            var jsonData = await response.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<ResultFeatureDto>>(jsonData) ?? new();

            return View(values);
        }

        // CREATE GET (tek kayıt varsa Create'e izin verme)
        [HttpGet]
        public async Task<IActionResult> CreateFeature()
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync(ApiBase);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var list = JsonConvert.DeserializeObject<List<ResultFeatureDto>>(json) ?? new();

                var existing = list.FirstOrDefault();
                if (existing != null)
                {
                    // kayıt varsa update sayfasına yönlendir
                    return RedirectToAction("UpdateFeature", new { id = existing.FeatureId });
                }
            }

            return View();
        }

        // CREATE POST (tek kayıt varsa engelle)
        [HttpPost]
        public async Task<IActionResult> CreateFeature(CreateFeatureDto model)
        {
            var client = _httpClientFactory.CreateClient();

            // önce mevcut kayıt var mı kontrol et
            var checkResponse = await client.GetAsync(ApiBase);
            if (checkResponse.IsSuccessStatusCode)
            {
                var checkJson = await checkResponse.Content.ReadAsStringAsync();
                var list = JsonConvert.DeserializeObject<List<ResultFeatureDto>>(checkJson) ?? new();

                if (list.Any())
                {
                    ModelState.AddModelError("", "Sistemde zaten bir Feature kaydı var. Lütfen güncelleme yapın.");
                    return View(model);
                }
            }

            var jsonData = JsonConvert.SerializeObject(model);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(ApiBase, content);

            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            return View(model);
        }

        // UPDATE GET
        [HttpGet]
        public async Task<IActionResult> UpdateFeature(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"{ApiBase}/{id}");

            if (!response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            var jsonData = await response.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<UpdateFeatureDto>(jsonData);

            return View(values);
        }

        // UPDATE POST
        [HttpPost]
        public async Task<IActionResult> UpdateFeature(int id, UpdateFeatureDto model)
        {
            var client = _httpClientFactory.CreateClient();

            var jsonData = JsonConvert.SerializeObject(model);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"{ApiBase}/{id}", content);

            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            return View(model);
        }

        // DELETE (istersen silmeyi tamamen kapatabilirsin)
        public async Task<IActionResult> DeleteFeature(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.DeleteAsync($"{ApiBase}/{id}");

            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            return BadRequest();
        }
    }
}
