namespace BakerUI.Dto.FeatureDto
{
    public class UpdateFeatureDto
    {
    public int FeatureId { get; set; }
    public string Title { get; set; } = null!;

    public string SubTitle { get; set; } = null!;   
        public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsActive { get; set; }
    
    }
}
