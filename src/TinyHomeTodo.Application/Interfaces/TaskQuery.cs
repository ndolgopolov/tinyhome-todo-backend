namespace TinyHomeTodo.Application.Interfaces;

public record TaskQuery(bool? Completed, TaskSort Sort);
