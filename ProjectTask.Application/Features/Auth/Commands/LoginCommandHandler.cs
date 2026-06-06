using MediatR;
using ProjectTask.Application.Common.Models;
using ProjectTask.Application.DTOs;
using ProjectTask.Application.Interfaces;
using ProjectTask.Domain.Entities;

namespace ProjectTask.Application.Features.Auth.Commands;

public class LoginCommandHandler 
    : IRequestHandler<LoginCommand, ApiResponse<AuthResponseDto>>
{
    private readonly IRepository<User> _userRepo;
    private readonly IJwtService _jwtService;

    public LoginCommandHandler(
        IRepository<User> userRepo,
        IJwtService jwtService)
    {
        _userRepo = userRepo;
        _jwtService = jwtService;
    }

    public async Task<ApiResponse<AuthResponseDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var users = await _userRepo.GetAllAsync();

        var user = users.FirstOrDefault(x => x.Email == request.Model.Email);

        if (user == null)
            return ApiResponse<AuthResponseDto>.Fail("Invalid credentials");

        if (!BCrypt.Net.BCrypt.Verify(request.Model.Password, user.PasswordHash))
            return ApiResponse<AuthResponseDto>.Fail("Invalid credentials");

        var token = _jwtService.GenerateToken(user);

        return ApiResponse<AuthResponseDto>.SuccessResult(
            new AuthResponseDto { Token = token },
            "Login successful");
    }
}