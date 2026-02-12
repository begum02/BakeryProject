using System;

namespace BakerWebAPI.Entities
{
    public class About
    {
        public int AboutId { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string ImageUrl1 { get; set; } = null!;

        public string ImageUrl2 { get; set; } = null!;


        public List<AboutItem> AboutItems { get; set; } = new();
        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
