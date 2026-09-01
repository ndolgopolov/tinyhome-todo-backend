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
        if (command.DueDate is { } dueDate && dueDate.Kind != DateTimeKind.Utc)
        {
            throw new BadRequestException("dueDate must be sent in UTC, e.g. 2026-08-30T00:00:00Z.");
        }

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

    public async Task<TaskResponseDto> UpdateAsync(UpdateTaskCommand command, CancellationToken ct = default)
    {
        if (command.RouteId != command.BodyId)
        {
            throw new BadRequestException("The id in the request body must match the id in the route.");
        }

        if (command.DueDate is { } dueDate && dueDate.Kind != DateTimeKind.Utc)
        {
            throw new BadRequestException("dueDate must be sent in UTC, e.g. 2026-08-30T00:00:00Z.");
        }

        var task = await _repository.GetByIdAsync(command.RouteId, ct)
            ?? throw new NotFoundException($"Task with id {command.RouteId} was not found.");

        task.TaskDescription = command.TaskDescription;
        task.Completed = command.Completed;
        task.DueDate = command.DueDate;

        await _repository.SaveChangesAsync(ct);
        return Map(task);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var task = await _repository.GetByIdAsync(id, ct)
            ?? throw new NotFoundException($"Task with id {id} was not found.");

        _repository.Remove(task);
        await _repository.SaveChangesAsync(ct);
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
