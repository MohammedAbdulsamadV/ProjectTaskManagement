using Microsoft.EntityFrameworkCore;
using ProjectTask.Application.Interfaces;
using ProjectTask.Infrastructure.Persistence;
using Task = ProjectTask.Domain.Entities.Task;

namespace ProjectTask.Infrastructure.Repositories;

public class TaskRepository : Repository<Domain.Entities.Task>, ITaskRepository
{
    private readonly AppDbContext _context;

    public TaskRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Domain.Entities.Task?> GetTaskWithProjectAsync(int id)
    {
        return await _context.Tasks
            .Include(t => t.Project)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<List<Domain.Entities.Task>> GetTasksByProjectIdAsync(int projectId)
    {
        return await _context.Tasks
            .Where(t => t.ProjectId == projectId)
            .ToListAsync();
    }
}