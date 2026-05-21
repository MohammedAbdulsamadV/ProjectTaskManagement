using AutoMapper;
using MediatR;
using ProjectTask.Application.Common.Models;
using ProjectTask.Application.DTOs;
using ProjectTask.Application.Interfaces;
using Task = ProjectTask.Domain.Entities.Task;

namespace ProjectTask.Application.Features.Tasks.Commans.UpdateTaskStatus;

public class UpdateTaskStatusCommandHandler 
    : IRequestHandler<UpdateTaskStatusCommand, ApiResponse<TaskDto>>
{
    private readonly IRepository<Task> _taskRepo;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUser;

    public UpdateTaskStatusCommandHandler(
        IRepository<Task> taskRepo,
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
            var task = await _taskRepo.GetByIdAsync(request.Model.TaskId);

            if (task == null)
                return ApiResponse<TaskDto>.Fail("Task not found");

            if (task.Project.UserId != _currentUser.UserId)
                return ApiResponse<TaskDto>.Fail("Unauthorized access");

            task.Status = (TaskStatus)request.Model.Status;

            _taskRepo.Update(task);
            await _unitOfWork.SaveChangesAsync();

            var result = _mapper.Map<TaskDto>(task);

            return ApiResponse<TaskDto>.SuccessResult(result, "Task status updated");
        }
        catch (Exception ex)
        {
            return ApiResponse<TaskDto>.Fail(ex.Message);
        }
    }
}