using NexLibrary.Contracts.Common;
using NexLibrary.Contracts.DynamicForms;

namespace NexLibrary.Application.Interfaces.Services;

public interface IFormFieldService
{
    Task<ApiResponse<FormDesignResponse>> GetFormDesignAsync(
        string modulKodu,
        CancellationToken cancellationToken = default);

    Task<ApiResponse<List<FormFieldResponse>>> GetByModuleAsync(
        string modulKodu,
        CancellationToken cancellationToken = default);

    Task<ApiResponse<FormFieldResponse>> CreateAsync(
        FormFieldCreateRequest request,
        CancellationToken cancellationToken = default);

    Task<ApiResponse<FormFieldResponse>> UpdateAsync(
        int id,
        FormFieldUpdateRequest request,
        CancellationToken cancellationToken = default);

    Task<ApiResponse<bool>> SetActiveAsync(
        int id,
        bool aktifMi,
        CancellationToken cancellationToken = default);
}