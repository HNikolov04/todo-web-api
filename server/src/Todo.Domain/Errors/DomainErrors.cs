using Todo.Domain.Shared;

namespace Todo.Domain.Errors;

public static class DomainErrors
{
    // Entities
    public static class User
    {
        public static readonly Error UsernameRequired = new(
            "User.UsernameRequired",
            "Username is required.");

        public static readonly Error EmailRequired = new(
            "User.EmailRequired",
            "Email is required.");
    }

    public static class Todo
    {
        public static readonly Error TodoItemNotFound = new(
            "User.TodoItemNotFound",
            "The requested todo item was not found.");

        public static readonly Error EmptyTitle = new(
            "Todo.EmptyTitle",
            "Todo title cannot be empty.");

        public static readonly Error TitleTooLong = new(
            "Todo.TitleTooLong",
            "Todo title is too long. Maximum 100 characters allowed.");

        public static readonly Error InvalidDueDate = new(
            "Todo.InvalidDueDate",
            "Todo due date cannot be in the past.");

        public static readonly Error NoteNotFound = new(
            "Todo.NoteNotFound",
            "The requested note was not found.");
    }

    public static class Note
    {
        public static readonly Error EmptyText = new(
            "Note.EmptyText",
            "Note text cannot be empty.");

        public static readonly Error TooLong = new(
            "Note.TooLong",
            "Note text exceeds the maximum length of 500 characters.");
    }

    public static class ChangeLogEntry
    {
        public static readonly Error EmptyText = new(
            "ChangeLogEntry.EmptyText",
            "Change log description cannot be empty.");

        public static readonly Error TooLong = new(
            "ChangeLogEntry.TooLong",
            "Change log description exceeds the maximum length of 300 characters.");
    }
    
    // ValueObjects
    public static class FirstName
    {
        public static readonly Error Empty = new(
            "FirstName.Empty",
            "First name is empty");

        public static readonly Error TooLong = new(
            "LastName.TooLong",
            "FirstName name is too long");
    }

    public static class LastName
    {
        public static readonly Error Empty = new(
            "LastName.Empty",
            "Last name is empty");

        public static readonly Error TooLong = new(
            "LastName.TooLong",
            "Last name is too long");
    }

    public static class Address
    {
        public static readonly Error Empty = new(
            "Address.Empty",
            "Address is empty");

        public static readonly Error TooLong = new(
            "Address.TooLong",
            "Address is too long");
    }
}