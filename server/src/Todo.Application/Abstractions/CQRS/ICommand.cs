using MediatR;
using Todo.Domain.Shared;

namespace Todo.Application.Abstractions.CQRS;

public interface ICommand : IRequest<Result>
{
}

public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
}