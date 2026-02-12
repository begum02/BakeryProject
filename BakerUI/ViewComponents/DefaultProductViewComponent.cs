using BakerUI.Dto.ProductDto;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BakerUI.ViewComponents
{
    public class DefaultProductViewComponent : ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public DefaultProductViewComponent(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IViewComponentResult> InvokeAsync(
            string? category = null,
            decimal? min = null,
            decimal? max = null,
            string? sort = null,
            int page = 1,
            int size = 9)
        {
            // API'den tüm ürünleri çek
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync("https://localhost:7136/api/Product");

            var products = new List<ResultProductDto>();

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                products = JsonConvert.DeserializeObject<List<ResultProductDto>>(json) ?? new();
            }

            // Defaults
            category = string.IsNullOrWhiteSpace(category) ? "All" : category.Trim();
            sort = string.IsNullOrWhiteSpace(sort) ? "new" : sort.Trim().ToLowerInvariant();
            page = page < 1 ? 1 : page;
            size = size < 1 ? 9 : size;

            // ✅ Kategori sayıları (sidebar için - TÜM ürünler üzerinden)
            var categoryCounts = products
                .GroupBy(p => string.IsNullOrWhiteSpace(p.CategoryName) ? "Kategorisiz" : p.CategoryName!.Trim())
                .Select(g => new { Name = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .ThenBy(x => x.Name)
                .ToList();

            // ✅ FILTER: Kategori
            var isAll = category.Equals("All", StringComparison.OrdinalIgnoreCase);
            var filtered = isAll
                ? products
                : products.Where(p =>
                    string.Equals(
                        (p.CategoryName ?? "Kategorisiz").Trim(),
                        category,
                        StringComparison.OrdinalIgnoreCase
                    )
                ).ToList();

            // ✅ FILTER: Fiyat aralığı
            if (min.HasValue)
                filtered = filtered.Where(p => p.Price >= min.Value).ToList();

            if (max.HasValue)
                filtered = filtered.Where(p => p.Price <= max.Value).ToList();

            // ✅ SORT
            filtered = sort switch
            {
                "price_asc" => filtered.OrderBy(x => x.Price).ToList(),
                "price_desc" => filtered.OrderByDescending(x => x.Price).ToList(),
                "name_asc" => filtered.OrderBy(x => x.ProductName).ToList(),
                "name_desc" => filtered.OrderByDescending(x => x.ProductName).ToList(),
                _ => filtered.OrderByDescending(x => x.ProductId).ToList(), // en yeniler
            };

            // ✅ PAGING
            var totalCount = filtered.Count;
            var totalPages = Math.Max(1, (int)Math.Ceiling(totalCount / (double)size));
            page = Math.Min(page, totalPages);

            var paged = filtered
                .Skip((page - 1) * size)
                .Take(size)
                .ToList();

            // ✅ ViewBag ile view'a geç
            ViewBag.SelectedCategory = category;
            ViewBag.Min = min;
            ViewBag.Max = max;
            ViewBag.Sort = sort;
            ViewBag.Page = page;
            ViewBag.PageSize = size;
            ViewBag.TotalCount = totalCount;
            ViewBag.TotalPages = totalPages;
            ViewBag.CategoryCounts = categoryCounts;

            return View(paged);
        }
    }
}
