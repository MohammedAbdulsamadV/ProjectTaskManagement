using MediatR;
using ProjectTask.Application.Common.Models;
using ProjectTask.Application.Interfaces;
using ProjectTask.Domain.Entities;

namespace ProjectTask.Application.Features.Projects.Commands.DeleteProject;

public class DeleteProjectCommandHandler 
    : IRequestHandler<DeleteProjectCommand, ApiResponse<bool>>
{
    private readonly IRepository<Project> _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;

    public DeleteProjectCommandHandler(
        IRepository<Project> repository,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<ApiResponse<bool>> Handle(
        DeleteProjectCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var project = await _repository.GetByIdAsync(request.Id);

            if (project == null)
                return ApiResponse<bool>.Fail("Project not found");

            // 🔐 ownership check
            if (project.UserId != _currentUser.UserId)
                return ApiResponse<bool>.Fail("Unauthorized access");

            _repository.Delete(project);
            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResult(true, "Project deleted successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<bool>.Fail($"Error: {ex.Message}");
        }
    }
}