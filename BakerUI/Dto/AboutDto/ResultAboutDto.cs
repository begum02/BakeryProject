namespace BakerUI.Dto.AboutDto
{
    public class ResultAboutDto
    {
        public int AboutId { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string ImageUrl1 { get; set; } = null!;
        public string ImageUrl2 { get; set; } = null!;
        public List<AboutItemDto> AboutItems { get; set; } = new();
    }
}
