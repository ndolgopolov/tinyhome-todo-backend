using TinyHomeTodo.Application.Validation;

namespace TinyHomeTodo.Application.Dtos;

public class CreateTaskRequestDto
{
    [NotBlank]
    public string TaskDescription { get; set; } = string.Empty;
    public bool? Completed { get; set; }
    public DateTime? DueDate { get; set; }
}
