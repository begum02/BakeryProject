using BakerWebAPI.Context;
using BakerWebAPI.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BakerWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly BakerContext _context;

        public ProductController(BakerContext bakerContext)
        {
            _context = bakerContext;
        }

        // 🔹 Listeleme (CategoryName dahil, ama DTO yok)
        [HttpGet]
        public IActionResult ProductList()
        {
            var values = _context.Products.Where(x => x.IsActive)
                .Include(x => x.Category)
                .Select(x => new
                {
                    x.ProductId,
                    x.ProductName,
                    x.Description,
                    x.Price,
                    x.ImageUrl,
                    x.CategoryId,
                    CategoryName = x.Category.CategoryName
                })
                .ToList();

            return Ok(values);
        }

        // 🔹 Tek ürün (opsiyonel)
        [HttpGet("{id}")]
        public IActionResult GetProduct(int id)
        {
            var value = _context.Products
                .Include(x => x.Category)
                .Where(x => x.ProductId == id&&x.IsActive)
                .Select(x => new
                {
                    x.ProductId,
                    x.ProductName,
                    x.Description,
                    x.Price,
                    x.ImageUrl,
                    x.IsActive,
                    x.CategoryId,
                    CategoryName = x.Category.CategoryName
                })
                .FirstOrDefault();

            if (value == null)
                return NotFound("Ürün bulunamadı");

            return Ok(value);
        }

        // 🔹 Ekleme
        [HttpPost]
        public IActionResult CreateProduct([FromBody] Product product)
        {
            try
            {
                if (product == null)
                    return BadRequest("Product is null");

                // Validation
                if (string.IsNullOrWhiteSpace(product.ProductName))
                    return BadRequest("ProductName is required");

                if (string.IsNullOrWhiteSpace(product.ImageUrl))
                    return BadRequest("ImageUrl is required");

                if (product.Price <= 0)
                    return BadRequest("Price must be greater than 0");

                // Navigation'ı temizle
                product.Category = null!;
                product.IsActive = true;

                // Category var mı kontrol
                var categoryExists = _context.Categories.Any(c => c.CategoryId == product.CategoryId);
                if (!categoryExists)
                    return BadRequest($"Category with ID {product.CategoryId} not found");

                _context.Products.Add(product);
                _context.SaveChanges();

                return Ok("Ürün eklendi");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message} | InnerException: {ex.InnerException?.Message}");
            }
        }

        // 🔹 Güncelleme (DTO yok)
        [HttpPut("{id}")]
        public IActionResult UpdateProduct(int id, [FromBody] Product product)
        {
            var entity = _context.Products.FirstOrDefault(x => x.ProductId == id && x.IsActive);
            if (entity == null)
                return NotFound("Ürün bulunamadı");

            // Category var mı kontrolü (opsiyonel ama iyi)
            var categoryExists = _context.Categories.Any(c => c.CategoryId == product.CategoryId);
            if (!categoryExists)
                return BadRequest("Geçersiz CategoryId");

            entity.ProductName = product.ProductName;
            entity.Description = product.Description;
            entity.Price = product.Price;
            entity.ImageUrl = product.ImageUrl;
            entity.IsActive = product.IsActive;
            entity.CategoryId = product.CategoryId;

            _context.SaveChanges();
            return Ok("Ürün güncellendi");
        }

        // 🔹 Silme
        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            var entity = _context.Products.Find(id);
            if (entity == null)
                return NotFound("Ürün bulunamadı");

            entity.IsActive = false;
            _context.SaveChanges();  // ✅ BURAYI EKLEYİN

            return Ok("Ürün silindi");
        }
    }
}
