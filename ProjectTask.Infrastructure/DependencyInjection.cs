using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProjectTask.Application.Interfaces;
using ProjectTask.Infrastructure.Caching;
using ProjectTask.Infrastructure.Identity;
using ProjectTask.Infrastructure.Persistence;
using ProjectTask.Infrastructure.Repositories;
using ICacheService = ProjectTask.Application.Interfaces.ICacheService;

namespace ProjectTask.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // DbContext
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        // HttpContext
        services.AddHttpContextAccessor();

        // Repositories
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Services
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<IJwtService, JwtService>();
        
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration["Redis:Connection"];
        });

        services.AddScoped<ICacheService, RedisCacheService>();

        return services;
    }
}