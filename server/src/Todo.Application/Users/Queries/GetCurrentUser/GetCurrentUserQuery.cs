using Todo.Application.Abstractions.CQRS;

namespace Todo.Application.Users.Queries.GetCurrentUser;

public sealed record GetCurrentUserQuery(Guid UserId) : IQuery<GetCurrentUserResponse>;