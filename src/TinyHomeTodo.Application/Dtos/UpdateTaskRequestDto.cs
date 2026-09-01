using TinyHomeTodo.Application.Validation;

namespace TinyHomeTodo.Application.Dtos;

// createdDate is immutable
public class UpdateTaskRequestDto
{
    public Guid Id { get; init; }
    [NotBlank]
    public string TaskDescription { get; init; } = string.Empty;
    public bool Completed { get; init; }
    public DateTime? DueDate { get; init; }
}
