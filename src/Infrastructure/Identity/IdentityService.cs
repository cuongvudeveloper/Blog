using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Blog.Application.Common.Interfaces;
using Blog.Application.Common.Models;
using Blog.Application.Oauth.Commands.Info;
using Blog.Domain.Entities;
using Blog.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Blog.Infrastructure.Identity;

public class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IUserClaimsPrincipalFactory<ApplicationUser> _userClaimsPrincipalFactory;
    private readonly IAuthorizationService _authorizationService;
    private readonly JwtSettings _jwtSettings;

    public IdentityService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IUserClaimsPrincipalFactory<ApplicationUser> userClaimsPrincipalFactory,
        IAuthorizationService authorizationService,
        IOptions<JwtSettings> jwtSettings)
    {
        _userManager = userManager;
        _signInManager = signInManager;
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

    public async Task<(string, ResultStatus)> LoginAsync(string email, string password)
    {
        JwtSecurityTokenHandler handler = new();
        ApplicationUser? user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            return (string.Empty, ResultStatus.NotFound);
        }
        SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, password, true);

        if (result.IsNotAllowed)
        {
            return (string.Empty, ResultStatus.IsNotAllowed);
        }

        if (result.IsLockedOut)
        {
            return (string.Empty, ResultStatus.IsLockedOut);
        }

        if (!result.Succeeded)
        {
            return (string.Empty, ResultStatus.Failure);
        }

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
        return (handler.WriteToken(accessToken), ResultStatus.Success);
    }

    public async Task<InfoResponse> InfoAsync(Guid userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        _ = Guard.Against.Null(user);
        var roles = await _userManager.GetRolesAsync(user);
        return new InfoResponse()
        {
            UserName = user.UserName!,
            Email = user.Email!,
            Role = roles.ToArray()
        };
    }
}
