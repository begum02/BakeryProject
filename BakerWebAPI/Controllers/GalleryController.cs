using BakerWebAPI.Context;
using BakerWebAPI.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BakerWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GalleryController : ControllerBase
    {
        private readonly BakerContext _context;

        public GalleryController(BakerContext bakerContext)
        {
            _context = bakerContext;
        }

        // 🔹 Listeleme (CategoryName dahil)
        [HttpGet]
        public IActionResult GalleryList()
        {
            var values = _context.Galleries
                .Where(x => x.IsActive)
                .Include(x => x.Category)
                .Select(x => new
                {
                    x.GalleryId,
                    x.ImageUrl,
                    x.Title,
                    x.Description,
                    x.CategoryId,
                    CategoryName = x.Category != null ? x.Category.CategoryName : null,
                    x.IsActive
                })
                .ToList();

            return Ok(values);
        }

        // 🔹 Tek kayıt
        [HttpGet("{id}")]
        public IActionResult GetGallery(int id)
        {
            var value = _context.Galleries
                .Include(x => x.Category)
                .Where(x => x.GalleryId == id)
                .Select(x => new
                {
                    x.GalleryId,
                    x.ImageUrl,
                    x.Title,
                    x.Description,
                    x.CategoryId,
                    CategoryName = x.Category != null ? x.Category.CategoryName : null,
                    x.IsActive
                })
                .FirstOrDefault();

            if (value == null)
                return NotFound("Galeri kaydı bulunamadı");

            return Ok(value);
        }

        // 🔹 Ekleme
        [HttpPost]
        public IActionResult CreateGallery([FromBody] Gallery gallery)
        {
            try
            {
                if (gallery == null)
                    return BadRequest("Gallery is null");

                // Validation
                if (string.IsNullOrWhiteSpace(gallery.ImageUrl))
                    return BadRequest("ImageUrl is required");

                // Navigation'ı temizle
                gallery.Category = null;
                gallery.IsActive = true;

                // CategoryId varsa kontrol et
                if (gallery.CategoryId.HasValue)
                {
                    var categoryExists = _context.Categories.Any(c => c.CategoryId == gallery.CategoryId.Value);
                    if (!categoryExists)
                        return BadRequest($"Category with ID {gallery.CategoryId} not found");
                }

                _context.Galleries.Add(gallery);
                _context.SaveChanges();

                return Ok("Ekleme işlemi başarıyla gerçekleşti");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message} | InnerException: {ex.InnerException?.Message}");
            }
        }

        // 🔹 Güncelleme
        [HttpPut("{id}")]
        public IActionResult UpdateGallery(int id, [FromBody] Gallery gallery)
        {
            try
            {
                var entity = _context.Galleries.FirstOrDefault(x => x.GalleryId == id);
                if (entity == null)
                    return NotFound("Galeri kaydı bulunamadı");

                // CategoryId varsa kontrol et
                if (gallery.CategoryId.HasValue)
                {
                    var categoryExists = _context.Categories.Any(c => c.CategoryId == gallery.CategoryId.Value);
                    if (!categoryExists)
                        return BadRequest($"Category with ID {gallery.CategoryId} not found");
                }

                entity.ImageUrl = gallery.ImageUrl;
                entity.Title = gallery.Title;
                entity.Description = gallery.Description;
                entity.CategoryId = gallery.CategoryId;
                entity.IsActive = gallery.IsActive;

                _context.SaveChanges();

                return Ok("Güncelleme işlemi başarıyla gerçekleşti");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        // 🔹 Silme
        [HttpDelete("{id}")]
        public IActionResult DeleteGallery(int id)
        {
            var entity = _context.Galleries.Find(id);
            if (entity == null)
                return NotFound("Galeri kaydı bulunamadı");

            entity.IsActive = false;
            _context.SaveChanges();

            return Ok("Galeri yayından kaldırıldı");
        }
    }
}
