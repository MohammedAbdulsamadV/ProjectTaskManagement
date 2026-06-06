using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using ProjectTask.Infrastructure.Persistence;

namespace ProjectTask.Infrastructure.Persistence;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

        optionsBuilder.UseSqlServer(
            "Server=localhost,1433;Database=ProjectTaskDb;User Id=sa;Password=Your_password123;TrustServerCertificate=True");

        return new AppDbContext(optionsBuilder.Options);
    }
}