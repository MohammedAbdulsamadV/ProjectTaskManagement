using MediatR;
using ProjectTask.Application.Common.Models;
using ProjectTask.Application.Interfaces;
using Task = ProjectTask.Domain.Entities.Task;

namespace ProjectTask.Application.Features.Tasks.Commans.DeleteTask;

public class DeleteTaskCommandHandler 
    : IRequestHandler<DeleteTaskCommand, ApiResponse<bool>>
{
    private readonly IRepository<Task> _taskRepo;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;

    public DeleteTaskCommandHandler(
        IRepository<Task> taskRepo,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser)
    {
        _taskRepo = taskRepo;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<ApiResponse<bool>> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var task = await _taskRepo.GetByIdAsync(request.Id);

            if (task == null)
                return ApiResponse<bool>.Fail("Task not found");

            if (task.Project.UserId != _currentUser.UserId)
                return ApiResponse<bool>.Fail("Unauthorized access");

            _taskRepo.Delete(task);
            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResult(true, "Task deleted successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<bool>.Fail(ex.Message);
        }
    }
}