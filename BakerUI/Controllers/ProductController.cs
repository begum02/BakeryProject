using BakerUI.Dto.ProductDto;
using BakerUI.Dto.CategoryDto; // Bu satırı ekle
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering; // Bu da gerekli olabilir
using Newtonsoft.Json;
using System.Text;

namespace BakerUI.Controllers
{
    public class ProductController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ProductController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // LIST
        public async Task<IActionResult> Index(
            string? category,
            decimal? min,
            decimal? max,
            string? sort,
            int page = 1,
            int size = 9)
        {
            ViewBag.Category = category;
            ViewBag.Min = min;
            ViewBag.Max = max;
            ViewBag.Sort = sort;
            ViewBag.Page = page;
            ViewBag.PageSize = size;

            return View();
        }

        //Admin List
        [HttpGet]
        public async Task<IActionResult> ProductList()
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync("https://localhost:7136/api/Product");

            if (!response.IsSuccessStatusCode)
                return View(new List<ResultProductDto>());

            var jsonData = await response.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<ResultProductDto>>(jsonData)
                         ?? new List<ResultProductDto>();

            return View(values);
        }

        // CREATE GET
        [HttpGet]
        public IActionResult CreateProduct()
        {
            return View();
        }

        // CREATE POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProduct(CreateProductDto model)
        {
            if (!ModelState.IsValid)
            {
                // Categories'i yeniden yükle (ViewComponent için)
                var client2 = _httpClientFactory.CreateClient();
                var categoryResponse = await client2.GetAsync("https://localhost:7136/api/Category");
                
                if (categoryResponse.IsSuccessStatusCode)
                {
                    var categoryJson = await categoryResponse.Content.ReadAsStringAsync();
                    var categories = JsonConvert.DeserializeObject<List<ResultCategoryDto>>(categoryJson);
                    ViewBag.Categories = categories?.Select(c => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                    {
                        Value = c.CategoryId.ToString(),
                        Text = c.CategoryName
                    }).ToList();
                }
                
                return View(model);
            }

            var client = _httpClientFactory.CreateClient();
            
            // CategoryController'daki gibi direkt DTO gönder
            var jsonData = JsonConvert.SerializeObject(model);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            try
            {
                var response = await client.PostAsync("https://localhost:7136/api/Product", content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Ürün başarıyla eklendi!";
                    return RedirectToAction("ProductList");
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    ModelState.AddModelError("", $"Hata: {errorContent}");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Bağlantı hatası: {ex.Message}");
            }

            return View(model);
        }

        // UPDATE GET
        [HttpGet]
        public async Task<IActionResult> UpdateProduct(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync("https://localhost:7136/api/Product/" + id);

            if (!response.IsSuccessStatusCode)
                return RedirectToAction("ProductList");

            var jsonData = await response.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<UpdateProductDto>(jsonData);

            return View(values);
        }

        // UPDATE POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProduct(UpdateProductDto model)
        {
            // IsActive'i otomatik true yap
            model.IsActive = true;

            var client = _httpClientFactory.CreateClient();

            var jsonData = JsonConvert.SerializeObject(model);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"https://localhost:7136/api/Product/{model.ProductId}", content);

            if (response.IsSuccessStatusCode)
                return RedirectToAction("ProductList");

            var errorMessage = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError("", $"Güncelleme hatası: {errorMessage}");

            return View(model);
        }

        // DELETE
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.DeleteAsync("https://localhost:7136/api/Product/" + id);

            if (response.IsSuccessStatusCode)
                return RedirectToAction("ProductList");

            return BadRequest();
        }
    }
}
