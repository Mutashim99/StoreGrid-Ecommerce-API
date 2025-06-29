global using EcommerceAPI.DTOs;
global using EcommerceAPI.Data;
global using EcommerceAPI.Controllers;
global using EcommerceAPI.Services;
using EcommerceAPI.Mappings;
using EcommerceAPI.Services.Auth;
using EcommerceAPI.Services.Email;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;
using EcommerceAPI.Services.UserManagement.UserProfileManagement;
using Microsoft.OpenApi.Models;
using EcommerceAPI.Services.UserManagement.UserAddressManagement;
using EcommerceAPI.Services.UserManagement.UserFavoriteManagement;
using EcommerceAPI.Services.UserManagement.UserReviewManagement;
using EcommerceAPI.Services.UserManagement.UserCartManagement;
using EcommerceAPI.Services.UserManagement.UserOrderManagement;
using EcommerceAPI.Services.Product;
using EcommerceAPI.Services.AdminManagement.AdminAuth;

using BCrypt.Net;
using EcommerceAPI.Services.AdminManagement.AdminCategory;
using EcommerceAPI.Services.AdminManagement.AdminOrder;
using EcommerceAPI.Services.AdminManagement.AdminProduct;
using EcommerceAPI.Services.AdminManagement.AdminReview;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultString")));
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]))
        };
    });
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "EcommerceAPI", Version = "v1" });


    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' followed by your JWT token. Example: Bearer eyJhbGciOi..."
    });

 
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});
builder.Services.AddAuthorization();
builder.Services.AddScoped<IAuth,AuthService>();
builder.Services.AddScoped<IEmail, EmailService>();
builder.Services.AddScoped<IUserProfile,UserProfileService>();
builder.Services.AddScoped<IUserAddress,UserAddressService>();
builder.Services.AddScoped<IUserFavorite, UserFavoriteService>();
builder.Services.AddScoped<IUserReview,UserReviewService>();
builder.Services.AddScoped<IUserCart, UserCartService>();
builder.Services.AddScoped<IUserOrder,UserOrderService>();
builder.Services.AddScoped<IProduct, ProductService>();
builder.Services.AddScoped<IAdminAuth, AdminAuthService>();
builder.Services.AddScoped<IAdminCategory, AdminCategoryService>();
builder.Services.AddScoped<IAdminOrder, AdminOrderService>();
builder.Services.AddScoped<IAdminProduct, AdminProductService>();
builder.Services.AddScoped<IAdminReview, AdminReviewService>();
builder.Services.AddAutoMapper(typeof(Program));


string plainPassword = "SuperAdminIsVeryCool";
string hashedPassword = BCrypt.Net.BCrypt.HashPassword(plainPassword);
Console.WriteLine(hashedPassword);


var app = builder.Build();

app.MapGet("/", () => "StoreGrid API is running successfully on Azure!");




// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "StoreGrid API V1");
    c.RoutePrefix = "swagger";
});


app.UseHttpsRedirection();
app.UseAuthentication(); 
app.UseAuthorization();

app.MapControllers();

app.Run();
