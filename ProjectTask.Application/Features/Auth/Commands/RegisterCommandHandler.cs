using MediatR;
using ProjectTask.Application.Common.Models;
using ProjectTask.Application.DTOs;
using ProjectTask.Application.Interfaces;
using ProjectTask.Domain.Entities;

namespace ProjectTask.Application.Features.Auth.Commands;

public class RegisterCommandHandler 
    : IRequestHandler<RegisterCommand, ApiResponse<AuthResponseDto>>
{
    private readonly IRepository<User> _userRepo;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtService _jwtService;

    public RegisterCommandHandler(
        IRepository<User> userRepo,
        IUnitOfWork unitOfWork,
        IJwtService jwtService)
    {
        _userRepo = userRepo;
        _unitOfWork = unitOfWork;
        _jwtService = jwtService;
    }

    public async Task<ApiResponse<AuthResponseDto>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var existingUsers = await _userRepo.GetAllAsync();

        if (existingUsers.Any(x => x.Email == request.Model.Email))
            return ApiResponse<AuthResponseDto>.Fail("Email already exists");

        var user = new User
        {
            UserName = request.Model.UserName,
            Email = request.Model.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Model.Password)
        };

        await _userRepo.AddAsync(user);
        await _unitOfWork.SaveChangesAsync();

        var token = _jwtService.GenerateToken(user);

        return ApiResponse<AuthResponseDto>.SuccessResult(
            new AuthResponseDto { Token = token },
            "User registered successfully");
    }
}