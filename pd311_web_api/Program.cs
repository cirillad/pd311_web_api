using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using pd311_web_api.BLL;
using pd311_web_api.BLL.DTOs.Account;
using pd311_web_api.BLL.Services;
using pd311_web_api.BLL.Services.Account;
using pd311_web_api.BLL.Services.Email;
using pd311_web_api.BLL.Services.Role;
using pd311_web_api.BLL.Services.User;
using pd311_web_api.BLL.Services.Image; // Ensure this is added for ImageService
using pd311_web_api.DAL;
using pd311_web_api.DAL.Repositories.Manufactures;
using pd311_web_api.DAL.Repositories.Cars;
using static pd311_web_api.DAL.Entities.IdentityEntities;
using pd311_web_api.BLL.MapperProfiles;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            RequireExpirationTime = true,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"] ?? ""))
        };
    });

// Add services to the container.
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.AddScoped<IManufactureRepository, ManufactureRepository>();
builder.Services.AddScoped<IManufactureService, ManufactureService>();

// Add services for Car and CarImage
builder.Services.AddScoped<ICarRepository, CarRepository>();
builder.Services.AddScoped<ICarService, CarService>();

// Add Image Service (this is the missing part)
builder.Services.AddScoped<IImageService, ImageService>();

builder.Services.AddControllers();

// Add fluent validation
builder.Services.AddValidatorsFromAssemblyContaining<LoginValidator>();

// Add automapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Add database context
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql("name=NpgsqlLocal");
});

// Add identity
builder.Services
    .AddIdentity<AppUser, AppRole>(options =>
    {
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredUniqueChars = 0;
        options.Password.RequireDigit = false;
        options.Password.RequiredLength = 6;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;
        options.User.RequireUniqueEmail = true;
    })
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// Add Swagger
builder.Services.AddSwaggerGen();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("localhost3000", builder =>
    {
        builder.WithOrigins("http://localhost:3000")
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    await DbSeeder.SeedAsync(scope.ServiceProvider);
}

// Middleware для логування
app.UseMiddleware<RequestLoggingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

Settings.RootPath = builder.Environment.ContentRootPath;
string rootPath = Path.Combine(builder.Environment.ContentRootPath, "wwwroot");
string imagesPath = Path.Combine(rootPath, Settings.RootImagesPath);

if (!Directory.Exists(rootPath))
{
    Directory.CreateDirectory(rootPath);
}

if (!Directory.Exists(imagesPath))
{
    Directory.CreateDirectory(imagesPath);
}

// static files
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(imagesPath),
    RequestPath = "/images"
});

app.UseCors("localhost3000");

app.UseAuthorization();

app.MapControllers();

app.Run();
