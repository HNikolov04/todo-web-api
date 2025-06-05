using Todo.Domain.Shared;

namespace Todo.Application.Abstractions.Services.NotificationService;

public interface IEmailNotificationService
{
    Result<Task> SendEmailAsync();
}