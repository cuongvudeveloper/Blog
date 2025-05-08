using Ardalis.GuardClauses;
using Blog.Application.Common.Interfaces;
using Blog.Application.Common.Models;
using Blog.Domain.Entities;

namespace Blog.Application.Oauth.Commands.Info;
public record InfoCommand : IRequest<Result<InfoResponse>>
{
}

public record InfoResponse
{
    public ApplicationUser? User { get; init; } = null!;
}

public class InfoCommandHandler : IRequestHandler<InfoCommand, Result<InfoResponse>>
{
    private readonly IUser _user;
    private readonly IIdentityService _identityService;

    public InfoCommandHandler(IUser user, IIdentityService identityService)
    {
        _user = user;
        _identityService = identityService;
    }

    public async Task<Result<InfoResponse>> Handle(InfoCommand request, CancellationToken cancellationToken)
    {
        _ = Guard.Against.Null(_user.Id);

        ApplicationUser? user = await _identityService.InfoAsync(_user.Id ?? Guid.Empty);

        return Result<InfoResponse>.Success(new InfoResponse()
        {
            User = user,
        });
    }
}
