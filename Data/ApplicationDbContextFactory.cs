using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MyPortfolio.Data
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

            var connectionString = "Host=dpg-d1mgrlidbo4c73fa3b60-a.oregon-postgres.render.com;Port=5432;Database=portfolio_db_hq2z;Username=root;Password=Od009dlPoyRGsdMaBmvKbfonq4PgkFia;";

            optionsBuilder.UseNpgsql(connectionString);

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
