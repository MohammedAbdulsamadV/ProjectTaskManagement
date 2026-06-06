namespace ProjectTask.Application.Interfaces;

public interface ITaskRepository : IRepository<Domain.Entities.Task>
{
    Task<Domain.Entities.Task?> GetTaskWithProjectAsync(int id);
    Task<List<Domain.Entities.Task>> GetTasksByProjectIdAsync(int projectId);}