using FluentValidation;

namespace ProjectTask.Application.Features.Projects.Commands.CreateProject;

public class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
{
    public CreateProjectCommandValidator()
    {
        RuleFor(x => x.Model.Name)
            .NotEmpty()
            .MaximumLength(100);
    }
}