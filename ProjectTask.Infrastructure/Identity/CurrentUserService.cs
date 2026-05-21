using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using ProjectTask.Application.Interfaces;

namespace ProjectTask.Infrastructure.Identity;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public int UserId
    {
        get
        {
            var user = _httpContextAccessor.HttpContext?.User;

            var userId = user?.Claims
                .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?
                .Value;

            if (string.IsNullOrEmpty(userId))
                throw new Exception("User not authenticated");

            return int.Parse(userId);
        }
    }

    public string Email
    {
        get
        {
            var user = _httpContextAccessor.HttpContext?.User;

            var email = user?.Claims
                .FirstOrDefault(x => x.Type == ClaimTypes.Email)?
                .Value;

            if (string.IsNullOrEmpty(email))
                throw new Exception("User not authenticated");

            return email;
        }
    }
}