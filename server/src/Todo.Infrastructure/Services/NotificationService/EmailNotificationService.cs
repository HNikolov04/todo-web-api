using Todo.Application.Abstractions.Services.NotificationService;
using Todo.Domain.Shared;

namespace Todo.Infrastructure.Services.NotificationService;

public class EmailNotificationService : IEmailNotificationService
{
    public Result<Task> SendEmailAsync()
    {
        return Task.CompletedTask;
    }
}