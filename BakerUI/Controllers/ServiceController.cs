using BakerUI.Dto.ServiceDto;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace BakerUI.Controllers
{
    public class ServiceController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ServiceController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // =========================
        // ✅ UI: Aktif service göster
        // =========================
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync("https://localhost:7136/api/Service/Index");

            if (!response.IsSuccessStatusCode)
                return View(new ResultServiceDto());

            var jsonData = await response.Content.ReadAsStringAsync();
            var value = JsonConvert.DeserializeObject<ResultServiceDto>(jsonData) ?? new ResultServiceDto();

            return View(value);
        }

        // =========================
        // ✅ ADMIN: Tek ekran Edit (GET)
        // =========================
        [HttpGet]
        public async Task<IActionResult> EditService()
        {
            var client = _httpClientFactory.CreateClient();

            // Admin list endpoint (liste döner)
            var response = await client.GetAsync("https://localhost:7136/api/Service");

            if (!response.IsSuccessStatusCode)
                return View(new UpdateServiceDto());

            var jsonData = await response.Content.ReadAsStringAsync();

            // API liste dönüyor
            var list = JsonConvert.DeserializeObject<List<ResultServiceDto>>(jsonData) ?? new();
            var service = list.FirstOrDefault();

            // kayıt yoksa boş form
            if (service == null)
                return View(new UpdateServiceDto());

            // Result -> Update map
            var model = new UpdateServiceDto
            {
                ServiceId = service.ServiceId,
                Title = service.Title,
                Description = service.Description,
                ImageUrl1 = service.ImageUrl1,
                ImageUrl2 = service.ImageUrl2,
                IsActive = service.IsActive,
                Items = service.Items ?? new()
            };

            // güvenlik: item serviceId set
            foreach (var it in model.Items)
                it.ServiceId = model.ServiceId;

            return View(model);
        }

        // =========================
        // ✅ ADMIN: Tek POST (ServiceId varsa PUT, yoksa POST)
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditService(UpdateServiceDto model)
        {
            var client = _httpClientFactory.CreateClient();

            // item güvenliği
            if (model.Items != null)
            {
                foreach (var it in model.Items)
                {
                    it.ServiceId = model.ServiceId;

                    // yeni eklenen itemlar için default aktif
                    // (checkbox yoksa false gitmesin diye)
                    // it.IsActive = it.IsActive; // gerek yok; DTO'da default true verdik
                }
            }

            var jsonData = JsonConvert.SerializeObject(model);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            HttpResponseMessage response;

            // ✅ varsa update
            if (model.ServiceId > 0)
            {
                response = await client.PutAsync("https://localhost:7136/api/Service/" + model.ServiceId, content);
            }
            else
            {
                // ✅ yoksa create
                response = await client.PostAsync("https://localhost:7136/api/Service", content);
            }

            if (response.IsSuccessStatusCode)
                return RedirectToAction(nameof(EditService));

            // hata varsa aynı view
            return View(model);
        }

        // =========================
        // ✅ ADMIN: Soft delete
        // =========================
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.DeleteAsync("https://localhost:7136/api/Service/" + id);

            if (response.IsSuccessStatusCode)
                return RedirectToAction(nameof(EditService));

            return BadRequest();
        }
    }
}
