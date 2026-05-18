using System.Net.Http.Headers;
using System.Security.Claims;

namespace NexLibrary.Web.Services;

public sealed class AuthTokenHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthTokenHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var accessToken = _httpContextAccessor.HttpContext?
            .User
            .FindFirstValue("AccessToken");

        if (!string.IsNullOrWhiteSpace(accessToken))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue(
                "Bearer",
                accessToken);
        }

        return base.SendAsync(request, cancellationToken);
    }
}