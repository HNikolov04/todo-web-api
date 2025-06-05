using Todo.Domain.Shared;

namespace Todo.Application.Abstractions.Services.NotificationService;

public interface IPhoneNotificationService
{
    Result<Task> SendSmsAsync();
}