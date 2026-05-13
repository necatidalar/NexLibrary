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

            if (response is null || !response.BasariliMi)
            {
                return null;
            }

            return response.Veri;
        }
        catch
        {
            return null;
        }
    }
}