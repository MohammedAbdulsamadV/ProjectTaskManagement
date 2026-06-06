using MediatR;
using ProjectTask.Application.Common.Models;
using ProjectTask.Application.DTOs;

namespace ProjectTask.Application.Features.Auth.Commands;

public class LoginCommand : IRequest<ApiResponse<AuthResponseDto>>
{
    public LoginDto Model { get; set; }

    public LoginCommand(LoginDto model)
    {
        Model = model;
    }
}