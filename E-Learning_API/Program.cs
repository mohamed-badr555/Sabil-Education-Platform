// In E-Learning_API/Program.cs
using BLL.MappingProfiles;
using DAL.Data.Models;
using DAL.Repositories;
using DAL.Repositories.BasketRepo;
using DAL.DB_Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Text;
using BLL.Managers.CourseManager;
using System.Text.Json.Serialization;
using DAL.Repositories.VideoRepo;
using DAL.Repositories.CourseRepo;
using E_Learning_API.Middlewares;

using StackExchange.Redis;
using BLL.Managers.BasketManager;
using E_Learning_API.Middlewares;
using E_Learning_API.Extensions;
using BLL.Managers.CategoryManager;
using BLL.Managers.UnitManager;
using DAL.Repositories.CourseUnitRepo;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IVideoRepo, VideoRepo>();
builder.Services.AddScoped<ICourseRepo, CourseRepo>();


builder.Services.AddScoped<ICourseUnitRepo, CourseUnitRepo>();
builder.Services.AddScoped<IUnitManager, UnitManager>();

// Add application services including error handling
builder.Services.AddApplicationServices();

// Register Redis connection (singleton)
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var configuration = builder.Configuration.GetConnectionString("Redis");
    return ConnectionMultiplexer.Connect(configuration);
});

// Register basket repository
builder.Services.AddScoped(typeof(IBasketRepository),typeof(BasketRepository));
builder.Services.AddScoped<IBasketManager, BasketManager>();
builder.Services.AddScoped<ICourseManager, CourseManager>();
//builder.Services.AddScoped<ICourseManager,CourseManager>();



builder.Services.AddMemoryCache();



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

// Add exception middleware at the beginning of the pipeline
app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionMiddleware>();

app.UseAuthorization();

app.MapControllers();


app.Run();
