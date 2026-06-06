using AutoMapper;
using MediatR;
using ProjectTask.Application.Common.Models;
using ProjectTask.Application.DTOs;
using ProjectTask.Application.Interfaces;
using ProjectTask.Domain.Entities;

namespace ProjectTask.Application.Features.Projects.Commands.UpdateProject;

public class UpdateProjectCommandHandler 
    : IRequestHandler<UpdateProjectCommand, ApiResponse<ProjectDto>>
{
    private readonly IRepository<Project> _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUser;

    public UpdateProjectCommandHandler(
        IRepository<Project> repository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ICurrentUserService currentUser)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUser = currentUser;
    }

    public async Task<ApiResponse<ProjectDto>> Handle(
        UpdateProjectCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var project = await _repository.GetByIdAsync(request.Model.Id);

            if (project == null)
                return ApiResponse<ProjectDto>.Fail("Project not found");

            if (project.UserId != _currentUser.UserId)
                return ApiResponse<ProjectDto>.Fail("Unauthorized access");

            project.Name = request.Model.Name;
            project.Description = request.Model.Description;

            _repository.Update(project);
            await _unitOfWork.SaveChangesAsync();

            var result = _mapper.Map<ProjectDto>(project);

            return ApiResponse<ProjectDto>.SuccessResult(
                result,
                "Project updated successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<ProjectDto>.Fail($"Error: {ex.Message}");
        }
    }
}