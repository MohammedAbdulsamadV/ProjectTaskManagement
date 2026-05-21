using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProjectTask.Application.Interfaces;
using ProjectTask.Infrastructure.Caching;
using ProjectTask.Infrastructure.Identity;
using ProjectTask.Infrastructure.Persistence;
using ProjectTask.Infrastructure.Repositories;
using StackExchange.Redis;
using ICacheService = ProjectTask.Application.Interfaces.ICacheService;

namespace ProjectTask.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddHttpContextAccessor();

        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<IJwtService, JwtService>();
        services.AddSingleton<IConnectionMultiplexer>(sp =>
        {
            var config = sp.GetRequiredService<IConfiguration>();

            var options = ConfigurationOptions.Parse(config["Redis:Connection"]);

            options.ConnectRetry = 5;
            options.ConnectTimeout = 10000;
            options.AbortOnConnectFail = false;

            return ConnectionMultiplexer.Connect(options);
        });
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration["Redis:Connection"];
        });

        services.AddScoped<ICacheService, RedisCacheService>();

        return services;
    }
}