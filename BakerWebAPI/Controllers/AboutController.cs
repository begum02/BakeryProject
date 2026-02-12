using BakerWebAPI.Context;
using BakerWebAPI.Entities;
using BakerWebAPI.Dto.AboutDto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BakerWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AboutController : ControllerBase
    {
        private readonly BakerContext _context;

        public AboutController(BakerContext bakerContext)
        {
            _context = bakerContext;
        }

        [HttpGet("Index")]
        public IActionResult Index()
        {
            var about = _context.Abouts
                .Include(x => x.AboutItems)
                .Where(x => x.IsActive)
                .OrderByDescending(x => x.AboutId)
                .FirstOrDefault();

            if (about == null)
                return Ok(null);

            return Ok(about);
        }

        [HttpGet]
        public IActionResult AboutList()
        {
            var values = _context.Abouts
                .Include(x => x.AboutItems)
                .Where(x => x.IsActive)
                .ToList();

            return Ok(values);
        }

        [HttpGet("{id}")]
        public IActionResult GetAbout(int id)
        {
            var value = _context.Abouts
                .Include(x => x.AboutItems)
                .FirstOrDefault(x => x.AboutId == id && x.IsActive);

            if (value == null) return NotFound("About bulunamadı");
            return Ok(value);
        }

        [HttpPost]
        public IActionResult CreateAbout([FromBody] UpdateAboutDto aboutDto)
        {
            var exists = _context.Abouts.Any(x => x.IsActive);
            if (exists)
                return Conflict("About kaydı zaten var. İkinci kayıt eklenemez.");

            var about = new About
            {
                Title = aboutDto.Title,
                Description = aboutDto.Description,
                ImageUrl1 = aboutDto.ImageUrl1,
                ImageUrl2 = aboutDto.ImageUrl2,
                IsActive = true,
                CreatedDate = DateTime.Now,
                AboutItems = (aboutDto.AboutItems ?? new List<AboutItemDto>())
                    .Where(i => !string.IsNullOrWhiteSpace(i.Text))
                    .Select(i => new AboutItem { Text = i.Text.Trim() })
                    .ToList()
            };

            _context.Abouts.Add(about);
            _context.SaveChanges();

            return Ok("Ekleme işlemi başarıyla gerçekleşti");
        }

        [HttpPut("{id}")]
        public IActionResult UpdateAbout(int id, [FromBody] UpdateAboutDto aboutDto)
        {
            var entity = _context.Abouts
                .Include(x => x.AboutItems)
                .FirstOrDefault(x => x.AboutId == id && x.IsActive);

            if (entity == null) return NotFound("About bulunamadı");

            // Ana bilgileri güncelle
            entity.Title = aboutDto.Title;
            entity.Description = aboutDto.Description;
            entity.ImageUrl1 = aboutDto.ImageUrl1 ?? string.Empty;
            entity.ImageUrl2 = aboutDto.ImageUrl2 ?? string.Empty;

            // ✅ Mevcut items'ı sil
            _context.AboutItems.RemoveRange(entity.AboutItems);

            // ✅ Yeni items'ları ekle
            entity.AboutItems = (aboutDto.AboutItems ?? new List<AboutItemDto>())
                .Where(i => !string.IsNullOrWhiteSpace(i.Text))
                .Select(i => new AboutItem 
                { 
                    Text = i.Text.Trim(), 
                    AboutId = entity.AboutId 
                })
                .ToList();

            _context.SaveChanges();
            return Ok("Güncelleme işlemi başarıyla gerçekleşti");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteAbout(int id)
        {
            var about = _context.Abouts.FirstOrDefault(x => x.AboutId == id && x.IsActive);
            if (about == null) return NotFound("About bulunamadı");

            about.IsActive = false;
            _context.SaveChanges();

            return Ok("About yayından kaldırıldı");
        }
    }
}
