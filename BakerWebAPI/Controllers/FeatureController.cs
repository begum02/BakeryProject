using BakerWebAPI.Context;
using BakerWebAPI.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BakerWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeatureController : ControllerBase
    {
        private readonly BakerContext _context;

        public FeatureController(BakerContext bakerContext)
        {
            _context = bakerContext;
        }

        // 🔹 Listeleme
        [HttpGet]
        public IActionResult FeatureList()
        {
            var values = _context.Features.Where(x=>x.IsActive).ToList();
            return Ok(values);
        }

        // 🔹 Tek kayıt (opsiyonel)
        [HttpGet("{id}")]
        public IActionResult GetFeature(int id)
        {
            var value = _context.Features.Find(id);
            if (value == null)
                return NotFound("Feature bulunamadı");

            return Ok(value);
        }

        // 🔹 Ekleme (DTO yok)
        [HttpPost]
        public IActionResult CreateFeature([FromBody] Feature feature)
        {
            feature.IsActive = true;
            _context.Features.Add(feature);

            _context.SaveChanges();

            return Ok("Ekleme işlemi başarıyla gerçekleşti");
        }

        // 🔹 Güncelleme (DTO yok)
        [HttpPut("{id}")]
        public IActionResult UpdateFeature(int id, [FromBody] Feature feature)
        {
            var entity = _context.Features.FirstOrDefault(x => x.FeatureId == id && x.IsActive);
            if (entity == null)
                return NotFound("Feature bulunamadı");

            entity.Title = feature.Title;
            entity.Subtitle = feature.Subtitle;
            entity.Description = feature.Description;
            entity.ImageUrl = feature.ImageUrl;
            entity.IsActive = feature.IsActive;

            _context.SaveChanges();

            return Ok("Güncelleme işlemi başarıyla gerçekleşti");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteFeature(int id)
        {
            var entity = _context.Features.Find(id);
            if (entity == null)
                return NotFound("Feature bulunamadı");

            if (!entity.IsActive)
                return BadRequest("Feature zaten yayında değil");

            entity.IsActive = false;
            _context.SaveChanges();

            return Ok("Feature yayından kaldırıldı");
        }
    }
}
