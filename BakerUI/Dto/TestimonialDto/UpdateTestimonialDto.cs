namespace BakerUI.Dto.TestimonialDto
{
    public class UpdateTestimonialDto

    {
        public int TestimonialId { get; set; }
        public string NameSurname { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Comment { get; set; } = null!;
        public string? ImageUrl { get; set; }
      
        public bool IsActive { get; set; }
    }
}
