using TinyHomeTodo.Application.Entities;

namespace TinyHomeTodo.Application.Interfaces;

public interface ITaskRepository
{
    Task<List<TodoTask>> GetAllAsync(CancellationToken ct = default);
}
