using Microsoft.EntityFrameworkCore;
using MyPortfolio.Models; // make sure this matches your project namespace

namespace MyPortfolio.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Project> Projects { get; set; }
        public DbSet<ContactInfo> ContactInfos { get; set; }
        public DbSet<AboutUs> AboutUs { get; set; }

    }
}
