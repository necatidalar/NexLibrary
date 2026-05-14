using System.Net.Http.Json;
using System.Text.Json;
using NexLibrary.Contracts.Common;
using NexLibrary.Contracts.DynamicForms;

namespace NexLibrary.Web.Services;

public sealed class FormFieldApiService
{
    private readonly HttpClient _httpClient;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public FormFieldApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<FormFieldResponse>> GetByModuleAsync(
        string modulKodu,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<List<FormFieldResponse>>>(
                $"api/form-fields/module/{Uri.EscapeDataString(modulKodu)}",
                JsonOptions,
                cancellationToken);

            if (response is null || !response.BasariliMi || response.Veri is null)
            {
                return new List<FormFieldResponse>();
            }

            return response.Veri;
        }
        catch
        {
            return new List<FormFieldResponse>();
        }
    }

    public async Task<FormFieldResponse?> CreateAsync(
        FormFieldCreateRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var httpResponse = await _httpClient.PostAsJsonAsync(
                "api/form-fields",
                request,
                JsonOptions,
                cancellationToken);

            var response = await httpResponse.Content.ReadFromJsonAsync<ApiResponse<FormFieldResponse>>(
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

    public async Task<FormFieldResponse?> UpdateAsync(
        int id,
        FormFieldUpdateRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var httpResponse = await _httpClient.PutAsJsonAsync(
                $"api/form-fields/{id}",
                request,
                JsonOptions,
                cancellationToken);

            var response = await httpResponse.Content.ReadFromJsonAsync<ApiResponse<FormFieldResponse>>(
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

    public async Task<bool> SetActiveAsync(
        int id,
        bool aktifMi,
        CancellationToken cancellationToken = default)
    {
        try
        {
            using var request = new HttpRequestMessage(
                HttpMethod.Patch,
                $"api/form-fields/{id}/active?aktifMi={aktifMi.ToString().ToLowerInvariant()}");

            using var httpResponse = await _httpClient.SendAsync(request, cancellationToken);

            var response = await httpResponse.Content.ReadFromJsonAsync<ApiResponse<bool>>(
                JsonOptions,
                cancellationToken);

            return response is not null && response.BasariliMi && response.Veri;
        }
        catch
        {
            return false;
        }
    }
}