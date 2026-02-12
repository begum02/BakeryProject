namespace BakerUI.Dto.ProductDto
{
    public class CreateProductDto
    {
        public string ProductName { get; set; } = null!;
        public decimal Price { get; set; }
        public string ImageUrl { get; set; } = null!;
        public string? Description { get; set; }
        public int CategoryId { get; set; }
    }
}