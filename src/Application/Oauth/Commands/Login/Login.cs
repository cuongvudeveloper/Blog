using Blog.Application.Common.Interfaces;
using Blog.Application.Common.Models;

namespace Blog.Application.Oauth.Commands.Login;
public record LoginCommand : IRequest<Result<LoginResponse>>
{
    public string Email { get; init; } = null!;

    public string Password { get; init; } = null!;
}

public record LoginResponse
{
    public string AccessToken { get; init; } = null!;
}

public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<LoginResponse>>
{
    private readonly IIdentityService _identityService;

    public LoginCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<Result<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        string accessToken = await _identityService.LoginAsync(request.Email, request.Password);

        return Result<LoginResponse>.Success(new LoginResponse()
        {
            AccessToken = accessToken,
        });
    }
}
