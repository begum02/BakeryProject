namespace BakerWebAPI.Entities
{
    public class Testimonial
    {
        public int TestimonialId { get; set; }

        public string NameSurname { get; set; } = null!;
        public string Title { get; set; } = null!;          // örn: "Müşteri", "Food Blogger"
        public string Comment { get; set; } = null!;
        public string? ImageUrl { get; set; }               // opsiyonel profil foto


        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
