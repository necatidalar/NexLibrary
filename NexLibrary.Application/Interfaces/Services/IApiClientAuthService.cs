using NexLibrary.Contracts.Auth;
using NexLibrary.Contracts.Common;

namespace NexLibrary.Application.Interfaces.Services;

public interface IApiClientAuthService
{
    Task<ApiResponse<ApiClientTokenResponse>> CreateTokenAsync(
        ApiClientTokenRequest request,
        CancellationToken cancellationToken = default);
}