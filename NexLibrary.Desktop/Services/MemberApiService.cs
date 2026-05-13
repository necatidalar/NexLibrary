using NexLibrary.Contracts.Common;
using NexLibrary.Contracts.Members;

namespace NexLibrary.Desktop.Services;

public sealed class MemberApiService
{
    private readonly ApiClient _apiClient;

    public MemberApiService(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<PagedResponse<MemberListResponse>?> GetPagedAsync(
        int pageNumber = 1,
        int pageSize = 20,
        string? search = null,
        CancellationToken cancellationToken = default)
    {
        var url = $"api/members?pageNumber={pageNumber}&pageSize={pageSize}";

        if (!string.IsNullOrWhiteSpace(search))
        {
            url += $"&search={Uri.EscapeDataString(search)}";
        }

        var response = await _apiClient.GetAsync<PagedResponse<MemberListResponse>>(
            url,
            cancellationToken);

        return response is null || !response.BasariliMi
            ? null
            : response.Veri;
    }

    public async Task<MemberDetailResponse?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var response = await _apiClient.GetAsync<MemberDetailResponse>(
            $"api/members/{id}",
            cancellationToken);

        return response is null || !response.BasariliMi
            ? null
            : response.Veri;
    }

    public async Task<MemberDetailResponse?> CreateAsync(
        MemberCreateRequest request,
        CancellationToken cancellationToken = default)
    {
        var response = await _apiClient.PostAsync<MemberDetailResponse>(
            "api/members",
            request,
            cancellationToken);

        return response is null || !response.BasariliMi
            ? null
            : response.Veri;
    }

    public async Task<MemberDetailResponse?> UpdateAsync(
        int id,
        MemberUpdateRequest request,
        CancellationToken cancellationToken = default)
    {
        var response = await _apiClient.PutAsync<MemberDetailResponse>(
            $"api/members/{id}",
            request,
            cancellationToken);

        return response is null || !response.BasariliMi
            ? null
            : response.Veri;
    }

    public async Task<bool> DeleteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var response = await _apiClient.DeleteAsync<bool>(
            $"api/members/{id}",
            cancellationToken);

        return response is not null && response.BasariliMi && response.Veri;
    }
}