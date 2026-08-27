using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TinyHomeTodo.Application.Entities;

namespace TinyHomeTodo.Infrastructure.Persistence;

public class TodoTaskConfiguration : IEntityTypeConfiguration<TodoTask>
{
    public void Configure(EntityTypeBuilder<TodoTask> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.TaskDescription)
            .IsRequired();

        builder.Property(t => t.DueDate)
            .HasColumnType("timestamptz");

        builder.Property(t => t.CreatedDate)
            .HasColumnType("timestamptz");

        builder.HasIndex(t => t.DueDate);
        builder.HasIndex(t => t.Completed);
    }
}
