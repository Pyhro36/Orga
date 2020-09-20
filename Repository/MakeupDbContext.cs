using Microsoft.EntityFrameworkCore;
using Orga.Models;

namespace Orga.Repository
{
    public class MakeupDbContext : DbContext
    {
        public MakeupDbContext(DbContextOptions<MakeupDbContext> options) : base(options) { }

        public DbSet<Article> Articles { get; set; }

        public DbSet<Brand> Brands { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<ImageData> ImageDatas { get; set; }
    }
}