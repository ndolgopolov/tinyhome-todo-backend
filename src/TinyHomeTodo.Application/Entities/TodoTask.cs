namespace TinyHomeTodo.Application.Entities;

public class TodoTask
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string TaskDescription { get; set; } = string.Empty;

    public bool Completed { get; set; }

    public DateTime? DueDate { get; set; }

    public DateTime CreatedDate { get; set; }
}
