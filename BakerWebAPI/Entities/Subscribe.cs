namespace BakerWebAPI.Entities
{
    public class Subscribe
    {
        public int SubscribeId { get; set; }
        public string Email { get; set; } = null!;

        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
