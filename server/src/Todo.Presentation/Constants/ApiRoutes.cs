namespace Todo.Presentation.Constants;

public static class ApiRoutes
{
    public static class TodoItems
    {
        public const string Base = "api/todoitems";
        public const string GetById = "{id:guid}";
        public const string Update = "{id:guid}";
        public const string Delete = "{id:guid}";
    }

    public static class Notes
    {
        public const string Base = "api/todoitems/{todoItemId:guid}/notes";
        public const string GetById = "{id:guid}";
        public const string Update = "{id:guid}";
        public const string Delete = "{id:guid}";
    }

    public static class ChangeLogEntries
    {
        public const string Base = "api/todoitems/{todoItemId:guid}/changelogentries";
        // Add more routes here if needed (e.g., GetAll, Post, etc.)
    }

    public static class Users
    {
        public const string Base = "api/users";
        public const string Register = "register";
        public const string Login = "login";
        public const string Me = "me";
    }
}