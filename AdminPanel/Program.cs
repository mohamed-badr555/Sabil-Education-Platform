using BLL.Managers.CategoryManager;
using BLL.Managers.CourseManager;
using DAL.Data.Models;
using DAL.DB_Context;
using DAL.Repositories;
using DAL.Repositories.CourseRepo;
using DAL.Repositories.VideoRepo;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AdminPanel
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            // Add services to the container. 
            //.AddRazorRuntimeCompilation()
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<E_LearningDB>(option =>
            {
                option.UseSqlServer(builder.Configuration.GetConnectionString("EDB"));
            });

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                         .AddEntityFrameworkStores<E_LearningDB>();

            // Register repository services
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            // Register your manager services
            builder.Services.AddScoped<ICategoryManager, CategoryManager>();
            builder.Services.AddScoped<ICourseManager, CourseManager>();
            builder.Services.AddScoped<IVideoRepo, VideoRepo>();
            builder.Services.AddScoped<ICourseRepo, CourseRepo>();

            // Register AutoMapper if you're using it (assuming you have a MappingProfile class)
            builder.Services.AddAutoMapper(typeof(BLL.MappingProfiles.MappingProfile).Assembly);

            var app = builder.Build();

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

            app.Run();
        }
    }
}