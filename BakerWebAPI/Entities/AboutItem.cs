using System.Text.Json.Serialization;

namespace BakerWebAPI.Entities
{
    public class AboutItem
    {
            public int AboutItemId { get; set; }
    public string Text { get; set; } = null!; // "Quality Products"

    public bool IsActive { get; set; } = true;

    // FK
    public int AboutId { get; set; }
        
    [JsonIgnore] // ✅ cycle kırılır
    public About About { get; set; } = null!;
    
    }
}
