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
    private readonly ICacheService _cacheService;

    public GetAllProjectsQueryHandler(
        IRepository<Project> repository,
        IMapper mapper,
        ICurrentUserService currentUser,
        ICacheService cacheService)
    {
        _repository = repository;
        _mapper = mapper;
        _currentUser = currentUser;
        _cacheService = cacheService;
    }

    public async Task<ApiResponse<List<ProjectDto>>> Handle(
        GetAllProjectsQuery request,
        CancellationToken cancellationToken)
    {
        // 🔑 unique cache key per user
        var cacheKey = $"projects_user_{_currentUser.UserId}";

        // ✅ Try cache first
        var cachedProjects =
            await _cacheService.GetAsync<List<ProjectDto>>(cacheKey);

        if (cachedProjects is not null)
        {
            return ApiResponse<List<ProjectDto>>
                .SuccessResult(cachedProjects, "Projects retrieved from cache");
        }

        // ❌ Cache miss → DB
        var projects = await _repository.GetAllAsync();

        var userProjects = projects
            .Where(x => x.UserId == _currentUser.UserId)
            .ToList();

        var result = _mapper.Map<List<ProjectDto>>(userProjects);

        // 💾 Store in Redis
        await _cacheService.SetAsync(
            cacheKey,
            result,
            TimeSpan.FromMinutes(10));

        return ApiResponse<List<ProjectDto>>
            .SuccessResult(result, "Projects retrieved from database");
    }
}