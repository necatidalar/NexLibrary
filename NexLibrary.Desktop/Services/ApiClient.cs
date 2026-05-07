using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using NexLibrary.Contracts.Common;

namespace NexLibrary.Desktop.Services;

public sealed class ApiClient
{
    private readonly HttpClient _httpClient;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public ApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ApiResponse<T>?> GetAsync<T>(
        string url,
        CancellationToken cancellationToken = default)
    {
        using var response = await _httpClient.GetAsync(url, cancellationToken);

        return await ReadResponseAsync<T>(response, cancellationToken);
    }

    public async Task<ApiResponse<T>?> PostAsync<T>(
        string url,
        object data,
        CancellationToken cancellationToken = default)
    {
        using var response = await _httpClient.PostAsJsonAsync(
            url,
            data,
            JsonOptions,
            cancellationToken);

        return await ReadResponseAsync<T>(response, cancellationToken);
    }

    public async Task<ApiResponse<T>?> PutAsync<T>(
        string url,
        object data,
        CancellationToken cancellationToken = default)
    {
        using var response = await _httpClient.PutAsJsonAsync(
            url,
            data,
            JsonOptions,
            cancellationToken);

        return await ReadResponseAsync<T>(response, cancellationToken);
    }

    public async Task<ApiResponse<T>?> PatchAsync<T>(
        string url,
        CancellationToken cancellationToken = default)
    {
        using var request = new HttpRequestMessage(HttpMethod.Patch, url)
        {
            Content = new StringContent(string.Empty, Encoding.UTF8, "application/json")
        };

        using var response = await _httpClient.SendAsync(request, cancellationToken);

        return await ReadResponseAsync<T>(response, cancellationToken);
    }

    public async Task<ApiResponse<T>?> DeleteAsync<T>(
        string url,
        CancellationToken cancellationToken = default)
    {
        using var response = await _httpClient.DeleteAsync(url, cancellationToken);

        return await ReadResponseAsync<T>(response, cancellationToken);
    }

    private static async Task<ApiResponse<T>?> ReadResponseAsync<T>(
        HttpResponseMessage response,
        CancellationToken cancellationToken)
    {
        var json = await response.Content.ReadAsStringAsync(cancellationToken);

        if (string.IsNullOrWhiteSpace(json))
        {
            return ApiResponse<T>.Fail("API boş cevap döndürdü.");
        }

        var result = JsonSerializer.Deserialize<ApiResponse<T>>(json, JsonOptions);

        return result ?? ApiResponse<T>.Fail("API cevabı okunamadı.");
    }
}