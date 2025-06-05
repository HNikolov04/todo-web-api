using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Todo.Application.Abstractions.Services;

namespace Todo.Infrastructure.Authentication;

public class UserContext : IUserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserContext(IHttpContextAccessor accessor)
    {
        _httpContextAccessor = accessor;
    }

    public Guid UserId
    {
        get
        {
            var user = _httpContextAccessor.HttpContext?.User;

            if (user is null || user.Identity is not { IsAuthenticated: true })
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }

            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier); // ← THIS resolves to that long claim URI

            if (userIdClaim is null)
            {
                throw new UnauthorizedAccessException("User ID claim not found.");
            }

            return Guid.Parse(userIdClaim.Value);
        }
    }
}