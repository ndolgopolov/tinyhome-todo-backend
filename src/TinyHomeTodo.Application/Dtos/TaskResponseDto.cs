namespace TinyHomeTodo.Application.Dtos;

public class TaskResponseDto
{
    public Guid Id { get; set; }
    public string TaskDescription { get; set; } = string.Empty;
    public bool Completed { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime CreatedDate { get; set; }
}
