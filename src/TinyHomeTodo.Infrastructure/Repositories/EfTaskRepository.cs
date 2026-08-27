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

    public async Task<List<TodoTask>> GetAllAsync(CancellationToken ct = default)
    {
        return await _db.Tasks
            .OrderBy(t => t.DueDate)
            .ToListAsync(ct);
    }
}
