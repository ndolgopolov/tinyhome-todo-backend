namespace TinyHomeTodo.Application.Interfaces;

public record UpdateTaskCommand(Guid RouteId, Guid BodyId, string TaskDescription, bool Completed, DateTime? DueDate);
