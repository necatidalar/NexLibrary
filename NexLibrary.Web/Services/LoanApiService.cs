using System.Net.Http.Json;
using System.Text.Json;
using NexLibrary.Contracts.Common;
using NexLibrary.Contracts.Loans;

namespace NexLibrary.Web.Services;

public sealed class LoanApiService
{
    private readonly HttpClient _httpClient;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public LoanApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<PagedResponse<LoanListResponse>?> GetPagedAsync(
        int pageNumber = 1,
        int pageSize = 20,
        string? search = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var url = $"api/loans?pageNumber={pageNumber}&pageSize={pageSize}";

            if (!string.IsNullOrWhiteSpace(search))
            {
                url += $"&search={Uri.EscapeDataString(search)}";
            }

            var response = await _httpClient.GetFromJsonAsync<ApiResponse<PagedResponse<LoanListResponse>>>(
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

    public async Task<PagedResponse<LoanListResponse>?> GetOverdueAsync(
        int pageNumber = 1,
        int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var url = $"api/loans/overdue?pageNumber={pageNumber}&pageSize={pageSize}";

            var response = await _httpClient.GetFromJsonAsync<ApiResponse<PagedResponse<LoanListResponse>>>(
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

    public async Task<LoanDetailResponse?> CreateAsync(
        LoanCreateRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var httpResponse = await _httpClient.PostAsJsonAsync(
                "api/loans",
                request,
                JsonOptions,
                cancellationToken);

            var response = await httpResponse.Content.ReadFromJsonAsync<ApiResponse<LoanDetailResponse>>(
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

    public async Task<LoanDetailResponse?> ReturnAsync(
        int id,
        LoanReturnRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            using var httpRequest = new HttpRequestMessage(
                HttpMethod.Patch,
                $"api/loans/{id}/return")
            {
                Content = JsonContent.Create(request, options: JsonOptions)
            };

            using var httpResponse = await _httpClient.SendAsync(
                httpRequest,
                cancellationToken);

            var response = await httpResponse.Content.ReadFromJsonAsync<ApiResponse<LoanDetailResponse>>(
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

    public async Task<int> MarkOverdueAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            using var request = new HttpRequestMessage(
                HttpMethod.Patch,
                "api/loans/mark-overdue");

            using var httpResponse = await _httpClient.SendAsync(
                request,
                cancellationToken);

            var response = await httpResponse.Content.ReadFromJsonAsync<ApiResponse<int>>(
                JsonOptions,
                cancellationToken);

            return response is null || !response.BasariliMi
                ? 0
                : response.Veri;
        }
        catch
        {
            return 0;
        }
    }
}