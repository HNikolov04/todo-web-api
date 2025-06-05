using Todo.Application.Abstractions.CQRS;
using Todo.Domain.Errors;
using Todo.Domain.Repositories;
using Todo.Domain.Shared;

namespace Todo.Application.Notes.Commands.DeleteNote;

public sealed class DeleteNoteCommandHandler : ICommandHandler<DeleteNoteCommand>
{
    private readonly ITodoItemRepository _todoItemRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteNoteCommandHandler(
        ITodoItemRepository todoItemRepository,
        IUnitOfWork unitOfWork)
    {
        _todoItemRepository = todoItemRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteNoteCommand request, CancellationToken cancellationToken)
    {
        var todoItem = await _todoItemRepository.GetByIdAsync(request.TodoItemId, cancellationToken);

        if (todoItem is null)
        {
            return Result.Failure(ApplicationErrors.Todo.TodoItemNotFound);
        }

        var removeResult = todoItem.RemoveNote(request.NoteId);

        if (removeResult.IsFailure)
        {
            return Result.Failure(removeResult.Errors);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}