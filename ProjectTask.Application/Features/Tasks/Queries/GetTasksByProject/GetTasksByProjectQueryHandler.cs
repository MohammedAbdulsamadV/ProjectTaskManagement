using AutoMapper;
using MediatR;
using ProjectTask.Application.Common.Models;
using ProjectTask.Application.DTOs;
using ProjectTask.Application.Interfaces;
using ProjectTask.Domain.Entities;
using Task = ProjectTask.Domain.Entities.Task;

namespace ProjectTask.Application.Features.Tasks.Queries.GetTasksByProject;

public class GetTasksByProjectQueryHandler
    : IRequestHandler<GetTasksByProjectQuery, ApiResponse<List<TaskDto>>>
{
    private readonly IRepository<Task> _taskRepo;
    private readonly IRepository<Project> _projectRepo;
    private readonly ICurrentUserService _currentUser;

    public GetTasksByProjectQueryHandler(
        IRepository<Task> taskRepo,
        IRepository<Project> projectRepo,
        ICurrentUserService currentUser)
    {
        _taskRepo = taskRepo;
        _projectRepo = projectRepo;
        _currentUser = currentUser;
    }

    public async Task<ApiResponse<List<TaskDto>>> Handle(
        GetTasksByProjectQuery request,
        CancellationToken cancellationToken)
    {
        var project = await _projectRepo.GetByIdAsync(request.ProjectId);

        if (project == null)
            return ApiResponse<List<TaskDto>>
                .Fail("Project not found");

        if (project.UserId != _currentUser.UserId)
            return ApiResponse<List<TaskDto>>
                .Fail("Unauthorized access");

        var tasks = await _taskRepo.GetAllAsync();

        var projectTasks = tasks
            .Where(x => x.ProjectId == request.ProjectId)
            .ToList();

        var result = projectTasks.Select(task => new TaskDto
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            DueDate = task.DueDate,
            Priority = task.Priority.ToString(),
            Status = task.Status.ToString(),
        }).ToList();

        return ApiResponse<List<TaskDto>>
            .SuccessResult(result, "Tasks retrieved successfully");
    }
}