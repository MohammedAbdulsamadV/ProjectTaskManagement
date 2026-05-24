using ProjectTask.Application.Interfaces;
using ProjectTask.Domain.Entities;
using ProjectTask.Infrastructure.Persistence;

namespace ProjectTask.Infrastructure.Repositories;

public class ProjectRepository : Repository<Project> , IProjectRepository
{
    public ProjectRepository(AppDbContext context) : base(context)
    {
    }
    
}