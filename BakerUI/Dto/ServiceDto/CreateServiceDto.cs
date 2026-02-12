namespace BakerUI.Dto.ServiceDto
{
    public class CreateServiceDto
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;

        public string? ImageUrl1 { get; set; }
        public string? ImageUrl2 { get; set; }

        public List<ServiceItemDto> Items { get; set; } = new();

    }
}
