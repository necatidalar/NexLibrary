using Microsoft.AspNetCore.Mvc;
using NexLibrary.Api.Security;
using NexLibrary.Application.Interfaces.Services;
using NexLibrary.Contracts.Permissions;
using NexLibrary.Contracts.Users;

namespace NexLibrary.Api.Controllers;

[ApiController]
[Route("api/users")]
public sealed class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    [PermissionAuthorize(PermissionCodes.UsersView)]
    public async Task<IActionResult> GetPaged(
        int pageNumber = 1,
        int pageSize = 20,
        string? search = null,
        CancellationToken cancellationToken = default)
    {
        var result = await _userService.GetPagedAsync(
            pageNumber,
            pageSize,
            search,
            cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id:int}")]
    [PermissionAuthorize(PermissionCodes.UsersView)]
    public async Task<IActionResult> GetById(
        int id,
        CancellationToken cancellationToken = default)
    {
        var result = await _userService.GetByIdAsync(
            id,
            cancellationToken);

        return Ok(result);
    }

    [HttpGet("roles")]
    [PermissionAuthorize(PermissionCodes.UsersView)]
    public async Task<IActionResult> GetRoles(
        CancellationToken cancellationToken = default)
    {
        var result = await _userService.GetRolesAsync(cancellationToken);

        return Ok(result);
    }

    [HttpPost]
    [PermissionAuthorize(PermissionCodes.UsersCreate)]
    public async Task<IActionResult> Create(
        UserCreateRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await _userService.CreateAsync(
            request,
            cancellationToken);

        return Ok(result);
    }

    [HttpPut("{id:int}")]
    [PermissionAuthorize(PermissionCodes.UsersEdit)]
    public async Task<IActionResult> Update(
        int id,
        UserUpdateRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await _userService.UpdateAsync(
            id,
            request,
            cancellationToken);

        return Ok(result);
    }
}