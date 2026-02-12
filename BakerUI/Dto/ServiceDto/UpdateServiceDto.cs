namespace BakerUI.Dto.ServiceDto
{
    public class UpdateServiceDto


    {
        public int ServiceId { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;

        public string? ImageUrl1 { get; set; }
        public string? ImageUrl2 { get; set; }

        public bool IsActive { get; set; }

        public List<ServiceItemDto> Items { get; set; } = new();
    }
}
