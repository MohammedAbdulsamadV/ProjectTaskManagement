using AutoMapper;
using MediatR;
using ProjectTask.Application.Common.Models;
using ProjectTask.Application.DTOs;
using ProjectTask.Application.Interfaces;
using ProjectTask.Domain.Entities;

namespace ProjectTask.Application.Features.Projects.Queries.GetProjectById;

public class GetProjectByIdQueryHandler 
    : IRequestHandler<GetProjectByIdQuery, ApiResponse<ProjectDto>>
{
    private readonly IRepository<Project> _repository;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUser;

    public GetProjectByIdQueryHandler(
        IRepository<Project> repository,
        IMapper mapper,
        ICurrentUserService currentUser)
    {
        _repository = repository;
        _mapper = mapper;
        _currentUser = currentUser;
    }

    public async Task<ApiResponse<ProjectDto>> Handle(
        GetProjectByIdQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var project = await _repository.GetByIdAsync(request.Id);

            if (project == null)
                return ApiResponse<ProjectDto>.Fail("Project not found");

            // 🔐 security check
            if (project.UserId != _currentUser.UserId)
                return ApiResponse<ProjectDto>.Fail("Unauthorized access");

            var result = _mapper.Map<ProjectDto>(project);

            return ApiResponse<ProjectDto>.SuccessResult(
                result,
                "Project retrieved successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<ProjectDto>.Fail($"Error: {ex.Message}");
        }
    }
}