namespace BakerWebAPI.Entities
{
    public class Service
    {
        public int ServiceId { get; set; }

        public string Title { get; set; } = null!;        // "What Do We Offer For You?"
        public string Description { get; set; } = null!;  // üst açıklama

        public string? ImageUrl1 { get; set; }            // sağdaki 1. görsel
        public string? ImageUrl2 { get; set; }            // sağdaki 2. görsel

        public bool IsActive { get; set; } = true;

        public List<ServiceItem> Items { get; set; } = new();
    }
}