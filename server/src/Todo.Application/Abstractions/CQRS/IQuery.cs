using MediatR;
using Todo.Domain.Shared;

namespace Todo.Application.Abstractions.CQRS;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}