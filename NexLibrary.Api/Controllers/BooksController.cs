using Microsoft.AspNetCore.Mvc;
using NexLibrary.Api.Security;
using NexLibrary.Application.Interfaces.Services;
using NexLibrary.Contracts.Books;
using NexLibrary.Contracts.Permissions;

namespace NexLibrary.Api.Controllers;

[ApiController]
[Route("api/books")]
public sealed class BooksController : ControllerBase
{
    private readonly IBookService _bookService;

    public BooksController(IBookService bookService)
    {
        _bookService = bookService;
    }

    [HttpGet]
    [PermissionAuthorize(PermissionCodes.BooksView)]
    public async Task<IActionResult> GetPaged(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? search = null,
        CancellationToken cancellationToken = default)
    {
        var result = await _bookService.GetPagedAsync(
            pageNumber,
            pageSize,
            search,
            cancellationToken);

        return result.BasariliMi ? Ok(result) : BadRequest(result);
    }

    [HttpGet("{id:int}")]
    [PermissionAuthorize(PermissionCodes.BooksView)]
    public async Task<IActionResult> GetById(
        int id,
        CancellationToken cancellationToken = default)
    {
        var result = await _bookService.GetByIdAsync(id, cancellationToken);

        return result.BasariliMi ? Ok(result) : NotFound(result);
    }

    [HttpPost]
    [PermissionAuthorize(PermissionCodes.BooksCreate)]
    public async Task<IActionResult> Create(
        [FromBody] BookCreateRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await _bookService.CreateAsync(request, cancellationToken);

        return result.BasariliMi ? Ok(result) : BadRequest(result);
    }

    [HttpPut("{id:int}")]
    [PermissionAuthorize(PermissionCodes.BooksEdit)]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] BookUpdateRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await _bookService.UpdateAsync(id, request, cancellationToken);

        return result.BasariliMi ? Ok(result) : BadRequest(result);
    }

    [HttpDelete("{id:int}")]
    [PermissionAuthorize(PermissionCodes.BooksDelete)]
    public async Task<IActionResult> Delete(
        int id,
        CancellationToken cancellationToken = default)
    {
        var result = await _bookService.DeleteAsync(id, cancellationToken);

        return result.BasariliMi ? Ok(result) : BadRequest(result);
    }
}