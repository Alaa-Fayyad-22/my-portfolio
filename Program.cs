using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using MyPortfolio.Data; // Your namespace for ApplicationDbContext

namespace MyPortfolio
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddScoped<EmailService>();


            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                Console.WriteLine("ERROR: DefaultConnection string is missing.");
            }
            else
            {
                Console.WriteLine("DefaultConnection loaded.");
            }

            // **Register your DbContext here**
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(connectionString));



            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<ApplicationDbContext>();
                    context.Database.EnsureCreated();

                    // Seed ContactInfo if table is empty
                    if (!context.ContactInfos.Any())
                    {
                        context.ContactInfos.Add(new MyPortfolio.Models.ContactInfo
                        {
                            Email = "alaa@example.com",
                            Phone = "+961 71 234 567",
                            Address = "Beirut, Lebanon",
                            LinkedIn = "https://linkedin.com/in/alaafayyad",
                            GitHub = "https://github.com/alaafayyad"
                        });

                        context.SaveChanges();
                    }

                    // Seed Projects if table is empty
                    if (!context.Projects.Any())
                    {
                        context.Projects.AddRange(
                            new MyPortfolio.Models.Project
                            {
                                Title = "Portfolio Website",
                                Description = "ASP.NET Core MVC portfolio using MySQL and Bootstrap",
                                ImageUrl = "/images/project1.jpg"
                            },
                            new MyPortfolio.Models.Project
                            {
                                Title = "E-commerce Perfume Shop",
                                Description = "Built with Razor Pages and Stripe integration",
                                ImageUrl = "/images/project2.jpg"
                            }
                        );

                        context.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error seeding database: {ex.Message}");
                }
            }


            app.Run();
        }
    }
}
