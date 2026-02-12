namespace BakerUI.Dto.CategoryDto
{
    public class UpdateCategoryDto
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;
   
        public string? ImageUrl { get; set; }

        public string? CategoryDescription { get; set; }
        public string? PriceRange { get; set; }
        public bool IsActive { get; set; }
    }
}
