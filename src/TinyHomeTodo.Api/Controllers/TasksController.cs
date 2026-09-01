using Microsoft.AspNetCore.Mvc;
using TinyHomeTodo.Application.Dtos;
using TinyHomeTodo.Application.Interfaces;

namespace TinyHomeTodo.Api.Controllers;

[ApiController]
[Route("api/tasks")]
[Produces("application/json")]
[ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
[ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status500InternalServerError)]
public class TasksController : ControllerBase
{
    private readonly ITaskService _taskService;

    public TasksController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    /// <summary>List tasks, optionally filtered by completion and sorted</summary>
    /// <param name="sortBy"><c>dueDate</c> or <c>createdDate</c>, <c>-</c> prefix for descending, default <c>dueDate</c> ascending</param>
    /// <response code="200">matching tasks</response>
    /// <response code="400">unknown <c>completed</c> or <c>sort_by</c> value</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<TaskResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<TaskResponseDto>>> GetAll(
        [FromQuery] bool? completed = null,
        [FromQuery(Name = "sort_by")] string? sortBy = null,
        CancellationToken ct = default)
    {
        var query = new TaskQuery(completed, TaskSort.Parse(sortBy));
        return Ok(await _taskService.GetAllAsync(query, ct));
    }

    /// <summary>Get one task by id</summary>
    /// <response code="200">the task</response>
    /// <response code="400">id is not a GUID</response>
    /// <response code="404">no task with that id</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(TaskResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TaskResponseDto>> GetById(Guid id, CancellationToken ct = default)
    {
        return Ok(await _taskService.GetByIdAsync(id, ct));
    }

    /// <summary>Create a task</summary>
    /// <response code="201">the created task</response>
    /// <response code="400">validation failed, or <c>dueDate</c> not in UTC</response>
    [HttpPost]
    [ProducesResponseType(typeof(TaskResponseDto), StatusCodes.Status201Created)]
    public async Task<ActionResult<TaskResponseDto>> Create(CreateTaskRequestDto request, CancellationToken ct = default)
    {
        var command = new CreateTaskCommand(request.TaskDescription, request.Completed, request.DueDate);
        var result = await _taskService.CreateAsync(command, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>Update an existing task</summary>
    /// <param name="id">must match the <c>id</c> in the body, <c>createdDate</c> is server-owned and ignored</param>
    /// <response code="200">the updated task</response>
    /// <response code="400">id invalid or mismatched, validation failed, or <c>dueDate</c> not in UTC</response>
    /// <response code="404">no task with that id</response>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(TaskResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TaskResponseDto>> Update(Guid id, UpdateTaskRequestDto request, CancellationToken ct = default)
    {
        var command = new UpdateTaskCommand(id, request.Id, request.TaskDescription, request.Completed, request.DueDate);
        var result = await _taskService.UpdateAsync(command, ct);
        return Ok(result);
    }

    /// <summary>Delete a task</summary>
    /// <response code="204">deleted</response>
    /// <response code="400">id is not a GUID</response>
    /// <response code="404">no task with that id</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct = default)
    {
        await _taskService.DeleteAsync(id, ct);
        return NoContent();
    }
}
