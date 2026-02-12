namespace BakerUI.Dto.CategoryDto
{
    public class CreateCategoryDto
    {
        public string CategoryName { get; set; } = null!;
        public string CategoryDescription { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public string? PriceRange { get; set; }
    }
}
