using NexLibrary.Contracts.DynamicForms;

namespace NexLibrary.Desktop.Services;

public sealed class FormFieldApiService
{
    private readonly ApiClient _apiClient;

    public FormFieldApiService(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<FormDesignResponse?> GetFormDesignAsync(
        string modulKodu,
        CancellationToken cancellationToken = default)
    {
        var response = await _apiClient.GetAsync<FormDesignResponse>(
            $"api/form-fields/design/{modulKodu}",
            cancellationToken);

        if (response is null || !response.BasariliMi)
        {
            return null;
        }

        return response.Veri;
    }

    public async Task<List<FormFieldResponse>> GetByModuleAsync(
        string modulKodu,
        CancellationToken cancellationToken = default)
    {
        var response = await _apiClient.GetAsync<List<FormFieldResponse>>(
            $"api/form-fields/module/{modulKodu}",
            cancellationToken);

        if (response is null || !response.BasariliMi || response.Veri is null)
        {
            return new List<FormFieldResponse>();
        }

        return response.Veri;
    }
}