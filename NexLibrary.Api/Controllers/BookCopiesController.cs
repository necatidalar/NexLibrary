using Microsoft.AspNetCore.Mvc;
using NexLibrary.Application.Interfaces.Services;
using NexLibrary.Contracts.BookCopies;

namespace NexLibrary.Api.Controllers;

[ApiController]
[Route("api/book-copies")]
public sealed class BookCopiesController : ControllerBase
{
    private readonly IBookCopyService _bookCopyService;

    public BookCopiesController(IBookCopyService bookCopyService)
    {
        _bookCopyService = bookCopyService;
    }

    [HttpGet("book/{kitapId:int}")]
    public async Task<IActionResult> GetByBookId(
        int kitapId,
        CancellationToken cancellationToken = default)
    {
        var result = await _bookCopyService.GetByBookIdAsync(kitapId, cancellationToken);

        return result.BasariliMi ? Ok(result) : BadRequest(result);
    }

    [HttpGet("book/{kitapId:int}/available")]
    public async Task<IActionResult> GetAvailableByBookId(
        int kitapId,
        CancellationToken cancellationToken = default)
    {
        var result = await _bookCopyService.GetAvailableByBookIdAsync(kitapId, cancellationToken);

        return result.BasariliMi ? Ok(result) : BadRequest(result);
    }

    [HttpGet("stock-summary")]
    public async Task<IActionResult> GetStockSummary(CancellationToken cancellationToken = default)
    {
        var result = await _bookCopyService.GetStockSummaryAsync(cancellationToken);

        return result.BasariliMi ? Ok(result) : BadRequest(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] BookCopyCreateRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await _bookCopyService.CreateAsync(request, cancellationToken);

        return result.BasariliMi ? Ok(result) : BadRequest(result);
    }
}