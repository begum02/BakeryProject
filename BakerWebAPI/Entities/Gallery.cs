namespace BakerWebAPI.Entities
{
    public class Gallery
    {
        public int GalleryId { get; set; }
        public string ImageUrl { get; set; } = null!;
        public string? Title { get; set; }
        public int? CategoryId { get; set; }
        public Category? Category { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
