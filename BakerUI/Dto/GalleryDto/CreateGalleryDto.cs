namespace BakerUI.Dto.GalleryDto
{
    public class CreateGalleryDto
    {
    public string ImageUrl { get; set; } = null!;
    public string? Title { get; set; }
    public int? CategoryId { get; set; }


    }
}
