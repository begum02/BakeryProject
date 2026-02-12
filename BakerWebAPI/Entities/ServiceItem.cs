namespace BakerWebAPI.Entities
{
    public class ServiceItem
    {
                public int ServiceItemId { get; set; }

    public string Title { get; set; } = null!;        // "Quality Products"
    public string Description { get; set; } = null!;  // alt açıklama
    public string? Icon { get; set; }                 // "fa fa-bread-slice"

    public bool IsActive { get; set; } = true;

    // FK
    public int ServiceId { get; set; }
    public Service Services { get; set; } = null!;
    
    }
}
