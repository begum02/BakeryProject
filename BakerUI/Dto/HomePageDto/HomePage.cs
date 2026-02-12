using BakerUI.Dto.AboutDto;
using BakerUI.Dto.CategoryDto;
using BakerUI.Dto.ChefDto;
using BakerUI.Dto.FeatureDto;
using BakerUI.Dto.HomeStatsDto;
using BakerUI.Dto.ServiceDto;
using BakerUI.Dto.TestimonialDto;


namespace BakerUI.Dto.HomePageDto
{
    public class HomePage
    {
        public ResultFeatureDto Hero { get; set; } = null!;
        public HomeStats Stats { get; set; } = null!;
        public ResultAboutDto About { get; set; } = null!;

        public ResultCategoryDto Categories { get; set; } = null!;

        public ResultServiceDto Services { get; set; } = null!;

        public ResultChefDto Chefs { get; set; } = null!;

        public ResultTestimonialDto Testimonials { get; set; } = null!;






    }
}
