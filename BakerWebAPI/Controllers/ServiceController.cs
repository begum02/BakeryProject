using BakerWebAPI.Dto.ServiceDto;
using BakerWebAPI.Context;
using BakerWebAPI.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BakerWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly BakerContext _context;
        private const int MAX_ITEMS = 4;

        public ServiceController(BakerContext context)
        {
            _context = context;
        }

        // ✅ UI/Home: aktif Service + aktif Items
        // GET: api/Service/Index
        [HttpGet("Index")]
        public IActionResult Index()
        {
            var service = _context.Services
                .Include(x => x.Items)
                .Where(x => x.IsActive)
                .OrderByDescending(x => x.ServiceId)
                .FirstOrDefault();

            if (service == null)
            {
                return Ok(new
                {
                    Title = (string?)null,
                    Description = (string?)null,
                    ImageUrl1 = (string?)null,
                    ImageUrl2 = (string?)null,
                    Items = Array.Empty<object>()
                });
            }

            return Ok(new
            {
                service.ServiceId,
                service.Title,
                service.Description,
                service.ImageUrl1,
                service.ImageUrl2,
                service.IsActive,
                Items = service.Items
                    .Where(i => i.IsActive)
                    .OrderBy(i => i.ServiceItemId)
                    .Select(i => new
                    {
                        i.ServiceItemId,
                        i.Title,
                        i.Description,
                        i.Icon,
                        i.ServiceId
                    })
                    .ToList()
            });
        }

        // ✅ ADMIN: Sadece AKTİF service list + AKTİF items (en fazla 4 item)
        // GET: api/Service
        [HttpGet]
        public IActionResult ServiceList()
        {
            var values = _context.Services
                .Where(x => x.IsActive)
                .Include(x => x.Items)
                .OrderByDescending(x => x.ServiceId)
                .Select(x => new
                {
                    x.ServiceId,
                    x.Title,
                    x.Description,
                    x.ImageUrl1,
                    x.ImageUrl2,
                    x.IsActive,

                    Items = x.Items
                        .Where(i => i.IsActive)
                        .OrderBy(i => i.ServiceItemId)
                        .Take(MAX_ITEMS)
                        .Select(i => new
                        {
                            i.ServiceItemId,
                            i.Title,
                            i.Description,
                            i.Icon,
                            i.IsActive,
                            i.ServiceId
                        })
                        .ToList(),

                    TotalItemCount = x.Items.Count(i => i.IsActive)
                })
                .ToList();

            return Ok(values);
        }

        // ✅ ADMIN: Tek service (sadece AKTİF olan) + Items
        // GET: api/Service/{id}
        [HttpGet("{id:int}")]
        public IActionResult GetService(int id)
        {
            var value = _context.Services
                .Where(x => x.ServiceId == id && x.IsActive)
                .Include(x => x.Items)
                .Select(x => new
                {
                    x.ServiceId,
                    x.Title,
                    x.Description,
                    x.ImageUrl1,
                    x.ImageUrl2,
                    x.IsActive,

                    Items = x.Items
                        .OrderBy(i => i.ServiceItemId)
                        .Select(i => new
                        {
                            i.ServiceItemId,
                            i.Title,
                            i.Description,
                            i.Icon,
                            i.IsActive,
                            i.ServiceId
                        })
                        .ToList(),

                    TotalItemCount = x.Items.Count(i => i.IsActive)
                })
                .FirstOrDefault();

            if (value == null)
                return NotFound("Hizmet bulunamadı");

            return Ok(value);
        }

        // ✅ ADMIN: Create service (Items de gönderilebilir ama 4 sınırı var)
        // POST: api/Service
        [HttpPost]
        public IActionResult CreateService([FromBody] Service service)
        {
            if (service == null)
                return BadRequest("Geçersiz veri");

            // ✅ TEK SERVICE KURALI: aktif service varsa ikinci service eklenemez
            var exists = _context.Services.Any(x => x.IsActive);
            if (exists)
                return Conflict("Aktif bir hizmet kaydı zaten var. İkinci hizmet kaydı eklenemez.");

            // ⭐ 4 item kuralı (Create)
            if (service.Items != null && service.Items.Count > MAX_ITEMS)
                return BadRequest($"Bir hizmet için maksimum {MAX_ITEMS} adet item oluşturulabilir.");

            // güvenlik: create'te aktif başlat
            service.IsActive = true;

            // Create güvenliği: gelen itemlar kesin yeni olsun
            if (service.Items != null)
            {
                foreach (var it in service.Items)
                {
                    it.ServiceItemId = 0; // ✅ yeni kayıt garantisi
                    it.IsActive = true;  // istersen bunu kaldırabilirsin
                }
            }

            _context.Services.Add(service);
            _context.SaveChanges();

            return Ok("Hizmet ekleme işlemi başarıyla gerçekleşti");
        }

        // ✅ ADMIN: Update service + items (TOPLU) + 4 item kuralı (aktiflere göre)
        // PUT: api/Service/{id}
        //[HttpPut("{id:int}")]
        //public IActionResult UpdateService(int id, [FromBody] Service service)
        //{
        //    if (service == null)
        //        return BadRequest("Geçersiz veri");

        //    var entity = _context.Services
        //        .Include(x => x.Items)
        //        .FirstOrDefault(x => x.ServiceId == id && x.IsActive);

        //    if (entity == null)
        //        return NotFound("Hizmet bulunamadı");

        //    // Service alanları
        //    entity.Title = service.Title;
        //    entity.Description = service.Description;
        //    entity.ImageUrl1 = service.ImageUrl1;
        //    entity.ImageUrl2 = service.ImageUrl2;
        //    entity.IsActive = service.IsActive;

        //    // Items gelmediyse sadece Service güncelle
        //    if (service.Items == null)
        //    {
        //        _context.SaveChanges();
        //        return Ok("Service güncellendi (Items gönderilmedi)");
        //    }

        //    // ⭐ Update request 4'ten fazla gönderemez (aktif/pasif fark etmez)
        //    if (service.Items.Count > MAX_ITEMS)
        //        return BadRequest($"Bir hizmet için maksimum {MAX_ITEMS} adet item olabilir. Gönderilen: {service.Items.Count}");

        //    var existing = entity.Items.ToDictionary(x => x.ServiceItemId);

        //    foreach (var incoming in service.Items)
        //    {
        //        incoming.ServiceId = id;

        //        if (incoming.ServiceItemId != 0 && existing.TryGetValue(incoming.ServiceItemId, out var dbItem))
        //        {
        //            dbItem.Title = incoming.Title;
        //            dbItem.Description = incoming.Description;
        //            dbItem.Icon = incoming.Icon;
        //            dbItem.IsActive = incoming.IsActive;
        //        }
        //        else
        //        {
        //            if (entity.Items.Count(i => i.IsActive) >= MAX_ITEMS)
        //                return BadRequest($"Bu hizmette zaten {MAX_ITEMS} aktif item var. Yeni item ekleyemezsin.");

        //            entity.Items.Add(new ServiceItem
        //            {
        //                ServiceId = id,
        //                Title = incoming.Title,
        //                Description = incoming.Description,
        //                Icon = incoming.Icon,
        //                IsActive = incoming.IsActive
        //            });
        //        }
        //    }

        //    var incomingIds = service.Items
        //        .Where(x => x.ServiceItemId != 0)
        //        .Select(x => x.ServiceItemId)
        //        .ToHashSet();

        //    foreach (var dbItem in entity.Items)
        //    {
        //        if (dbItem.ServiceItemId != 0 && !incomingIds.Contains(dbItem.ServiceItemId))
        //            dbItem.IsActive = false;
        //    }

        //    _context.SaveChanges();
        //    return Ok("Service + Items güncellendi");
        //}

        // ✅ ADMIN: Update service + items (TOPLU) + 4 item kuralı (aktiflere göre) - DTO ile
        // PUT: api/Service/{id}
        [HttpPut("{id:int}")]
        public IActionResult UpdateService(int id, [FromBody] UpdateServiceDto serviceDto)
        {
            if (serviceDto == null)
                return BadRequest("Geçersiz veri");

            var entity = _context.Services
                .Include(x => x.Items)
                .FirstOrDefault(x => x.ServiceId == id && x.IsActive);

            if (entity == null)
                return NotFound("Hizmet bulunamadı");

            // Service alanlarını güncelle
            entity.Title = serviceDto.Title;
            entity.Description = serviceDto.Description;
            entity.ImageUrl1 = serviceDto.ImageUrl1 ?? string.Empty;
            entity.ImageUrl2 = serviceDto.ImageUrl2 ?? string.Empty;

            // Items gelmediyse sadece Service güncelle
            if (serviceDto.Items == null || !serviceDto.Items.Any())
            {
                _context.SaveChanges();
                return Ok("Service güncellendi (Items gönderilmedi)");
            }

            // ⭐ Update request 4'ten fazla gönderemez
            if (serviceDto.Items.Count > MAX_ITEMS)
                return BadRequest($"Bir hizmet için maksimum {MAX_ITEMS} adet item olabilir. Gönderilen: {serviceDto.Items.Count}");

            // ✅ Mevcut items'ı sil
            _context.ServiceItems.RemoveRange(entity.Items);

            // ✅ Yeni items'ları ekle
            entity.Items = (serviceDto.Items ?? new List<ServiceItemDto>())
                .Where(i => !string.IsNullOrWhiteSpace(i.Title))
                .Select(i => new ServiceItem
                {
                    ServiceId = id,
                    Title = i.Title.Trim(),
                    Description = i.Description?.Trim() ?? string.Empty,
                    Icon = i.Icon?.Trim() ?? string.Empty,
                    IsActive = true
                })
                .ToList();

            _context.SaveChanges();
            return Ok("Service + Items güncellendi");
        }

        // ✅ ADMIN: Soft Delete service + items
        // DELETE: api/Service/{id}
        [HttpDelete("{id:int}")]
        public IActionResult DeleteService(int id)
        {
            var entity = _context.Services
                .Include(x => x.Items)
                .FirstOrDefault(x => x.ServiceId == id && x.IsActive);

            if (entity == null)
                return NotFound("Hizmet bulunamadı");

            entity.IsActive = false;

            foreach (var item in entity.Items)
                item.IsActive = false;

            _context.SaveChanges();
            return Ok("Hizmet yayından kaldırıldı");
        }
    }
}
