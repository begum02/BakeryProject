namespace BakerUI.Dto.FeatureDto
{
    public class ResultFeatureDto
    {
        public int FeatureId { get; set; }
        public string Title { get; set; } = null!;

        public string ? SubTitle { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsActive { get; set; }
    }
}
