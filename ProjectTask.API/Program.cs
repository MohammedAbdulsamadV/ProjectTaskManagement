using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProjectTask.Application;
using ProjectTask.Infrastructure;
using ProjectTask.Infrastructure.Persistence;

namespace ProjectTask.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

    var configuration = builder.Configuration;

// ========================
// 📌 Controllers
// ========================
    builder.Services.AddControllers();

// ========================
// 📌 Swagger
// ========================
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

// ========================
// 📌 Application Layer DI
// ========================
    builder.Services.AddApplication();

// ========================
// 📌 Infrastructure Layer DI (EF + Redis + Repos)
// ========================
    builder.Services.AddInfrastructure(configuration);

// ========================
// 📌 Database (if not inside Infrastructure)
// ========================
        builder.Services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(
                builder.Configuration.GetConnectionString("DefaultConnection"),
                sql =>
                {
                    sql.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(10),
                        errorNumbersToAdd: null);
                });
        });
// ========================
// 📌 JWT Authentication
// ========================
    var jwtSettings = configuration.GetSection("Jwt");

    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings["Key"]!)
            )
        };
    });

// ========================
// 📌 Authorization
// ========================
builder.Services.AddAuthorization();

// ========================
// 📌 Redis (if not inside Infrastructure)
// ========================
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = configuration["Redis:Connection"];
});

var app = builder.Build();

// ========================
// 📌 Middleware Pipeline
// ========================

// Global Exception Handling
app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
    }
}