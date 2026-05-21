using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectTask.Application.Features.Tasks.Commans.CreateTask;
using ProjectTask.Application.Features.Tasks.Commans.DeleteTask;
using ProjectTask.Application.Features.Tasks.Commans.UpdateTaskStatus;
using ProjectTask.Application.Features.Tasks.Queries.GetTasksByProject;

namespace ProjectTask.API.Controllers;

[ApiController]
[Route("api/tasks")]
[Authorize]
public class TasksController : ControllerBase
{
    private readonly IMediator _mediator;

    public TasksController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateTaskCommand command)
        => Ok(await _mediator.Send(command));

    [HttpPut("status")]
    public async Task<IActionResult> UpdateStatus(UpdateTaskStatusCommand command)
        => Ok(await _mediator.Send(command));

    [HttpGet("project/{projectId}")]
    public async Task<IActionResult> GetByProject(int projectId)
        => Ok(await _mediator.Send(new GetTasksByProjectQuery(projectId)));

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
        => Ok(await _mediator.Send(new DeleteTaskCommand(id)));
}