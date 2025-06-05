using Todo.Application.Abstractions.CQRS;
using Todo.Domain.Entities;
using Todo.Domain.Errors;
using Todo.Domain.Repositories;
using Todo.Domain.Shared;
using Todo.Domain.ValueObjects.Note;
using static System.Net.Mime.MediaTypeNames;

namespace Todo.Application.Notes.Commands.CreateNote;

public sealed class CreateNoteCommandHandler : ICommandHandler<CreateNoteCommand, Guid>
{
    private readonly ITodoItemRepository _todoItemRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateNoteCommandHandler(ITodoItemRepository todoItemRepository, IUnitOfWork unitOfWork)
    {
        _todoItemRepository = todoItemRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreateNoteCommand request, CancellationToken cancellationToken)
    {
        var todoItem = await _todoItemRepository.GetByIdAsync(request.TodoItemId, cancellationToken);

        if (todoItem is null)
        {
            return Result.Failure<Guid>(ApplicationErrors.Todo.TodoItemNotFound);
        }

        var noteTextResult = NoteText.Create(request.Text);

        if (noteTextResult.IsFailure)
        {
            return Result.Failure<Guid>(noteTextResult.Errors);
        }

        var noteId = todoItem.CreateAndAddNote(noteTextResult.Value);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(noteId);
    }
}