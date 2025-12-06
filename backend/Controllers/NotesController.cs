using Microsoft.AspNetCore.Mvc;
using NotesApi.Models;
using NotesApi.Services;

namespace NotesApi.Controllers;

/// <summary>
/// API controller for managing notes.
/// </summary>
[ApiController]
[Route("api/notes")]
public class NotesController : ControllerBase
{
    private readonly INotesService _notesService;

    public NotesController(INotesService notesService)
    {
        _notesService = notesService;
    }

    /// <summary>
    /// Creates a new note.
    /// </summary>
    /// <param name="request">The note creation request.</param>
    /// <returns>The created note with generated ID and timestamp.</returns>
    /// <response code="201">Returns the newly created note.</response>
    /// <response code="400">If the request is invalid.</response>
    /// <response code="500">If there was an internal server error.</response>
    [HttpPost]
    [ProducesResponseType(typeof(Note), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create([FromBody] CreateNoteRequest request)
    {
        // Validate model state
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Additional validation for empty strings (ModelState doesn't catch whitespace-only)
        if (string.IsNullOrWhiteSpace(request.Title))
        {
            return BadRequest(new { error = "Title cannot be empty or whitespace" });
        }

        if (string.IsNullOrWhiteSpace(request.Content))
        {
            return BadRequest(new { error = "Content cannot be empty or whitespace" });
        }

        // Explicit length validation (ensures consistent behavior in both integration and unit test scenarios)
        if (request.Title.Length > 100)
        {
            return BadRequest(new { error = "Title cannot exceed 100 characters" });
        }

        if (request.Content.Length > 5000)
        {
            return BadRequest(new { error = "Content cannot exceed 5000 characters" });
        }

        try
        {
            var createdNote = await _notesService.CreateAsync(request.Title, request.Content);
            // Use Created with explicit location URL since GET endpoint doesn't exist yet
            return Created($"/api/notes/{createdNote.Id}", createdNote);
        }
        catch (Exception)
        {
            // TODO: Log the full exception details using ILogger for debugging
            // Security: Never expose internal exception details to clients
            return StatusCode(500, new { error = "An error occurred while creating the note" });
        }
    }
}
