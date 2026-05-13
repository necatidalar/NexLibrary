using NexLibrary.Contracts.Dashboard;

namespace NexLibrary.Desktop.Services;

public sealed class DashboardApiService
{
    private readonly ApiClient _apiClient;

    public DashboardApiService(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<DashboardSummaryResponse?> GetSummaryAsync(
        CancellationToken cancellationToken = default)
    {
        var response = await _apiClient.GetAsync<DashboardSummaryResponse>(
            "api/dashboard/summary",
            cancellationToken);

        return response is null || !response.BasariliMi
            ? null
            : response.Veri;
    }
}