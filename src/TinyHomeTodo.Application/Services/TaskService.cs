using TinyHomeTodo.Application.Dtos;
using TinyHomeTodo.Application.Entities;
using TinyHomeTodo.Application.Exceptions;
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

    public async Task<TaskResponseDto> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var task = await _repository.GetByIdAsync(id, ct)
            ?? throw new NotFoundException($"Task with id {id} was not found.");
        return Map(task);
    }

    public async Task<TaskResponseDto> CreateAsync(CreateTaskCommand command, CancellationToken ct = default)
    {
        var task = new TodoTask
        {
            TaskDescription = command.TaskDescription,
            Completed = command.Completed ?? false,
            DueDate = command.DueDate,
            CreatedDate = DateTime.UtcNow
        };

        _repository.Add(task);
        await _repository.SaveChangesAsync(ct);

        return Map(task);
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
