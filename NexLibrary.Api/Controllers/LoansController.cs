using Microsoft.AspNetCore.Mvc;
using NexLibrary.Api.Security;
using NexLibrary.Application.Interfaces.Services;
using NexLibrary.Contracts.Loans;
using NexLibrary.Contracts.Permissions;

namespace NexLibrary.Api.Controllers;

[ApiController]
[Route("api/loans")]
public sealed class LoansController : ControllerBase
{
    private readonly ILoanService _loanService;

    public LoansController(ILoanService loanService)
    {
        _loanService = loanService;
    }

    [HttpGet]
    [PermissionAuthorize(PermissionCodes.LoansView)]
    public async Task<IActionResult> GetPaged(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? search = null,
        CancellationToken cancellationToken = default)
    {
        var result = await _loanService.GetPagedAsync(
            pageNumber,
            pageSize,
            search,
            cancellationToken);

        return result.BasariliMi ? Ok(result) : BadRequest(result);
    }

    [HttpGet("overdue")]
    [PermissionAuthorize(PermissionCodes.LoansView)]
    public async Task<IActionResult> GetOverdue(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var result = await _loanService.GetOverdueAsync(
            pageNumber,
            pageSize,
            cancellationToken);

        return result.BasariliMi ? Ok(result) : BadRequest(result);
    }

    [HttpGet("{id:int}")]
    [PermissionAuthorize(PermissionCodes.LoansView)]
    public async Task<IActionResult> GetById(
        int id,
        CancellationToken cancellationToken = default)
    {
        var result = await _loanService.GetByIdAsync(id, cancellationToken);

        return result.BasariliMi ? Ok(result) : NotFound(result);
    }

    [HttpPost]
    [PermissionAuthorize(PermissionCodes.LoansCreate)]
    public async Task<IActionResult> Create(
        [FromBody] LoanCreateRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await _loanService.CreateAsync(request, cancellationToken);

        return result.BasariliMi ? Ok(result) : BadRequest(result);
    }

    [HttpPatch("{id:int}/return")]
    [PermissionAuthorize(PermissionCodes.LoansReturn)]
    public async Task<IActionResult> Return(
        int id,
        [FromBody] LoanReturnRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await _loanService.ReturnAsync(
            id,
            request,
            cancellationToken);

        return result.BasariliMi ? Ok(result) : BadRequest(result);
    }

    [HttpPatch("{id:int}/cancel")]
    [PermissionAuthorize(PermissionCodes.LoansCancel)]
    public async Task<IActionResult> Cancel(
        int id,
        CancellationToken cancellationToken = default)
    {
        var result = await _loanService.CancelAsync(id, cancellationToken);

        return result.BasariliMi ? Ok(result) : BadRequest(result);
    }

    [HttpPatch("mark-overdue")]
    [PermissionAuthorize(PermissionCodes.LoansView)]
    public async Task<IActionResult> MarkOverdue(
        CancellationToken cancellationToken = default)
    {
        var result = await _loanService.MarkOverdueAsync(cancellationToken);

        return result.BasariliMi ? Ok(result) : BadRequest(result);
    }
}