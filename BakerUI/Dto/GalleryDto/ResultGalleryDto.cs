namespace BakerUI.Dto.GalleryDto
{
    public class ResultGalleryDto
    {
     public int GalleryId { get; set; }
    public string ImageUrl { get; set; } = null!;
        public int? CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string? Title { get; set; }

    
    }
}
