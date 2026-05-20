using Microsoft.AspNetCore.Mvc;
using NexLibrary.Api.Security;
using NexLibrary.Application.Interfaces.Services;
using NexLibrary.Contracts.DynamicForms;
using NexLibrary.Contracts.Permissions;

namespace NexLibrary.Api.Controllers;

[ApiController]
[Route("api/form-fields")]
public sealed class FormFieldsController : ControllerBase
{
    private readonly IFormFieldService _formFieldService;

    public FormFieldsController(IFormFieldService formFieldService)
    {
        _formFieldService = formFieldService;
    }

    [HttpGet("design/{modulKodu}")]
    [PermissionAuthorize(PermissionCodes.FormFieldsView)]
    public async Task<IActionResult> GetFormDesign(
        string modulKodu,
        CancellationToken cancellationToken = default)
    {
        var result = await _formFieldService.GetFormDesignAsync(
            modulKodu,
            cancellationToken);

        return result.BasariliMi ? Ok(result) : BadRequest(result);
    }

    [HttpGet("module/{modulKodu}")]
    [PermissionAuthorize(PermissionCodes.FormFieldsView)]
    public async Task<IActionResult> GetByModule(
        string modulKodu,
        CancellationToken cancellationToken = default)
    {
        var result = await _formFieldService.GetByModuleAsync(
            modulKodu,
            cancellationToken);

        return result.BasariliMi ? Ok(result) : BadRequest(result);
    }

    [HttpPost]
    [PermissionAuthorize(PermissionCodes.FormFieldsCreate)]
    public async Task<IActionResult> Create(
        [FromBody] FormFieldCreateRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await _formFieldService.CreateAsync(request, cancellationToken);

        return result.BasariliMi ? Ok(result) : BadRequest(result);
    }

    [HttpPut("{id:int}")]
    [PermissionAuthorize(PermissionCodes.FormFieldsEdit)]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] FormFieldUpdateRequest request,
        CancellationToken cancellationToken = default)
    {
        request.Id = id;

        var result = await _formFieldService.UpdateAsync(
            id,
            request,
            cancellationToken);

        return result.BasariliMi ? Ok(result) : BadRequest(result);
    }

    [HttpPatch("{id:int}/active")]
    [PermissionAuthorize(PermissionCodes.FormFieldsManage)]
    public async Task<IActionResult> SetActive(
        int id,
        [FromQuery] bool aktifMi,
        CancellationToken cancellationToken = default)
    {
        var result = await _formFieldService.SetActiveAsync(
            id,
            aktifMi,
            cancellationToken);

        return result.BasariliMi ? Ok(result) : BadRequest(result);
    }
}