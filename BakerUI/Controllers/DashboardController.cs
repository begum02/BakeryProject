using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BakerUI.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public DashboardController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient();

            // Toplam sayılar
            var productCount = await GetCountAsync(client, "https://localhost:7136/api/Product");
            var serviceCount = await GetCountAsync(client, "https://localhost:7136/api/Service");
            var galleryCount = await GetCountAsync(client, "https://localhost:7136/api/Gallery");
            var chefCount = await GetCountAsync(client, "https://localhost:7136/api/Chef");
            var testimonialCount = await GetCountAsync(client, "https://localhost:7136/api/Testimonial");
            
            var (messageCount, unreadCount) = await GetMessageStatsAsync(client);

            // ✅ Kategori bazlı ürün dağılımı (grafik için)
            var categoryStats = await GetCategoryStatsAsync(client);

            // ViewBag
            ViewBag.ProductCount = productCount;
            ViewBag.ServiceCount = serviceCount;
            ViewBag.GalleryCount = galleryCount;
            ViewBag.ChefCount = chefCount;
            ViewBag.TestimonialCount = testimonialCount;
            ViewBag.MessageCount = messageCount;
            ViewBag.UnreadMessageCount = unreadCount;
            
            // ✅ Grafik verisi (AJAX yerine direkt ViewBag)
            ViewBag.CategoryStats = categoryStats;

            // Partial view'lar için
            ViewBag.RecentProducts = await GetRecentProductsAsync(client);
            ViewBag.RecentMessages = await GetRecentMessagesAsync(client);

            return View();
        }

        // Helper: API'den count al
        private async Task<int> GetCountAsync(HttpClient client, string url)
        {
            try
            {
                var response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var items = JsonConvert.DeserializeObject<List<dynamic>>(json);
                    return items?.Count ?? 0;
                }
            }
            catch { }
            return 0;
        }

        // Helper: Mesaj istatistikleri
        private async Task<(int total, int unread)> GetMessageStatsAsync(HttpClient client)
        {
            try
            {
                var response = await client.GetAsync("https://localhost:7136/api/Message");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var messages = JsonConvert.DeserializeObject<List<dynamic>>(json);
                    var total = messages?.Count ?? 0;
                    var unread = messages?.Count(m => !(bool)(m.IsRead ?? true)) ?? 0;
                    return (total, unread);
                }
            }
            catch { }
            return (0, 0);
        }

        // ✅ Kategori istatistikleri (AJAX yerine server-side)
        private async Task<List<object>> GetCategoryStatsAsync(HttpClient client)
        {
            try
            {
                var response = await client.GetAsync("https://localhost:7136/api/Product");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var products = JsonConvert.DeserializeObject<List<BakerUI.Dto.ProductDto.ResultProductDto>>(json);

                    var categoryStats = products?
                        .GroupBy(p => string.IsNullOrWhiteSpace(p.CategoryName) ? "Kategorisiz" : p.CategoryName)
                        .Select(g => new
                        {
                            category = g.Key,
                            count = g.Count()
                        })
                        .OrderByDescending(x => x.count)
                        .Cast<object>()
                        .ToList();

                    return categoryStats ?? new List<object>();
                }
            }
            catch { }
            return new List<object>();
        }

        // Son 5 ürün
        private async Task<List<BakerUI.Dto.ProductDto.ResultProductDto>> GetRecentProductsAsync(HttpClient client)
        {
            try
            {
                var response = await client.GetAsync("https://localhost:7136/api/Product");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var products = JsonConvert.DeserializeObject<List<BakerUI.Dto.ProductDto.ResultProductDto>>(json);
                    return products?
                        .OrderByDescending(p => p.ProductId)
                        .Take(5)
                        .ToList() ?? new List<BakerUI.Dto.ProductDto.ResultProductDto>();
                }
            }
            catch { }
            return new List<BakerUI.Dto.ProductDto.ResultProductDto>();
        }

        // Son 5 mesaj
        private async Task<List<BakerUI.Dto.MessageDto.ResultMessageDto>> GetRecentMessagesAsync(HttpClient client)
        {
            try
            {
                var response = await client.GetAsync("https://localhost:7136/api/Message");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var messages = JsonConvert.DeserializeObject<List<BakerUI.Dto.MessageDto.ResultMessageDto>>(json);
                    return messages?
                        .OrderByDescending(m => m.MessageId)
                        .Take(5)
                        .ToList() ?? new List<BakerUI.Dto.MessageDto.ResultMessageDto>();
                }
            }
            catch { }
            return new List<BakerUI.Dto.MessageDto.ResultMessageDto>();
        }
    }
}
