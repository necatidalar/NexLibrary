namespace NexLibrary.Contracts.Auth;

public sealed class ApiClientTokenRequest
{
    public string ClientId { get; set; } = string.Empty;

    public string ClientSecret { get; set; } = string.Empty;
}