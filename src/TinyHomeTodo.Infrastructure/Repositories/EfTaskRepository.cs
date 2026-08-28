using Microsoft.EntityFrameworkCore;
using TinyHomeTodo.Application.Entities;
using TinyHomeTodo.Application.Interfaces;
using TinyHomeTodo.Infrastructure.Persistence;

namespace TinyHomeTodo.Infrastructure.Repositories;

public class EfTaskRepository : ITaskRepository
{
    private readonly TinyHomeTodoDbContext _db;

    public EfTaskRepository(TinyHomeTodoDbContext db)
    {
        _db = db;
    }

    public async Task<List<TodoTask>> GetAllAsync(bool? completed, TaskSort sort, CancellationToken ct = default)
    {
        var query = _db.Tasks.AsQueryable();

        if (completed.HasValue)
        {
            query = query.Where(t => t.Completed == completed.Value);
        }

        var ordered = sort switch
        {
            { Field: TaskSortField.DueDate, Direction: SortDirection.Asc } =>
                query.OrderBy(t => t.DueDate == null).ThenBy(t => t.DueDate),
            { Field: TaskSortField.DueDate, Direction: SortDirection.Desc } =>
                query.OrderBy(t => t.DueDate == null).ThenByDescending(t => t.DueDate),
            { Field: TaskSortField.CreatedDate, Direction: SortDirection.Asc } =>
                query.OrderBy(t => t.CreatedDate),
            { Field: TaskSortField.CreatedDate, Direction: SortDirection.Desc } =>
                query.OrderByDescending(t => t.CreatedDate),
            _ => throw new ArgumentOutOfRangeException(nameof(sort), sort, "Unhandled sort field/direction")
        };

        return await ordered.ThenBy(t => t.Id).ToListAsync(ct);
    }

    public Task<TodoTask?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => _db.Tasks.FirstOrDefaultAsync(t => t.Id == id, ct);

    // Sync as recommended per EF documentation
    public void Add(TodoTask task) => _db.Tasks.Add(task);

    public Task<int> SaveChangesAsync(CancellationToken ct = default)
        => _db.SaveChangesAsync(ct);
}
