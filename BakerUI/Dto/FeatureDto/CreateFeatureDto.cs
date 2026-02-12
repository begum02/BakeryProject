namespace BakerUI.Dto.FeatureDto
{
    public class CreateFeatureDto
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string? ImageUrl { get; set; } // fa icon, svg class vs.
    }
}
