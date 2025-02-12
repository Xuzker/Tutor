using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Tutor.Data;
using Tutor.Initialize;
using Tutor.Models;
using Tutor.Email;
using Tutor.VisitService;

namespace Tutor
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            builder.Services.AddRazorPages();

            builder.Services.AddScoped<VisitLogService>();

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
            builder.Services.AddTransient<Tutor.Email.IEmailSender, EmailSender>();

            var app = builder.Build();

            app.Use(async (context, next) =>
            {
                using (var scope = app.Services.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    var visit = new VisitLog
                    {
                        VisitDate = DateTime.UtcNow,
                        IpAddress = context.Connection.RemoteIpAddress?.ToString() ?? "Unknown"
                    };
                    dbContext.VisitLogs.Add(visit);
                    await dbContext.SaveChangesAsync();
                }
                await next();
            });

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapRazorPages();

            //InitializeAdmin.Initialize(app.Services).Wait();

            app.Run();
        }
    }
}
