using BakerWebAPI.Controllers;
using BakerWebAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace BakerWebAPI.Context
{
    public class BakerContext : DbContext
    {
        public BakerContext(DbContextOptions<BakerContext> options)
            : base(options)
        {
        }

        public DbSet<About> Abouts { get; set; }

        public DbSet<AboutItem> AboutItems { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<ServiceItem> ServiceItems { get; set; }
        public DbSet<Gallery> Galleries { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Testimonial> Testimonials { get; set; }

        public DbSet<Subscribe> Subscribes { get; set; }
        public DbSet<Chef> Chefs { get; set; }
        public DbSet<Feature> Features { get; set; }
       public DbSet<Category> Categories { get; set; }
         public DbSet<Product> Products { get; set; }

    }
}
