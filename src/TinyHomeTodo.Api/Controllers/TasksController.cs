using Microsoft.AspNetCore.Mvc;
using TinyHomeTodo.Application.Dtos;
using TinyHomeTodo.Application.Interfaces;

namespace TinyHomeTodo.Api.Controllers;

[ApiController]
[Route("api/tasks")]
public class TasksController : ControllerBase
{
    private readonly ITaskService _taskService;

    public TasksController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskResponseDto>>> GetAll(
        [FromQuery] bool? completed = null,
        [FromQuery(Name = "sort_by")] string? sortBy = null,
        CancellationToken ct = default)
    {
        var query = new TaskQuery(completed, TaskSort.Parse(sortBy));
        return Ok(await _taskService.GetAllAsync(query, ct));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TaskResponseDto>> GetById(Guid id, CancellationToken ct = default)
    {
        return Ok(await _taskService.GetByIdAsync(id, ct));
    }

    [HttpPost]
    public async Task<ActionResult<TaskResponseDto>> Create(CreateTaskRequestDto request, CancellationToken ct = default)
    {
        var command = new CreateTaskCommand(request.TaskDescription, request.Completed, request.DueDate);
        var result = await _taskService.CreateAsync(command, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<TaskResponseDto>> Update(Guid id, UpdateTaskRequestDto request, CancellationToken ct = default)
    {
        var command = new UpdateTaskCommand(id, request.Id, request.TaskDescription, request.Completed, request.DueDate);
        var result = await _taskService.UpdateAsync(command, ct);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct = default)
    {
        await _taskService.DeleteAsync(id, ct);
        return NoContent();
    }
}
