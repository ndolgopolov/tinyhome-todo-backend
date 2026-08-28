using TinyHomeTodo.Application.Entities;

namespace TinyHomeTodo.Application.Interfaces;

public interface ITaskRepository
{
    Task<List<TodoTask>> GetAllAsync(bool? completed, TaskSort sort, CancellationToken ct = default);

    Task<TodoTask?> GetByIdAsync(Guid id, CancellationToken ct = default);

    void Add(TodoTask task);

    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
