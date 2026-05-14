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