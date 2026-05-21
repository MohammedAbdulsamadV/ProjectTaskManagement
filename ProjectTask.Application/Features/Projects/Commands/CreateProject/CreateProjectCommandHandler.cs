using AutoMapper;
using MediatR;
using ProjectTask.Application.Common.Models;
using ProjectTask.Application.DTOs;
using ProjectTask.Application.Interfaces;
using ProjectTask.Domain.Entities;

namespace ProjectTask.Application.Features.Projects.Commands.CreateProject;

public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand,ApiResponse<ProjectDto>>
{
    private readonly IRepository<Project> _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUser;

    public CreateProjectCommandHandler(
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
        CreateProjectCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            // 🧠 Validation (basic inside handler - or use FluentValidation)
            if (string.IsNullOrWhiteSpace(request.Model.Name))
                return ApiResponse<ProjectDto>.Fail("Project name is required");

            // 📦 Map DTO → Entity
            var project = _mapper.Map<Project>(request.Model);

            // 🔐 Assign current user (IMPORTANT for multi-tenant)
            project.UserId = _currentUser.UserId;

            project.CreatedAt = DateOnly.FromDateTime(DateTime.UtcNow);

            // 💾 Save
            await _repository.AddAsync(project);
            await _unitOfWork.SaveChangesAsync();

            // 📤 Map back to DTO
            var result = _mapper.Map<ProjectDto>(project);

            // ✅ Response
            return ApiResponse<ProjectDto>.SuccessResult(
                result,
                "Project created successfully");
        }
        catch (Exception ex)
        {
            // ❌ Safe error handling
            return ApiResponse<ProjectDto>.Fail(
                $"Error occurred while creating project: {ex.Message}");
        }
    }
}