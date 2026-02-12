using BakerWebAPI.Context;
using BakerWebAPI.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BakerWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly BakerContext _context;

        public CategoryController(BakerContext bakerContext)
        {
            _context = bakerContext;
        }

        // ✅ Listeleme: IsActive bilgisini de dön
        [HttpGet]
        public IActionResult CategoryList()
        {
            var values = _context.Categories.Where(x => x.IsActive)
                .Select(x => new
                {
                    x.CategoryId,
                    x.CategoryName,
                    x.CategoryDescription,
                    x.ImageUrl,
                    x.PriceRange,
                    x.IsActive  // ✅ IsActive bilgisini ekle
                }).ToList();

            return Ok(values);
        }

        // ✅ Tek kayıt (opsiyonel ama faydalı)
        // ✅ Tek kayıt - Update için pasif kategorileri de al, IsActive bilgisini dön
        [HttpGet("{id}")]
        public IActionResult GetCategory(int id)
        {
            var value = _context.Categories
                .Where(x => x.CategoryId == id)  // IsActive kontrolünü kaldır
                .Select(x => new
                {
                    x.CategoryId,
                    x.CategoryName,
                    x.CategoryDescription,
                    x.ImageUrl,
                    x.PriceRange,
                    x.IsActive  // ✅ IsActive bilgisini ekle
                })
                .FirstOrDefault();

            if (value == null)
                return NotFound("Kategori bulunamadı");

            return Ok(value);
        }

        // ✅ Ekleme: Body'den direkt Entity alıyoruz
        [HttpPost]
        public IActionResult CreateCategory([FromBody] Category category)
        {
            // Navigation'in yanlışlıkla dolu gelmesini istemiyorsan temizle:
            category.Products = new List<Product>();

            category.IsActive = true;

            _context.Categories.Add(category);
            _context.SaveChanges();

            return Ok("Kategori ekleme işlemi başarıyla gerçekleşti");
        }

        // ✅ Güncelleme: id ile DB'den çekip alanları güncelliyoruz
        [HttpPut("{id}")]
        public IActionResult UpdateCategory(int id, [FromBody] Category category)
        {
            var entity = _context.Categories.FirstOrDefault(x => x.CategoryId == id);  // IsActive kontrolü YOK
            if (entity == null)
                return NotFound("Kategori bulunamadı");

            entity.CategoryName = category.CategoryName;
            entity.CategoryDescription = category.CategoryDescription;
            entity.ImageUrl = category.ImageUrl;
            entity.PriceRange = category.PriceRange;
            entity.IsActive = category.IsActive;  // ✅ IsActive'i de güncelle

            _context.SaveChanges();
            return Ok("Güncelleme işlemi başarıyla gerçekleşti");
        }
        // ✅ Silme: id ile DB'den çekip IsActive = false yapıyoruz

        [HttpDelete("{id}")]
        public IActionResult DeleteCategory(int id)
        {
            var entity = _context.Categories.Find(id);
            if (entity == null)
                return NotFound("Kategori bulunamadı");

            if (!entity.IsActive)
                return BadRequest("Kategori zaten yayında değil");

            entity.IsActive = false;
            _context.SaveChanges();

            return Ok("Kategori yayından kaldırıldı");
        }
    }
}