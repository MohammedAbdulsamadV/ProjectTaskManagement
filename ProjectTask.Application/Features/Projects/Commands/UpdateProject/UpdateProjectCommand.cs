using MediatR;
using ProjectTask.Application.Common.Models;
using ProjectTask.Application.DTOs;

namespace ProjectTask.Application.Features.Projects.Commands.UpdateProject;

public class UpdateProjectCommand : IRequest<ApiResponse<ProjectDto>>
{
    public UpdateProjectDto Model { get; set; }

    public UpdateProjectCommand(UpdateProjectDto model)
    {
        Model = model;
    }
}