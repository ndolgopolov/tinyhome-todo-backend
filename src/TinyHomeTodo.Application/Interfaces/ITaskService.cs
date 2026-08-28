using TinyHomeTodo.Application.Dtos;

namespace TinyHomeTodo.Application.Interfaces;

public interface ITaskService
{
    Task<List<TaskResponseDto>> GetAllAsync(TaskQuery query, CancellationToken ct = default);
}
