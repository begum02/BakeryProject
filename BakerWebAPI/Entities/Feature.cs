namespace BakerWebAPI.Entities
{
    public class Feature
    {
        public  int  FeatureId { get; set; }
        public  string  Title { get; set; }

        public string Subtitle { get; set; }

        public  string Description { get; set; }

        public string ImageUrl { get; set; }

           public bool IsActive { get; set; } = true;



    }
}
