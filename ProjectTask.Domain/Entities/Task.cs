using ProjectTask.Domain.Common;
using ProjectTask.Domain.Enums;
using TaskStatus = System.Threading.Tasks.TaskStatus;

namespace ProjectTask.Domain.Entities;

public class Task : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public TaskStatus Status { get; set; }  
    public DateOnly DueDate { get; set; }
    public TaskPriority Priority { get; set; }
    
    public int ProjectId { get; set; }
    public Project Project { get; set; } = null!;}