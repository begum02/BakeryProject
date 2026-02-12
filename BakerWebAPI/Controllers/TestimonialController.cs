using BakerWebAPI.Context;
using BakerWebAPI.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace BakerWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestimonialController : ControllerBase
    {
        private readonly BakerContext _context;

        public TestimonialController(BakerContext bakerContext)
        {
            _context = bakerContext;
        }

        // ✅ Public – sadece aktif yorumlar
        // GET: api/Testimonial
        [HttpGet]
        public IActionResult TestimonialList()
        {
            var values = _context.Testimonials
                .Where(x => x.IsActive)
                .OrderByDescending(x => x.CreatedDate)
                .ToList();

            return Ok(values);
        }

        // ✅ Admin – sadece aktif yorumlar (PASİFLERİ GÖSTERME)
        // GET: api/Testimonial/admin
        [HttpGet("admin")]
        public IActionResult TestimonialAdminList()
        {
            var values = _context.Testimonials
                .Where(x => x.IsActive) // ✅ adminde de silinmiş/pasif görünmesin
                .OrderByDescending(x => x.CreatedDate)
                .ToList();

            return Ok(values);
        }

        // ✅ Tek kayıt – sadece aktif dön
        // GET: api/Testimonial/{id}
        [HttpGet("{id:int}")]
        public IActionResult GetTestimonial(int id)
        {
            var value = _context.Testimonials
                .FirstOrDefault(x => x.TestimonialId == id && x.IsActive);

            if (value == null)
                return NotFound("Yorum bulunamadı");

            return Ok(value);
        }

        // ✅ Ekleme (DTO yok)
        // POST: api/Testimonial
        [HttpPost]
        public IActionResult CreateTestimonial([FromBody] Testimonial testimonial)
        {
            if (testimonial == null)
                return BadRequest("Geçersiz veri");

            testimonial.CreatedDate = DateTime.Now;
            testimonial.IsActive = true;

            _context.Testimonials.Add(testimonial);
            _context.SaveChanges();

            return Ok("Yorum eklendi");
        }

        // ✅ Güncelleme (admin) – sadece aktif kayıt güncellensin
        // PUT: api/Testimonial/{id}
        [HttpPut("{id:int}")]
        public IActionResult UpdateTestimonial(int id, [FromBody] Testimonial testimonial)
        {
            if (testimonial == null)
                return BadRequest("Geçersiz veri");

            var entity = _context.Testimonials
                .FirstOrDefault(x => x.TestimonialId == id && x.IsActive);

            if (entity == null)
                return NotFound("Yorum bulunamadı");

            entity.NameSurname = testimonial.NameSurname;
            entity.Title = testimonial.Title;
            entity.Comment = testimonial.Comment;
            entity.ImageUrl = testimonial.ImageUrl;

            // İstersen admin aktif/pasif de değiştirebilsin:
            entity.IsActive = testimonial.IsActive;

            // CreatedDate güncellenmez
            _context.SaveChanges();

            return Ok("Güncelleme işlemi başarıyla gerçekleşti");
        }

        // ✅ Soft delete (pasife çekme)
        // DELETE: api/Testimonial/{id}
        [HttpDelete("{id:int}")]
        public IActionResult DeleteTestimonial(int id)
        {
            var entity = _context.Testimonials
                .FirstOrDefault(x => x.TestimonialId == id && x.IsActive);

            if (entity == null)
                return NotFound("Yorum bulunamadı");

            entity.IsActive = false;
            _context.SaveChanges();

            return Ok("Yorum yayından kaldırıldı");
        }

        // (Opsiyonel) Aynı işi yapıyor: pasife al
        // PUT: api/Testimonial/deactivate/{id}
        [HttpPut("deactivate/{id:int}")]
        public IActionResult DeactivateTestimonial(int id)
        {
            var entity = _context.Testimonials
                .FirstOrDefault(x => x.TestimonialId == id && x.IsActive);

            if (entity == null)
                return NotFound("Yorum bulunamadı");

            entity.IsActive = false;
            _context.SaveChanges();

            return Ok("Yorum pasif hale getirildi");
        }
    }
}
