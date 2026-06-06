using ProjectTask.Domain.Common;

namespace ProjectTask.Domain.Entities;

public class Project : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; }  = string.Empty;
    public DateOnly CreatedAt { get; set; } =  DateOnly.FromDateTime(DateTime.UtcNow);
    
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public ICollection<Task> Tasks { get; set; } = new List<Task>();
}