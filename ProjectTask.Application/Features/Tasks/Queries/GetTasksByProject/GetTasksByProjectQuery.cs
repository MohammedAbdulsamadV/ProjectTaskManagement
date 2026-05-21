using MediatR;
using ProjectTask.Application.Common.Models;
using ProjectTask.Application.DTOs;

namespace ProjectTask.Application.Features.Tasks.Queries.GetTasksByProject;

public class GetTasksByProjectQuery : IRequest<ApiResponse<List<TaskDto>>>
{
    public int ProjectId { get; set; }

    public GetTasksByProjectQuery(int projectId)
    {
        ProjectId = projectId;
    }
}