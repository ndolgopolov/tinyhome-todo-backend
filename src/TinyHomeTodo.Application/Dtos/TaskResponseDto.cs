namespace TinyHomeTodo.Application.Dtos;

public class TaskResponseDto
{
    public Guid Id { get; set; }
    public string TaskDescription { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public DateTime? DueDate { get; set; }
    public bool Completed { get; set; }
}
