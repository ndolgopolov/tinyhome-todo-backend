using Microsoft.AspNetCore.Mvc;
using TinyHomeTodo.Application.Dtos;

namespace TinyHomeTodo.Api.Controllers;

[ApiController]
[Route("api/tasks")]
public class TasksController : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<TaskResponseDto>> GetAll()
    {
        var tasks = new List<TaskResponseDto>
        {
            new()
            {
                Id = 1,
                TaskDescription = "Finish the project",
                Completed = false,
                DueDate = new DateTime(2026, 8, 29, 0, 0, 0, DateTimeKind.Utc),
                CreatedDate = new DateTime(2026, 8, 27, 0, 0, 0, DateTimeKind.Utc)
            },
            new()
            {
                Id = 2,
                TaskDescription = "Renew registration",
                Completed = true,
                DueDate = null,
                CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            }
        };

        return Ok(tasks);
    }
}
