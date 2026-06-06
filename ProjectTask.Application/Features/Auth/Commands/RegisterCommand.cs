using MediatR;
using ProjectTask.Application.Common.Models;
using ProjectTask.Application.DTOs;

namespace ProjectTask.Application.Features.Auth.Commands;

public class RegisterCommand : IRequest<ApiResponse<AuthResponseDto>>
{
    public RegisterDto Model { get; set; }

    public RegisterCommand(RegisterDto model)
    {
        Model = model;
    }
}