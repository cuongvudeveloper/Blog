using Blog.Domain.Entities;
using Blog.Domain.Enums;

namespace Blog.Application.Common.Interfaces;

public interface IIdentityService
{
    Task<string?> GetUserNameAsync(Guid userId);

    Task<bool> IsInRoleAsync(Guid userId, string role);

    Task<bool> AuthorizeAsync(Guid userId, string policyName);

    Task<(string, ResultStatus)> LoginAsync(string email, string password);

    Task<ApplicationUser?> InfoAsync(Guid userId);
}
