namespace BakerWebAPI.Entities
{
    public class Chef
    {
        public  int  ChefId { get; set; }
        public string Name { get; set; }

        public string Title { get; set; }

        public string ImageUrl { get; set; }

        public bool IsActive { get; set; } = true;


    }
}
