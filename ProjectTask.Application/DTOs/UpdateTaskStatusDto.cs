using TaskStatus = ProjectTask.Domain.Enums.TaskStatus;

namespace ProjectTask.Application.DTOs;

public class UpdateTaskStatusDto
{
    public int TaskId { get; set; }
    public TaskStatus Status { get; set; }
}