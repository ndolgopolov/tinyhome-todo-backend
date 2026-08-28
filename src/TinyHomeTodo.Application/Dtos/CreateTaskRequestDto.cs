using System.ComponentModel.DataAnnotations;

namespace TinyHomeTodo.Application.Dtos;

public class CreateTaskRequestDto
{
    [Required]
    public string TaskDescription { get; set; } = string.Empty;
    public bool? Completed { get; set; }
    public DateTime? DueDate { get; set; }
}
