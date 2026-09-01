using System.ComponentModel.DataAnnotations;

namespace TinyHomeTodo.Application.Validation;

public class NotBlankAttribute : ValidationAttribute
{
    public NotBlankAttribute() : base("taskDescription must not be blank.")
    {
    }

    public override bool IsValid(object? value)
        => value is string text && !string.IsNullOrWhiteSpace(text);
}
