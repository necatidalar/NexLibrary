namespace NexLibrary.Contracts.Auth;

public sealed class ApiClientTokenResponse
{
    public int ApiClientId { get; set; }

    public string ClientId { get; set; } = string.Empty;

    public string ClientName { get; set; } = string.Empty;

    public string AccessToken { get; set; } = string.Empty;

    public DateTimeOffset ExpiresAt { get; set; }

    public List<string> Yetkiler { get; set; } = new();
}