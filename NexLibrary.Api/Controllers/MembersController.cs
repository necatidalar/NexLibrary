using Microsoft.AspNetCore.Mvc;
using NexLibrary.Api.Security;
using NexLibrary.Application.Interfaces.Services;
using NexLibrary.Contracts.Members;
using NexLibrary.Contracts.Permissions;

namespace NexLibrary.Api.Controllers;

[ApiController]
[Route("api/members")]
public sealed class MembersController : ControllerBase
{
    private readonly IMemberService _memberService;

    public MembersController(IMemberService memberService)
    {
        _memberService = memberService;
    }

    [HttpGet]
    [PermissionAuthorize(PermissionCodes.MembersView)]
    public async Task<IActionResult> GetPaged(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? search = null,
        CancellationToken cancellationToken = default)
    {
        var result = await _memberService.GetPagedAsync(
            pageNumber,
            pageSize,
            search,
            cancellationToken);

        return result.BasariliMi ? Ok(result) : BadRequest(result);
    }

    [HttpGet("{id:int}")]
    [PermissionAuthorize(PermissionCodes.MembersView)]
    public async Task<IActionResult> GetById(
        int id,
        CancellationToken cancellationToken = default)
    {
        var result = await _memberService.GetByIdAsync(id, cancellationToken);

        return result.BasariliMi ? Ok(result) : NotFound(result);
    }

    [HttpPost]
    [PermissionAuthorize(PermissionCodes.MembersCreate)]
    public async Task<IActionResult> Create(
        [FromBody] MemberCreateRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await _memberService.CreateAsync(request, cancellationToken);

        return result.BasariliMi ? Ok(result) : BadRequest(result);
    }

    [HttpPut("{id:int}")]
    [PermissionAuthorize(PermissionCodes.MembersEdit)]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] MemberUpdateRequest request,
        CancellationToken cancellationToken = default)
    {
        request.Id = id;

        var result = await _memberService.UpdateAsync(id, request, cancellationToken);

        return result.BasariliMi ? Ok(result) : BadRequest(result);
    }

    [HttpDelete("{id:int}")]
    [PermissionAuthorize(PermissionCodes.MembersDelete)]
    public async Task<IActionResult> Delete(
        int id,
        CancellationToken cancellationToken = default)
    {
        var result = await _memberService.DeleteAsync(id, cancellationToken);

        return result.BasariliMi ? Ok(result) : BadRequest(result);
    }
}