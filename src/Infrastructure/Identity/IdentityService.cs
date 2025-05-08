using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Blog.Application.Common.Interfaces;
using Blog.Application.Common.Models;
using Blog.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Blog.Infrastructure.Identity;

public class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserClaimsPrincipalFactory<ApplicationUser> _userClaimsPrincipalFactory;
    private readonly IAuthorizationService _authorizationService;
    private readonly JwtSettings _jwtSettings;

    public IdentityService(
        UserManager<ApplicationUser> userManager,
        IUserClaimsPrincipalFactory<ApplicationUser> userClaimsPrincipalFactory,
        IAuthorizationService authorizationService,
        IOptions<JwtSettings> jwtSettings)
    {
        _userManager = userManager;
        _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
        _authorizationService = authorizationService;
        _jwtSettings = jwtSettings.Value;
    }

    public async Task<string?> GetUserNameAsync(Guid userId)
    {
        ApplicationUser? user = await _userManager.FindByIdAsync(userId.ToString());

        return user?.UserName;
    }

    public async Task<bool> IsInRoleAsync(Guid userId, string role)
    {
        ApplicationUser? user = await _userManager.FindByIdAsync(userId.ToString());

        return user != null && await _userManager.IsInRoleAsync(user, role);
    }

    public async Task<bool> AuthorizeAsync(Guid userId, string policyName)
    {
        ApplicationUser? user = await _userManager.FindByIdAsync(userId.ToString());

        if (user == null)
        {
            return false;
        }

        ClaimsPrincipal principal = await _userClaimsPrincipalFactory.CreateAsync(user);

        AuthorizationResult result = await _authorizationService.AuthorizeAsync(principal, policyName);

        return result.Succeeded;
    }

    public async Task<string> LoginAsync(string email, string password)
    {
        JwtSecurityTokenHandler handler = new();
        ApplicationUser? user = await _userManager.FindByEmailAsync(email);
        _ = Guard.Against.Null(user);

        IList<string> roles = await _userManager.GetRolesAsync(user);
        ClaimsIdentity claims = new();
        claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
        claims.AddClaim(new Claim(JwtRegisteredClaimNames.Jti, user.Id.ToString()));
        foreach (string role in roles)
        {
            claims.AddClaim(new Claim(ClaimTypes.Role, role));
        }

        SecurityTokenDescriptor tokenDescriptor = new()
        {
            Subject = claims,
            Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryInMinutes),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SigningKey)), SecurityAlgorithms.HmacSha256Signature),
            Issuer = _jwtSettings.ValidIssuer,
            Audience = _jwtSettings.ValidAudience,
        };

        SecurityToken accessToken = handler.CreateToken(tokenDescriptor);
        return handler.WriteToken(accessToken);
    }

    public async Task<ApplicationUser?> InfoAsync(Guid userId)
    {
        return await _userManager.FindByIdAsync(userId.ToString());
    }
}
