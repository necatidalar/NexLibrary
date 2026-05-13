using Microsoft.AspNetCore.Mvc;
using NexLibrary.Application.Interfaces.Services;
using NexLibrary.Contracts.Loans;

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
    public async Task<IActionResult> GetById(
        int id,
        CancellationToken cancellationToken = default)
    {
        var result = await _loanService.GetByIdAsync(id, cancellationToken);

        return result.BasariliMi ? Ok(result) : NotFound(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] LoanCreateRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await _loanService.CreateAsync(request, cancellationToken);

        return result.BasariliMi ? Ok(result) : BadRequest(result);
    }

    [HttpPatch("{id:int}/return")]
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
    public async Task<IActionResult> Cancel(
        int id,
        CancellationToken cancellationToken = default)
    {
        var result = await _loanService.CancelAsync(id, cancellationToken);

        return result.BasariliMi ? Ok(result) : BadRequest(result);
    }

    [HttpPatch("mark-overdue")]
    public async Task<IActionResult> MarkOverdue(CancellationToken cancellationToken = default)
    {
        var result = await _loanService.MarkOverdueAsync(cancellationToken);

        return result.BasariliMi ? Ok(result) : BadRequest(result);
    }
}