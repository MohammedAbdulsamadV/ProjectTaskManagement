using MediatR;
using ProjectTask.Application.Common.Models;
using ProjectTask.Application.DTOs;

namespace ProjectTask.Application.Features.Tasks.Commans.UpdateTaskStatus;

public class UpdateTaskStatusCommand : IRequest<ApiResponse<TaskDto>>
{
    public UpdateTaskStatusDto Model { get; set; }

    public UpdateTaskStatusCommand(UpdateTaskStatusDto model)
    {
        Model = model;
    }
}