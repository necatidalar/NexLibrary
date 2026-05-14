using System.Net.Http.Json;
using System.Text.Json;
using NexLibrary.Contracts.Common;
using NexLibrary.Contracts.Members;

namespace NexLibrary.Web.Services;

public sealed class MemberApiService
{
    private readonly HttpClient _httpClient;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public MemberApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<PagedResponse<MemberListResponse>?> GetPagedAsync(
        int pageNumber = 1,
        int pageSize = 20,
        string? search = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var url = $"api/members?pageNumber={pageNumber}&pageSize={pageSize}";

            if (!string.IsNullOrWhiteSpace(search))
            {
                url += $"&search={Uri.EscapeDataString(search)}";
            }

            var response = await _httpClient.GetFromJsonAsync<ApiResponse<PagedResponse<MemberListResponse>>>(
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

    public async Task<MemberDetailResponse?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<MemberDetailResponse>>(
                $"api/members/{id}",
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

    public async Task<MemberDetailResponse?> CreateAsync(
        MemberCreateRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var httpResponse = await _httpClient.PostAsJsonAsync(
                "api/members",
                request,
                JsonOptions,
                cancellationToken);

            var response = await httpResponse.Content.ReadFromJsonAsync<ApiResponse<MemberDetailResponse>>(
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

    public async Task<MemberDetailResponse?> UpdateAsync(
        int id,
        MemberUpdateRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var httpResponse = await _httpClient.PutAsJsonAsync(
                $"api/members/{id}",
                request,
                JsonOptions,
                cancellationToken);

            var response = await httpResponse.Content.ReadFromJsonAsync<ApiResponse<MemberDetailResponse>>(
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