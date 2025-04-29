using BLL.MappingProfiles;
using DAL.Data.Models;
using DAL.Repositories;
using DAL.DB_Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Text;
using BLL.Managers.CourseManager;
using BLL.Managers.EnrollmentManager;





var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(CourseMappingProfile).Assembly);
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));


builder.Services.AddScoped<ICourseManager,CourseManager>();
builder.Services.AddScoped<IEnrollmentManager, EnrollmentManager>();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<E_LearningDB>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("EDB"));
});
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
             .AddEntityFrameworkStores<E_LearningDB>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
