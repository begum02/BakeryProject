
using BakerWebAPI.Dto.MessageDto;
using BakerWebAPI.Context;
using BakerWebAPI.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BakerWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly BakerContext _context;

        public MessageController(BakerContext bakerContext)
        {
            _context = bakerContext;
        }

        // 🔹 Listeleme (Admin)
        [HttpGet]
        public IActionResult MessageList()
        {
            var values = _context.Messages.Where(x => x.IsActive)
                .OrderByDescending(x => x.SendDate)
                .ToList();

            return Ok(values);
        }

        // 🔹 Tek mesaj (Admin)
        [HttpGet("{id}")]
        public IActionResult GetMessage(int id)
        {
            var value = _context.Messages.FirstOrDefault(x => x.MessageId == id && x.IsActive);
            if (value == null)
                return NotFound("Mesaj bulunamadı");

            return Ok(value);
        }

        // 🔹 Mesaj gönderme (Public)
        [HttpPost]
        public IActionResult CreateMessage([FromBody] Message message)
        {
            // Güvenli defaultlar
            message.SendDate = DateTime.Now;
            message.IsRead = false;
            message.IsActive = true;

            _context.Messages.Add(message);
            _context.SaveChanges();

            return Ok("Mesaj gönderildi");
        }

         //🔹 Okundu / okunmadı güncelleme(Admin)
        [HttpPut("{id}")]
        public IActionResult UpdateMessage(int id, [FromBody] UpdateMessageDto updateDto)
        {
            var entity = _context.Messages.FirstOrDefault(x => x.MessageId == id && x.IsActive);
            if (entity == null)
                return NotFound("Mesaj bulunamadı");

            // Sadece IsRead alanı güncelleniyor
            entity.IsRead = updateDto.IsRead;

            _context.SaveChanges();

            return Ok("Güncelleme işlemi başarıyla gerçekleşti");
        }

         //🔹 Mesaj güncelleme(Admin)


        // 🔹 Silme
        [HttpDelete("{id}")]
        public IActionResult DeleteMessage(int id)
        {
            var entity = _context.Messages.Find(id);
            if (entity == null)
                return NotFound("Mesaj bulunamadı");

            entity.IsActive = false; // Soft delete
            _context.SaveChanges(); // ✅ EKLENDİ
            return Ok("Silme işlemi başarıyla gerçekleşti");
        }
    }
}
