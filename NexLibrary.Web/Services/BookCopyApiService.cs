using System.Net.Http.Json;
using System.Text.Json;
using NexLibrary.Contracts.BookCopies;
using NexLibrary.Contracts.Common;

namespace NexLibrary.Web.Services;

public sealed class BookCopyApiService
{
    private readonly HttpClient _httpClient;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public BookCopyApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<BookCopyStockSummaryResponse>> GetStockSummaryAsync(
        CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<List<BookCopyStockSummaryResponse>>>(
                "api/book-copies/stock-summary",
                JsonOptions,
                cancellationToken);

            if (response is null || !response.BasariliMi || response.Veri is null)
            {
                return new List<BookCopyStockSummaryResponse>();
            }

            return response.Veri;
        }
        catch
        {
            return new List<BookCopyStockSummaryResponse>();
        }
    }

    public async Task<List<BookCopyListResponse>> GetByBookIdAsync(
        int kitapId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<List<BookCopyListResponse>>>(
                $"api/book-copies/book/{kitapId}",
                JsonOptions,
                cancellationToken);

            if (response is null || !response.BasariliMi || response.Veri is null)
            {
                return new List<BookCopyListResponse>();
            }

            return response.Veri;
        }
        catch
        {
            return new List<BookCopyListResponse>();
        }
    }

    public async Task<List<BookCopyListResponse>> GetAvailableByBookIdAsync(
        int kitapId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<List<BookCopyListResponse>>>(
                $"api/book-copies/book/{kitapId}/available",
                JsonOptions,
                cancellationToken);

            if (response is null || !response.BasariliMi || response.Veri is null)
            {
                return new List<BookCopyListResponse>();
            }

            return response.Veri;
        }
        catch
        {
            return new List<BookCopyListResponse>();
        }
    }

    public async Task<BookCopyListResponse?> CreateAsync(
        BookCopyCreateRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var httpResponse = await _httpClient.PostAsJsonAsync(
                "api/book-copies",
                request,
                JsonOptions,
                cancellationToken);

            var response = await httpResponse.Content.ReadFromJsonAsync<ApiResponse<BookCopyListResponse>>(
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