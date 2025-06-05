using MediatR;
using Todo.Domain.Shared;

namespace Todo.Application.Abstractions.CQRS;

public interface IQueryHandler<TQuery, TResponse>
    : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>
{
}