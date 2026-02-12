namespace BakerUI.Dto.ServiceDto
{
    public class ServiceItemDto
    {
        public int ServiceItemId { get; set; }   // Update/List için
        public int ServiceId { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Icon { get; set; } = null!; // fa fa-truck vb.
    }
}
