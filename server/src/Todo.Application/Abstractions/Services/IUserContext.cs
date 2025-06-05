namespace Todo.Application.Abstractions.Services;

public interface IUserContext
{
    Guid UserId { get; }
}