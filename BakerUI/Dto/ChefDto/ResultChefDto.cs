namespace BakerUI.Dto.ChefDto
{
    public class ResultChefDto
    {
        public int ChefId { get; set; }
        public string Name { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
        public bool IsActive { get; set; }
    }
}
