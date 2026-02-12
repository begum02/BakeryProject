namespace BakerUI.Dto.TestimonialDto
{
    public class CreateTestimonialDto
    {
        public string NameSurname { get; set; } = null!;
        public string  Title { get; set; } = null!;
        public string Comment { get; set; } = null!;
        public string? ImageUrl { get; set; }

    }
}
