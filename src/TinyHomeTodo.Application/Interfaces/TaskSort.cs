using TinyHomeTodo.Application.Exceptions;

namespace TinyHomeTodo.Application.Interfaces;

public enum TaskSortField
{
    DueDate,
    CreatedDate
}

public enum SortDirection
{
    Asc,
    Desc
}

public record TaskSort(TaskSortField Field, SortDirection Direction)
{
    public static readonly TaskSort Default = new(TaskSortField.DueDate, SortDirection.Asc);

    public static TaskSort Parse(string? raw)
    {
        if (string.IsNullOrWhiteSpace(raw))
        {
            return Default;
        }

        var token = raw.Trim();
        var direction = SortDirection.Asc;

        if (token[0] == '-')
        {
            direction = SortDirection.Desc;
            token = token[1..];
        }

        var field = token.ToLowerInvariant() switch
        {
            "duedate" => TaskSortField.DueDate,
            "createddate" => TaskSortField.CreatedDate,
            _ => throw new BadRequestException($"Unknown sort_by value '{raw}'.")
        };

        return new TaskSort(field, direction);
    }
}
