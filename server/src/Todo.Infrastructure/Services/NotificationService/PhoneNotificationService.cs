using Todo.Application.Abstractions.Services.NotificationService;
using Todo.Domain.Shared;

namespace Todo.Infrastructure.Services.NotificationService;

public class PhoneNotificationService : IPhoneNotificationService
{
    public Result<Task> SendSmsAsync()
    {
        return Task.CompletedTask;
    }
}