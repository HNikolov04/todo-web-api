using Todo.Domain.Entities;

namespace Todo.Application.Abstractions.Services;

public interface IJwtProvider
{
    string Generate(User user);
}
