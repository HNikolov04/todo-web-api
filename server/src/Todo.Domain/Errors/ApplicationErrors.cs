using Todo.Domain.Shared;

namespace Todo.Domain.Errors;

public static class ApplicationErrors
{
    public static class Todo
    {
        public static readonly Error NoItemsFound = new(
            "Todo.NoItemsFound",
            "No todo items were found for the current user.");

        public static readonly Error TodoItemNotFound = new(
            "Todo.TodoItemNotFound",
            "The specified todo item was not found.");
    }

    public static class Note
    {
        public static readonly Error NoteNotFound = new(
            "Todo.NoteNotFound",
            "The specified note was not found.");
    }

    public static class User
    {
        public static readonly Error InvalidCredentials = new(
            "User.InvalidCredentials",
            "Invalid email or password.");

        public static readonly Error NotFound = new(
            "User.NotFound",
            "User could not be found.");

        public static readonly Error DeleteFailed = new(
            "User.DeleteFailed",
            "The user could not be deleted.");

        public static readonly Error UpdateFailed = new(
            "User.UpdateFailed",
            "Failed to update the user.");
    }

    public static class Auth
    {
        public static readonly Error Unauthorized = new(
            "Auth.Unauthorized",
            "You are not authorized to perform this action.");
    }

}