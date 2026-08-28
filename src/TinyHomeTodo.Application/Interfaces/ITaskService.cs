using TinyHomeTodo.Application.Dtos;

namespace TinyHomeTodo.Application.Interfaces;

public interface ITaskService
{
    Task<List<TaskResponseDto>> GetAllAsync(TaskQuery query, CancellationToken ct = default);

    Task<TaskResponseDto> GetByIdAsync(Guid id, CancellationToken ct = default);

    Task<TaskResponseDto> CreateAsync(CreateTaskCommand command, CancellationToken ct = default);

    Task<TaskResponseDto> UpdateAsync(UpdateTaskCommand command, CancellationToken ct = default);
}
