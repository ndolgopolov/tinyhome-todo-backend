using System.ComponentModel.DataAnnotations;

namespace TinyHomeTodo.Application.Dtos;

// createdDate is immutable
public class UpdateTaskRequestDto
{
    public Guid Id { get; init; }
    [Required]
    public string TaskDescription { get; init; } = string.Empty;
    public bool Completed { get; init; }
    public DateTime? DueDate { get; init; }
}
