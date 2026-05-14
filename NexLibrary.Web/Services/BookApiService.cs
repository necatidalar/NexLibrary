using System.Net.Http.Json;
using System.Text.Json;
using NexLibrary.Contracts.Books;
using NexLibrary.Contracts.Common;

namespace NexLibrary.Web.Services;

public sealed class BookApiService
{
    private readonly HttpClient _httpClient;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public BookApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<PagedResponse<BookListResponse>?> GetPagedAsync(
        int pageNumber = 1,
        int pageSize = 20,
        string? search = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var url = $"api/books?pageNumber={pageNumber}&pageSize={pageSize}";

            if (!string.IsNullOrWhiteSpace(search))
            {
                url += $"&search={Uri.EscapeDataString(search)}";
            }

            var response = await _httpClient.GetFromJsonAsync<ApiResponse<PagedResponse<BookListResponse>>>(
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

    public async Task<BookDetailResponse?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<BookDetailResponse>>(
                $"api/books/{id}",
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

    public async Task<BookDetailResponse?> CreateAsync(
        BookCreateRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var httpResponse = await _httpClient.PostAsJsonAsync(
                "api/books",
                request,
                JsonOptions,
                cancellationToken);

            var response = await httpResponse.Content.ReadFromJsonAsync<ApiResponse<BookDetailResponse>>(
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

    public async Task<BookDetailResponse?> UpdateAsync(
        int id,
        BookUpdateRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var httpResponse = await _httpClient.PutAsJsonAsync(
                $"api/books/{id}",
                request,
                JsonOptions,
                cancellationToken);

            var response = await httpResponse.Content.ReadFromJsonAsync<ApiResponse<BookDetailResponse>>(
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