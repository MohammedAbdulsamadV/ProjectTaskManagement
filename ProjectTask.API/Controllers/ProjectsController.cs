using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectTask.Application.Features.Projects.Commands.CreateProject;
using ProjectTask.Application.Features.Projects.Commands.DeleteProject;
using ProjectTask.Application.Features.Projects.Commands.UpdateProject;
using ProjectTask.Application.Features.Projects.Queries.GetAllProjects;
using ProjectTask.Application.Features.Projects.Queries.GetProjectById;

namespace ProjectTask.API.Controllers;

[ApiController]
[Route("api/projects")]
[Authorize]
public class ProjectsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProjectsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateProjectCommand command)
        => Ok(await _mediator.Send(command));

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _mediator.Send(new GetAllProjectsQuery()));

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
        => Ok(await _mediator.Send(new GetProjectByIdQuery(id)));

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateProjectCommand command)
    {
        command.Model.Id = id;
        return Ok(await _mediator.Send(command));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
        => Ok(await _mediator.Send(new DeleteProjectCommand(id)));
}