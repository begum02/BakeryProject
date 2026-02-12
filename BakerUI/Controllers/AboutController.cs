using BakerUI.Dto.AboutDto;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace BakerUI.Controllers
{
    public class AboutController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AboutController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync("https://localhost:7136/api/About");

            if (!response.IsSuccessStatusCode)
                return View(new ResultAboutDto());

            var jsonData = await response.Content.ReadAsStringAsync();

            // API liste dönüyor
            var list = JsonConvert.DeserializeObject<List<ResultAboutDto>>(jsonData) ?? new();
            var about = list.FirstOrDefault();

            if (about == null)
                return View(new ResultAboutDto());

            return View(about);
        }


        // ✅ Tek ekran: Edit (GET) -> varsa doldur, yoksa boş
        [HttpGet]
        public async Task<IActionResult> EditAbout()
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync("https://localhost:7136/api/About");

            // API hata verirse boş form aç
            if (!response.IsSuccessStatusCode)
                return View(new UpdateAboutDto());

            var jsonData = await response.Content.ReadAsStringAsync();

            // API liste dönüyor (1 kayıt)
            var list = JsonConvert.DeserializeObject<List<ResultAboutDto>>(jsonData) ?? new();
            var about = list.FirstOrDefault();

            // kayıt yoksa boş form
            if (about == null)
                return View(new UpdateAboutDto());

            // ✅ Result -> Update map (alan adları sende böyleyse birebir)
            var model = new UpdateAboutDto
            {
                AboutId = about.AboutId,
                Title = about.Title,
                Description = about.Description,
                ImageUrl1 = about.ImageUrl1,
                ImageUrl2 = about.ImageUrl2,
                AboutItems=about.AboutItems
            };

            return View(model);
        }

        // ✅ Tek POST: Id varsa PUT, yoksa POST
        [HttpPost]
        public async Task<IActionResult> EditAbout(UpdateAboutDto model)
        {
            var client = _httpClientFactory.CreateClient();
            var jsonData = JsonConvert.SerializeObject(model);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            HttpResponseMessage response;

            // ✅ varsa update
            if (model.AboutId > 0)
            {
                response = await client.PutAsync("https://localhost:7136/api/About/" + model.AboutId, content);
            }
            else
            {
                // ✅ yoksa create
                response = await client.PostAsync("https://localhost:7136/api/About", content);
            }

            if (response.IsSuccessStatusCode)
                return RedirectToAction("EditAbout");

            // hata varsa aynı view
            return View(model);
        }

        // (opsiyonel) Soft delete kullanacaksan kalsın
        [HttpGet]
        public async Task<IActionResult> DeleteAbout(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.DeleteAsync("https://localhost:7136/api/About/" + id);

            if (response.IsSuccessStatusCode)
                return RedirectToAction("EditAbout");

            return BadRequest();
        }
    }
}
