using System.Security.Claims;

namespace FindKočka.Services
{
    public interface IUserService
    {
        int? GetUserId(ClaimsPrincipal User);
    }
}