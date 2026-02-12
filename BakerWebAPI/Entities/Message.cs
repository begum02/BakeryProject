namespace BakerWebAPI.Entities
{
    public class Message
    {
        public int MessageId { get; set; }
        public string NameSurname { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Subject { get; set; } = null!;
        public string MessageDetail { get; set; } = null!;
        public DateTime SendDate { get; set; }
        public bool IsRead { get; set; } = false;
        public bool IsActive { get; set; } = true;
    }
}