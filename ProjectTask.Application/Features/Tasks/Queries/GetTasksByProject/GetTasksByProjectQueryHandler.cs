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
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUser;
    private readonly ICacheService _cacheService;

    public GetTasksByProjectQueryHandler(
        IRepository<Task> taskRepo,
        IRepository<Project> projectRepo,
        IMapper mapper,
        ICurrentUserService currentUser,
        ICacheService cacheService)
    {
        _taskRepo = taskRepo;
        _projectRepo = projectRepo;
        _mapper = mapper;
        _currentUser = currentUser;
        _cacheService = cacheService;
    }

    public async Task<ApiResponse<List<TaskDto>>> Handle(
        GetTasksByProjectQuery request,
        CancellationToken cancellationToken)
    {
        var cacheKey =
            $"tasks_project_{request.ProjectId}_user_{_currentUser.UserId}";

        var cachedTasks =
            await _cacheService.GetAsync<List<TaskDto>>(cacheKey);

        if (cachedTasks is not null)
        {
            return ApiResponse<List<TaskDto>>
                .SuccessResult(cachedTasks, "Tasks retrieved from cache");
        }

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

        var result = _mapper.Map<List<TaskDto>>(projectTasks);

        await _cacheService.SetAsync(
            cacheKey,
            result,
            TimeSpan.FromMinutes(10));

        return ApiResponse<List<TaskDto>>
            .SuccessResult(result, "Tasks retrieved from database");
    }
}