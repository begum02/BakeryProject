namespace BakerUI.Dto.GalleryDto
{
    public class UpdateGalleryDto
    {
              public int GalleryId { get; set; }
    public string ImageUrl { get; set; } = null!;
    public string? Title { get; set; }
     public int? CategoryId { get; set; }


    }
}
