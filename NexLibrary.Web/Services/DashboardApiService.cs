using System.Net.Http.Json;
using System.Text.Json;
using NexLibrary.Contracts.Common;
using NexLibrary.Contracts.Dashboard;

namespace NexLibrary.Web.Services;

public sealed class DashboardApiService
{
    private readonly HttpClient _httpClient;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public DashboardApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<DashboardSummaryResponse?> GetSummaryAsync(
        CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<DashboardSummaryResponse>>(
                "api/dashboard/summary",
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