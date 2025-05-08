using System.Security.Claims;

using Blog.Application.Common.Interfaces;

namespace Blog.Web.Services;

public class CurrentUser : IUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    private string? IdString => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
    public Guid? Id => IdString == null ? null : Guid.Parse(IdString);
}
