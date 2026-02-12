namespace BakerUI.Dto.CategoryDto
{
    public class ResultCategoryDto
    {
        public int CategoryId { get; set; }

        public string CategoryName { get; set; } = null!;

        public string? ImageUrl { get; set; }   // ✅
        public string? PriceRange { get; set; }   // "$11 - $99"

        public string? CategoryDescription { get; set; }
        public bool IsActive { get; set; }
    }
}
