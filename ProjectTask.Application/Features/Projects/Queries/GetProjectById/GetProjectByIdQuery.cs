using MediatR;
using ProjectTask.Application.Common.Models;
using ProjectTask.Application.DTOs;

namespace ProjectTask.Application.Features.Projects.Queries.GetProjectById;

public class GetProjectByIdQuery : IRequest<ApiResponse<ProjectDto>>
{
    public int Id { get; set; }

    public GetProjectByIdQuery(int id)
    {
        Id = id;
    }
}