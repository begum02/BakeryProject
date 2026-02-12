using BakerWebAPI.Context;
using BakerWebAPI.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BakerWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChefController : ControllerBase
    {
        private readonly BakerContext _context;

        public ChefController(BakerContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult ChefList()
        {
            // Entity direkt dönebilir (navigation yok)
            var values = _context.Chefs.Where(x=>x.IsActive).ToList();
            return Ok(values);
        }

        // (Opsiyonel) Tek kayıt
        [HttpGet("{id}")]
        public IActionResult GetChef(int id)
        {
            var value = _context.Chefs
        .FirstOrDefault(x => x.ChefId == id && x.IsActive);
            if (value == null)
                return NotFound("Chef bulunamadı");

            return Ok(value);
        }

        [HttpPost]
        public IActionResult CreateChef([FromBody] Chef chef)
        {
            // Default istersen garanti et:
            chef.IsActive = true;

            _context.Chefs.Add(chef);
            _context.SaveChanges();

            return Ok("Chef ekleme işlemi başarıyla gerçekleşti");
        }

        [HttpPut("{id}")]
        public IActionResult UpdateChef(int id, [FromBody] Chef chef)
        {
            var entity = _context.Chefs.FirstOrDefault(x => x.ChefId == id && x.IsActive);
            if (entity == null)
                return NotFound("Chef bulunamadı");

            entity.Name = chef.Name;
            entity.Title = chef.Title;
            entity.ImageUrl = chef.ImageUrl;
            entity.IsActive = chef.IsActive;

            _context.SaveChanges();

            return Ok("Güncelleme işlemi başarıyla gerçekleşti");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteChef(int id)
        {
            var entity = _context.Chefs.Find(id);
            if (entity == null)
                return NotFound("Chef bulunamadı");

            if (!entity.IsActive)
                return BadRequest("Chef zaten aktif değil");

            entity.IsActive = false;
            _context.SaveChanges();

            return Ok("Chef yayından kaldırıldı");
        }

    }
}
