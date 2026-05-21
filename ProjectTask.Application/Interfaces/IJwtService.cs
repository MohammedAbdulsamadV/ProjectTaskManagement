using ProjectTask.Domain.Entities;

namespace ProjectTask.Application.Interfaces;

public interface IJwtService
{
    string GenerateToken(User user);

}