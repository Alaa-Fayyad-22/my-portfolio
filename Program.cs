using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using MyPortfolio.Data;

namespace MyPortfolio
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddScoped<EmailService>();

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

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            // Add essential security headers
            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
                context.Response.Headers.Add("X-Frame-Options", "DENY");
                context.Response.Headers.Add("X-XSS-Protection", "1; mode=block");
                await next();
            });

            // Add improved CSP that allows external fonts, scripts, styles and Lottie files
            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("Content-Security-Policy",
                    "default-src 'self'; " +
                    "script-src 'self' 'unsafe-inline' https://unpkg.com https://cdn.jsdelivr.net https://*.lottiefiles.com; " +
                    "style-src 'self' 'unsafe-inline' https://fonts.googleapis.com https://cdn.jsdelivr.net; " +
                    "font-src 'self' https://fonts.gstatic.com https://cdn.jsdelivr.net; " + // <-- added cdn.jsdelivr.net here
                    "img-src 'self' data: https://*.lottiefiles.com https://lottie.host; " +
                    "frame-src 'self' https://*.lottiefiles.com https://lottie.host; " +
                    "connect-src 'self' https://*.lottiefiles.com https://lottie.host;");
                await next();
            });

            app.MapGet("/health", () => Results.Ok("Healthy!"));



            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = ctx =>
                {
                    var ext = Path.GetExtension(ctx.File.Name);
                    if (ext == ".cshtml" || ext == ".config")
                    {
                        ctx.Context.Response.StatusCode = 403;
                        ctx.Context.Response.ContentLength = 0;
                        ctx.Context.Response.Body = Stream.Null;
                    }
                }
            });

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
                    //if (!context.Projects.Any())
                    //{
                    //    context.Projects.AddRange(
                    //        new MyPortfolio.Models.Project
                    //        {
                    //            Title = "Portfolio Website",
                    //            Description = "ASP.NET Core MVC portfolio using MySQL and Bootstrap",
                    //            ImageUrl = "/images/project1.jpg"
                    //        },
                    //        new MyPortfolio.Models.Project
                    //        {
                    //            Title = "E-commerce Perfume Shop",
                    //            Description = "Built with Razor Pages and Stripe integration",
                    //            ImageUrl = "/images/project2.jpg"
                    //        }
                    //    );

                    //    context.SaveChanges();
                    //}
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error seeding database: {ex.Message}");
                }
            }

            Console.WriteLine("DefaultConnection: " + Environment.GetEnvironmentVariable("DefaultConnection"));


            app.Run();
        }
    }
}
