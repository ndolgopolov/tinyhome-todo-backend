using TinyHomeTodo.Application.Dtos;
using TinyHomeTodo.Application.Entities;
using TinyHomeTodo.Application.Interfaces;

namespace TinyHomeTodo.Application.Services;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _repository;

    public TaskService(ITaskRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<TaskResponseDto>> GetAllAsync(TaskQuery query, CancellationToken ct = default)
    {
        var tasks = await _repository.GetAllAsync(query.Completed, query.Sort, ct);
        return tasks.Select(Map).ToList();
    }

    private static TaskResponseDto Map(TodoTask task) => new()
    {
        Id = task.Id,
        TaskDescription = task.TaskDescription,
        Completed = task.Completed,
        DueDate = task.DueDate,
        CreatedDate = task.CreatedDate
    };
}
