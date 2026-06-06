using AutoMapper;
using MediatR;
using ProjectTask.Application.Common.Models;
using ProjectTask.Application.DTOs;
using ProjectTask.Application.Interfaces;
using Task = ProjectTask.Domain.Entities.Task;

namespace ProjectTask.Application.Features.Tasks.Commands.UpdateTaskStatus;

public class UpdateTaskStatusCommandHandler 
    : IRequestHandler<UpdateTaskStatusCommand, ApiResponse<TaskDto>>
{
    private readonly ITaskRepository _taskRepo;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUser;

    public UpdateTaskStatusCommandHandler(
        ITaskRepository taskRepo,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ICurrentUserService currentUser)
    {
        _taskRepo = taskRepo;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUser = currentUser;
    }

    public async Task<ApiResponse<TaskDto>> Handle(UpdateTaskStatusCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var task = await _taskRepo.GetTaskWithProjectAsync(request.Model.TaskId);
            if (task == null)
                return ApiResponse<TaskDto>.Fail("Task not found");

            if (task.Project.UserId != _currentUser.UserId)
                return ApiResponse<TaskDto>.Fail("Unauthorized access");

            task.Status = request.Model.Status;

            _taskRepo.Update(task);
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

            return ApiResponse<TaskDto>.SuccessResult(result, "Task status updated");
        }
        catch (Exception ex)
        {
            return ApiResponse<TaskDto>.Fail(ex.Message);
        }
    }
}