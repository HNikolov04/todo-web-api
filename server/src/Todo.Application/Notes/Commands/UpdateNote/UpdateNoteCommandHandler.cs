using Todo.Application.Abstractions.CQRS;
using Todo.Domain.Errors;
using Todo.Domain.Repositories;
using Todo.Domain.Shared;
using Todo.Domain.ValueObjects.Note;

namespace Todo.Application.Notes.Commands.UpdateNote;

public sealed class UpdateNoteCommandHandler : ICommandHandler<UpdateNoteCommand>
{
    private readonly ITodoItemRepository _todoItemRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateNoteCommandHandler(
        ITodoItemRepository todoItemRepository,
        IUnitOfWork unitOfWork)
    {
        _todoItemRepository = todoItemRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateNoteCommand request, CancellationToken cancellationToken)
    {
        var todoItem = await _todoItemRepository.GetByIdAsync(request.TodoItemId, cancellationToken);

        if (todoItem is null)
        {
            return Result.Failure(ApplicationErrors.Todo.TodoItemNotFound);
        }

        var textResult = NoteText.Create(request.Text);

        if (textResult.IsFailure)
        {
            return Result.Failure(textResult.Errors);
        }

        var updateResult = todoItem.UpdateNoteText(request.NoteId, textResult.Value);
        
        if (updateResult.IsFailure)
        {
            return Result.Failure(updateResult.Errors);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
