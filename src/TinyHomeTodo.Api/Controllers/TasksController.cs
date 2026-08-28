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
}
