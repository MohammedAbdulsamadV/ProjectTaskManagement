using MediatR;
using ProjectTask.Application.Common.Models;
using ProjectTask.Application.DTOs;

namespace ProjectTask.Application.Features.Tasks.Commands.UpdateTaskStatus;

public class UpdateTaskStatusCommand : IRequest<ApiResponse<TaskDto>>
{
    public UpdateTaskStatusDto Model { get; set; }

    public UpdateTaskStatusCommand(UpdateTaskStatusDto model)
    {
        Model = model;
    }
}