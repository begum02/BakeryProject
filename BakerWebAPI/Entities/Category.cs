namespace BakerWebAPI.Entities
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;
        public string CategoryDescription { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public string? PriceRange { get; set; }
        public bool IsActive { get; set; } = true;
        public ICollection<Product> Products { get; set; } = new List<Product>();

        // ? Gallery iliþkisinin ters tarafý
        public ICollection<Gallery> Galleries { get; set; } = new List<Gallery>();
    }
}