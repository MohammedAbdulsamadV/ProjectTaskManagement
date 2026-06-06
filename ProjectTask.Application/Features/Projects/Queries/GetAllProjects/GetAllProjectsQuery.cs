using MediatR;
using ProjectTask.Application.Common.Models;
using ProjectTask.Application.DTOs;

namespace ProjectTask.Application.Features.Projects.Queries.GetAllProjects;

public class GetAllProjectsQuery : IRequest<ApiResponse<List<ProjectDto>>>
{
}