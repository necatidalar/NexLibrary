using System.Net.Http.Json;
using System.Text.Json;
using NexLibrary.Contracts.Common;
using NexLibrary.Contracts.Permissions;

namespace NexLibrary.Web.Services;

public sealed class PermissionApiService
{
    private readonly HttpClient _httpClient;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public PermissionApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<RolePermissionMatrixResponse?> GetRolePermissionMatrixAsync(
        int rolId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<RolePermissionMatrixResponse>>(
                $"api/permissions/roles/{rolId}/matrix",
                JsonOptions,
                cancellationToken);

            return response is null || !response.BasariliMi
                ? null
                : response.Veri;
        }
        catch
        {
            return null;
        }
    }

    public async Task<RolePermissionMatrixResponse?> UpdateRolePermissionsAsync(
        int rolId,
        RolePermissionUpdateRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var httpResponse = await _httpClient.PutAsJsonAsync(
                $"api/permissions/roles/{rolId}",
                request,
                JsonOptions,
                cancellationToken);

            var response = await httpResponse.Content.ReadFromJsonAsync<ApiResponse<RolePermissionMatrixResponse>>(
                JsonOptions,
                cancellationToken);

            return response is null || !response.BasariliMi
                ? null
                : response.Veri;
        }
        catch
        {
            return null;
        }
    }
}