using MediatR;
using ProjectTask.Application.Common.Models;

namespace ProjectTask.Application.Features.Tasks.Commands.DeleteTask;

public class DeleteTaskCommand : IRequest<ApiResponse<bool>>
{
    public int Id { get; set; }

    public DeleteTaskCommand(int id)
    {
        Id = id;
    }
}