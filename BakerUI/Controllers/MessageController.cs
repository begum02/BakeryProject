using BakerUI.Dto.MessageDto;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace BakerUI.Controllers
{
    public class MessageController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public MessageController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // ADMIN LIST
        public async Task<IActionResult> MessageList()
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync("https://localhost:7136/api/Message");

            if (!response.IsSuccessStatusCode)
                return View(new List<ResultMessageDto>());

            var jsonData = await response.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<ResultMessageDto>>(jsonData);

            return View(values ?? new List<ResultMessageDto>());
        }

        // MESSAGE DETAIL
        [HttpGet]
        public async Task<IActionResult> MessageDetail(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"https://localhost:7136/api/Message/{id}");

            if (!response.IsSuccessStatusCode)
                return RedirectToAction("MessageList");

            var jsonData = await response.Content.ReadAsStringAsync();
            var message = JsonConvert.DeserializeObject<ResultMessageDto>(jsonData);

            if (message == null)
                return RedirectToAction("MessageList");

            // Mesajı okundu olarak işaretle
            if (!message.IsRead)
            {
                var updateDto = new UpdateMessageDto
                {
                    MessageId = message.MessageId,
                
                    IsRead = true
                };

                var updateJson = JsonConvert.SerializeObject(updateDto);
                var updateContent = new StringContent(updateJson, Encoding.UTF8, "application/json");
                await client.PutAsync($"https://localhost:7136/api/Message/{id}", updateContent);

                message.IsRead = true;
            }

            return View(message);
        }

        // UPDATE MESSAGE
        [HttpGet]
        public async Task<IActionResult> UpdateMessage(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"https://localhost:7136/api/Message/{id}");

            if (!response.IsSuccessStatusCode)
                return RedirectToAction("MessageList");

            var jsonData = await response.Content.ReadAsStringAsync();
            var message = JsonConvert.DeserializeObject<ResultMessageDto>(jsonData);

            if (message == null)
                return RedirectToAction("MessageList");

            var updateDto = new UpdateMessageDto
            {
                MessageId = message.MessageId,
      
                IsRead = message.IsRead
            };

            return View(updateDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateMessage(UpdateMessageDto model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var client = _httpClientFactory.CreateClient();

            var jsonData = JsonConvert.SerializeObject(model);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"https://localhost:7136/api/Message/{model.MessageId}", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Mesaj başarıyla güncellendi!";
                return RedirectToAction("MessageList");
            }

            TempData["ErrorMessage"] = "Mesaj güncellenemedi. Lütfen tekrar deneyin.";
            return View(model);
        }

        // DELETE
        public async Task<IActionResult> DeleteMessage(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.DeleteAsync($"https://localhost:7136/api/Message/{id}");

            if (response.IsSuccessStatusCode)
                return RedirectToAction("MessageList");

            return RedirectToAction("MessageList");
        }

        // CREATE MESSAGE (Public - İletişim Formu)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateMessage(CreateMessageDto model)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Index", "Default");

            var client = _httpClientFactory.CreateClient();

            var jsonData = JsonConvert.SerializeObject(model);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("https://localhost:7136/api/Message", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Mesajınız başarıyla gönderildi!";
                return RedirectToAction("Index", "Default");
            }

            TempData["ErrorMessage"] = "Mesaj gönderilemedi. Lütfen tekrar deneyin.";
            return RedirectToAction("Index", "Default");
        }
    }
}
