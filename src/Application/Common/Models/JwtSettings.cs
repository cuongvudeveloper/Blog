namespace Blog.Application.Common.Models;
public class JwtSettings
{
    public string ValidIssuer { get; init; } = null!;
    public string ValidAudience { get; init; } = null!;
    public string SigningKey { get; init; } = null!;
    public int ExpiryInMinutes { get; init; }
}
