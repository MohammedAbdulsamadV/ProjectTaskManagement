using MediatR;
using ProjectTask.Application.Common.Models;
using ProjectTask.Application.DTOs;

namespace ProjectTask.Application.Features.Tasks.Commans.CreateTask;

public class CreateTaskCommand : IRequest<ApiResponse<TaskDto>>
{
    public CreateTaskDto Model { get; set; }

    public CreateTaskCommand(CreateTaskDto model)
    {
        Model = model;
    }
}