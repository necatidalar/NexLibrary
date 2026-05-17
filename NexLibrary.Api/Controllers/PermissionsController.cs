using Microsoft.AspNetCore.Mvc;
using NexLibrary.Application.Interfaces.Services;
using NexLibrary.Contracts.Permissions;

namespace NexLibrary.Api.Controllers;

[ApiController]
[Route("api/permissions")]
public sealed class PermissionsController : ControllerBase
{
    private readonly IPermissionService _permissionService;

    public PermissionsController(IPermissionService permissionService)
    {
        _permissionService = permissionService;
    }

    [HttpGet("user/{kullaniciId:int}")]
    public async Task<IActionResult> GetUserPermissions(
        int kullaniciId,
        CancellationToken cancellationToken = default)
    {
        var result = await _permissionService.GetUserPermissionsAsync(
            kullaniciId,
            cancellationToken);

        return Ok(result);
    }

    [HttpGet("roles/{rolId:int}/matrix")]
    public async Task<IActionResult> GetRolePermissionMatrix(
        int rolId,
        CancellationToken cancellationToken = default)
    {
        var result = await _permissionService.GetRolePermissionMatrixAsync(
            rolId,
            cancellationToken);

        return Ok(result);
    }

    [HttpPut("roles/{rolId:int}")]
    public async Task<IActionResult> UpdateRolePermissions(
        int rolId,
        RolePermissionUpdateRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await _permissionService.UpdateRolePermissionsAsync(
            rolId,
            request,
            cancellationToken);

        return Ok(result);
    }
}