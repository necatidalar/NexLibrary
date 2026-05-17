using System.Net.Http.Json;
using System.Text.Json;
using NexLibrary.Contracts.Common;
using NexLibrary.Contracts.Users;

namespace NexLibrary.Web.Services;

public sealed class UserApiService
{
    private readonly HttpClient _httpClient;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public UserApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<PagedResponse<UserListResponse>?> GetPagedAsync(
        int pageNumber = 1,
        int pageSize = 20,
        string? search = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var url = $"api/users?pageNumber={pageNumber}&pageSize={pageSize}";

            if (!string.IsNullOrWhiteSpace(search))
            {
                url += $"&search={Uri.EscapeDataString(search)}";
            }

            var response = await _httpClient.GetFromJsonAsync<ApiResponse<PagedResponse<UserListResponse>>>(
                url,
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

    public async Task<UserDetailResponse?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<UserDetailResponse>>(
                $"api/users/{id}",
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

    public async Task<List<RoleResponse>> GetRolesAsync(
        CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<List<RoleResponse>>>(
                "api/users/roles",
                JsonOptions,
                cancellationToken);

            return response is null || !response.BasariliMi || response.Veri is null
                ? new List<RoleResponse>()
                : response.Veri;
        }
        catch
        {
            return new List<RoleResponse>();
        }
    }

    public async Task<UserDetailResponse?> CreateAsync(
        UserCreateRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var httpResponse = await _httpClient.PostAsJsonAsync(
                "api/users",
                request,
                JsonOptions,
                cancellationToken);

            var response = await httpResponse.Content.ReadFromJsonAsync<ApiResponse<UserDetailResponse>>(
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

    public async Task<UserDetailResponse?> UpdateAsync(
        int id,
        UserUpdateRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var httpResponse = await _httpClient.PutAsJsonAsync(
                $"api/users/{id}",
                request,
                JsonOptions,
                cancellationToken);

            var response = await httpResponse.Content.ReadFromJsonAsync<ApiResponse<UserDetailResponse>>(
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