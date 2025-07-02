using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Stripe;
using WebApplication1.Data;
using WebApplication1.Models;
using Product = WebApplication1.Models.Product;

namespace WebApplication1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));
            StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];

            // تسجيل الخدمات
            builder.Services.AddMemoryCache();
            builder.Services.AddSession();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<OrderService>();
            builder.Services.AddScoped<ShoppingCartService>();
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddControllersWithViews();

            builder.Services.AddIdentity<IdentityUser, IdentityRole>()
               .AddEntityFrameworkStores<ApplicationDbContext>()
               .AddDefaultTokenProviders();

            var app = builder.Build(); // 🟢 لازم نعمل Build الأول

            // 🟢 هنا نضيف المنتجات
            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                if (!context.Products.Any())
                {
                    context.Products.AddRange(
                      
    new Product { Name = "iPhone 14", img = "iphone14.jpg", Price = 25000, CategoryId = 1 },
    new Product { Name = "Samsung S23", img = "samsungS23.jpg", Price = 23000, CategoryId = 1 },
    new Product { Name = "HP Laptop", img = "hp.jpg", Price = 18000, CategoryId = 2 },
    new Product { Name = "iPad Air", img = "ipadair.jpg", Price = 15000, CategoryId = 1 },
    new Product { Name = "Realme C53", img = "realme.jpg", Price = 5000, CategoryId = 1 }

                    ); if (!context.Categories.Any())
                    {
                        context.Categories.AddRange(
                            new Category { Name = "Phones" },   // ID = 1
                            new Category { Name = "Laptops" }   // ID = 2
                        );
                        context.SaveChanges();
                    }


                    context.SaveChanges();
                }
            }

            // إعداد خط أنابيب الطلب
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseSession();
            app.UseAuthentication(); // مهم لو فيه تسجيل دخول
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
