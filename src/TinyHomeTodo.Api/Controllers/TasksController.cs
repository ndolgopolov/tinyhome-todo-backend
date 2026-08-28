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
    public async Task<ActionResult<IEnumerable<TaskResponseDto>>> GetAll(CancellationToken ct)
    {
        return Ok(await _taskService.GetAllAsync(ct));
    }
}
