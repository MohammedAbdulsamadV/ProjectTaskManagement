using FluentValidation;
using ProjectTask.Application.Features.Projects.Commands.CreateProject;

namespace ProjectTask.Application.Features.Projects.Commands.UpdateProject;

public class UpdateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
{
    public UpdateProjectCommandValidator()
    {
        RuleFor(x => x.Model.Name)
            .NotEmpty()
            .MaximumLength(100);
    }
}