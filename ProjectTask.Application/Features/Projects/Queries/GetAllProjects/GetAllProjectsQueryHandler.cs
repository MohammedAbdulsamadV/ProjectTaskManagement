using AutoMapper;
using MediatR;
using ProjectTask.Application.Common.Models;
using ProjectTask.Application.DTOs;
using ProjectTask.Application.Interfaces;
using ProjectTask.Domain.Entities;

namespace ProjectTask.Application.Features.Projects.Queries.GetAllProjects;

public class GetAllProjectsQueryHandler
    : IRequestHandler<GetAllProjectsQuery, ApiResponse<List<ProjectDto>>>
{
    private readonly IRepository<Project> _repository;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUser;

    public GetAllProjectsQueryHandler(
        IRepository<Project> repository,
        IMapper mapper,
        ICurrentUserService currentUser)
    {
        _repository = repository;
        _mapper = mapper;
        _currentUser = currentUser;
    }

    public async Task<ApiResponse<List<ProjectDto>>> Handle(
        GetAllProjectsQuery request,
        CancellationToken cancellationToken)
    {
        var projects = await _repository.GetAllAsync();

        var userProjects = projects
            .Where(x => x.UserId == _currentUser.UserId)
            .ToList();

        var result = _mapper.Map<List<ProjectDto>>(userProjects);

        return ApiResponse<List<ProjectDto>>
            .SuccessResult(result, "Projects retrieved from database");
    }
}