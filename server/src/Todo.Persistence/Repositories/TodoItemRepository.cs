using Microsoft.EntityFrameworkCore;
using Todo.Domain.Entities;
using Todo.Domain.Repositories;

namespace Todo.Persistence.Repositories;

public class TodoItemRepository : ITodoItemRepository
{
    private readonly ApplicationDbContext _context;

    public TodoItemRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<TodoItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.TodoItems
            .Include(t => t.Notes)
            .Include(t => t.ChangeLogEntries)
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public async Task<List<TodoItem>> GetAllAsync(
        Guid userId,
        int page,
        int pageSize,
        string? filter,
        string? sortBy,
        bool ascending,
        CancellationToken cancellationToken = default)
    {
        var query = _context.TodoItems
            .Where(t => t.UserId == userId) // 🔒 Always scoped to user
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter))
        {
            query = query.Where(t => t.Title.Value.Contains(filter));
        }

        query = ascending
            ? query.OrderBy(t => EF.Property<object>(t, sortBy ?? "CreatedOnUtc"))
            : query.OrderByDescending(t => EF.Property<object>(t, sortBy ?? "CreatedOnUtc"));

        return await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(TodoItem todoItem, CancellationToken cancellationToken = default)
    {
        await _context.TodoItems.AddAsync(todoItem, cancellationToken);
    }

    public Task UpdateAsync(TodoItem todoItem, CancellationToken cancellationToken = default)
    {
        _context.TodoItems.Update(todoItem);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var todoItem = await _context.TodoItems.FindAsync(new object[] { id }, cancellationToken);

        if (todoItem is not null)
        {
            _context.TodoItems.Remove(todoItem);
        }
    }
}