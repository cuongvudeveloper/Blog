using Blog.Application.Common.Models;
using Blog.Application.Oauth.Commands.Info;
using Blog.Application.Oauth.Commands.Login;
using MediatR;

namespace Blog.Web.Endpoints;

public class Oauth : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        RouteGroupBuilder unauthenticatedGroup = app.MapGroup(this);

        _ = unauthenticatedGroup.MapPost("login", Login);

        RouteGroupBuilder authenticatedGroup = app.MapGroup(this)
            .RequireAuthorization();

        _ = authenticatedGroup.MapGet("info", Info);
    }

    public async Task<Result<LoginResponse>> Login(ISender sender, LoginCommand command)
    {
        return await sender.Send(command);
    }

    public async Task<Result<InfoResponse>> Info(ISender sender)
    {
        return await sender.Send(new InfoCommand());
    }
}
