using Todo.Domain.Entities;

namespace Todo.Domain.Repositories;

public interface ITodoItemRepository
{
    Task<TodoItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<TodoItem>> GetAllAsync(
        Guid userId,
        int page,
        int pageSize,
        string? filter,
        string? sortBy,
        bool ascending,
        CancellationToken cancellationToken = default);
    Task AddAsync(TodoItem todoItem, CancellationToken cancellationToken = default);
    Task UpdateAsync(TodoItem todoItem, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}