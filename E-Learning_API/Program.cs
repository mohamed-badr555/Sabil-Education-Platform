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
using BLL.Managers.AccountManager;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IVideoRepo, VideoRepo>();
builder.Services.AddScoped<ICourseRepo, CourseRepo>();


builder.Services.AddScoped<ICourseUnitRepo, CourseUnitRepo>();
builder.Services.AddScoped<IUnitManager, UnitManager>();

builder.Services.AddScoped<IAccountManager, AccountManager>();

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


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "jwt";
    options.DefaultChallengeScheme = "jwt";
}).AddJwtBearer("jwt", option =>
{
    var SecretKey = builder.Configuration.GetSection("SecretKey").Value;
    var SecretKeyByte = Encoding.UTF8.GetBytes(SecretKey);
    SecurityKey securityKey = new SymmetricSecurityKey(SecretKeyByte);

    option.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
    {
        IssuerSigningKey = securityKey,
        ValidateIssuer = false,
        ValidateAudience = false
    };
});


builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    //options.Password.RequiredLength = 6;
});


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
