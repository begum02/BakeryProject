using BakerWebAPI.Context;
using BakerWebAPI.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BakerWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscribeController : ControllerBase
    {
        private readonly BakerContext _context;

        public SubscribeController(BakerContext bakerContext)
        {
            _context = bakerContext;
        }

        [HttpGet]
        public IActionResult SubscribeList()
        {
            var values = _context.Subscribes.Where(x => x.IsActive)
                .OrderByDescending(x => x.CreatedDate)
                .ToList();

            return Ok(values);
        }

        [HttpGet("{id}")]
        public IActionResult GetSubscribe(int id)
        {
            var value = _context.Subscribes.FirstOrDefault(x=>x.SubscribeId==id&&x.IsActive);
            if (value == null)
                return NotFound("Abonelik bulunamadı");

            return Ok(value);
        }

        [HttpPost]
        public IActionResult CreateSubscribe([FromBody] Subscribe subscribe)
        {
            // Client'ın bunları manipüle etmesini engelle
            subscribe.IsActive = true;
            subscribe.CreatedDate = DateTime.Now;

            _context.Subscribes.Add(subscribe);
            _context.SaveChanges();

            return Ok("Abonelik oluşturuldu");
        }

        // Güncelleme: Email + IsActive güncellenebilir (istersen sadece IsActive bırakırız)
        [HttpPut("{id}")]
        public IActionResult UpdateSubscribe(int id, [FromBody] Subscribe subscribe)
        {
            var entity = _context.Subscribes.FirstOrDefault(x => x.SubscribeId == id && x.IsActive);
            if (entity == null)
                return NotFound("Abonelik bulunamadı");

            entity.Email = subscribe.Email;
            entity.IsActive = subscribe.IsActive;

            // CreatedDate genelde güncellenmez

            _context.SaveChanges();
            return Ok("Güncelleme işlemi başarıyla gerçekleşti");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteSubscribe(int id)
        {
            var entity = _context.Subscribes.Find(id);
            if (entity == null)
                return NotFound("Abonelik bulunamadı");

           entity.IsActive = false; // Soft delete
                _context.SaveChanges();


            return Ok("Silme işlemi başarıyla gerçekleşti");
        }
    }
}
