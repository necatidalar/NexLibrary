using NexLibrary.Contracts.Common;
using NexLibrary.Contracts.Loans;

namespace NexLibrary.Desktop.Services;

public sealed class LoanApiService
{
    private readonly ApiClient _apiClient;

    public LoanApiService(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<PagedResponse<LoanListResponse>?> GetPagedAsync(
        int pageNumber = 1,
        int pageSize = 20,
        string? search = null,
        CancellationToken cancellationToken = default)
    {
        var url = $"api/loans?pageNumber={pageNumber}&pageSize={pageSize}";

        if (!string.IsNullOrWhiteSpace(search))
        {
            url += $"&search={Uri.EscapeDataString(search)}";
        }

        var response = await _apiClient.GetAsync<PagedResponse<LoanListResponse>>(
            url,
            cancellationToken);

        return response is null || !response.BasariliMi
            ? null
            : response.Veri;
    }

    public async Task<PagedResponse<LoanListResponse>?> GetOverdueAsync(
        int pageNumber = 1,
        int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var response = await _apiClient.GetAsync<PagedResponse<LoanListResponse>>(
            $"api/loans/overdue?pageNumber={pageNumber}&pageSize={pageSize}",
            cancellationToken);

        return response is null || !response.BasariliMi
            ? null
            : response.Veri;
    }

    public async Task<LoanDetailResponse?> CreateAsync(
        LoanCreateRequest request,
        CancellationToken cancellationToken = default)
    {
        var response = await _apiClient.PostAsync<LoanDetailResponse>(
            "api/loans",
            request,
            cancellationToken);

        return response is null || !response.BasariliMi
            ? null
            : response.Veri;
    }

    public async Task<LoanDetailResponse?> ReturnAsync(
        int id,
        LoanReturnRequest request,
        CancellationToken cancellationToken = default)
    {
        var response = await _apiClient.PatchWithBodyAsync<LoanDetailResponse>(
            $"api/loans/{id}/return",
            request,
            cancellationToken);

        return response is null || !response.BasariliMi
            ? null
            : response.Veri;
    }

    public async Task<bool> CancelAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var response = await _apiClient.PatchAsync<bool>(
            $"api/loans/{id}/cancel",
            cancellationToken);

        return response is not null && response.BasariliMi && response.Veri;
    }

    public async Task<int> MarkOverdueAsync(CancellationToken cancellationToken = default)
    {
        var response = await _apiClient.PatchAsync<int>(
            "api/loans/mark-overdue",
            cancellationToken);

        return response is null || !response.BasariliMi
            ? 0
            : response.Veri;
    }
}