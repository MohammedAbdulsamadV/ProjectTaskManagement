using MediatR;
using ProjectTask.Application.Common.Models;

namespace ProjectTask.Application.Features.Projects.Commands.DeleteProject;

public class DeleteProjectCommand : IRequest<ApiResponse<bool>>
{
    public int Id { get; set; }

    public DeleteProjectCommand(int id)
    {
        Id = id;
    }
}