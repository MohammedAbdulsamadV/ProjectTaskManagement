using AutoMapper;
using MediatR;
using ProjectTask.Application.Common.Models;
using ProjectTask.Application.DTOs;
using ProjectTask.Application.Interfaces;
using ProjectTask.Domain.Entities;
using ProjectTask.Domain.Enums;
using Task = ProjectTask.Domain.Entities.Task;

namespace ProjectTask.Application.Features.Tasks.Commands.CreateTask;

public class CreateTaskCommandHandler 
    : IRequestHandler<CreateTaskCommand, ApiResponse<TaskDto>>
{
    private readonly IRepository<Task> _taskRepo;
    private readonly IRepository<Project> _projectRepo;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUser;

    public CreateTaskCommandHandler(
        IRepository<Task> taskRepo,
        IRepository<Project> projectRepo,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ICurrentUserService currentUser)
    {
        _taskRepo = taskRepo;
        _projectRepo = projectRepo;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUser = currentUser;
    }

    public async Task<ApiResponse<TaskDto>> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var project = await _projectRepo.GetByIdAsync(request.Model.ProjectId);

            if (project == null)
                return ApiResponse<TaskDto>.Fail("Project not found");

            if (project.UserId != _currentUser.UserId)
                return ApiResponse<TaskDto>.Fail("Unauthorized access to project");

            var task = new Task
            {
                Title = request.Model.Title,
                Description = request.Model.Description,
                DueDate = request.Model.DueDate,
                Priority = (TaskPriority)request.Model.Priority,
                ProjectId = request.Model.ProjectId,
            };

            await _taskRepo.AddAsync(task);
            await _unitOfWork.SaveChangesAsync();

            var result = new TaskDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                DueDate = task.DueDate,
                Priority = task.Priority.ToString(),
                Status = task.Status.ToString(),
            };

            return ApiResponse<TaskDto>.SuccessResult(result, "Task created successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<TaskDto>.Fail(ex.Message);
        }
    }
}