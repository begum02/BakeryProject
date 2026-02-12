namespace BakerUI.Dto.AboutDto
{
    public class CreateAboutDto
    {
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string ImageUrl1 { get; set; } = null!;
        public string ImageUrl2 { get; set; } = null!;
        public List<string> AboutItems { get; set; } = new(); // sadece text

    }
}
