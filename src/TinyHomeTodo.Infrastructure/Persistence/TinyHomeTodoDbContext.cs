using Microsoft.EntityFrameworkCore;
using TinyHomeTodo.Application.Entities;

namespace TinyHomeTodo.Infrastructure.Persistence;

public class TinyHomeTodoDbContext : DbContext
{
    public TinyHomeTodoDbContext(DbContextOptions<TinyHomeTodoDbContext> options)
        : base(options)
    {
    }

    public DbSet<TodoTask> Tasks => Set<TodoTask>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TinyHomeTodoDbContext).Assembly);
    }
}
