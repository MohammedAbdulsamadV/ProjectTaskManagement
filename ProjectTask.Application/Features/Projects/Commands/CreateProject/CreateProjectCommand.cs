using MediatR;
using ProjectTask.Application.Common.Models;
using ProjectTask.Application.DTOs;

namespace ProjectTask.Application.Features.Projects.Commands.CreateProject;

public class CreateProjectCommand : IRequest<ApiResponse<ProjectDto>>
{
    public CreateProjectDto Model { get; set; }

    public CreateProjectCommand(CreateProjectDto model)
    {
        Model = model;
    }
}