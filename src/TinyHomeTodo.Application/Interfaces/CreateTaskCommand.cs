namespace TinyHomeTodo.Application.Interfaces;

public record CreateTaskCommand(string TaskDescription, bool? Completed, DateTime? DueDate);
