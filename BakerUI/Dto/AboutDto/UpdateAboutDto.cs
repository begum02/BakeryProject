namespace BakerUI.Dto.AboutDto
{
    public class UpdateAboutDto
    {
        public int AboutId { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
      
        public List<AboutItemDto>   AboutItems { get; set; } = new();
        public string ImageUrl1 { get;set; }
        public string ImageUrl2 { get;set; }
    }
}
