using System.ComponentModel.DataAnnotations;

namespace NotesApi.Models;

/// <summary>
/// Request model for creating a new note.
/// </summary>
public class CreateNoteRequest
{
    /// <summary>
    /// Title of the note (required, max 100 characters).
    /// </summary>
    [Required(ErrorMessage = "Title is required")]
    [StringLength(100, ErrorMessage = "Title cannot exceed 100 characters")]
    public required string Title { get; set; }

    /// <summary>
    /// Content of the note (required, max 5000 characters).
    /// </summary>
    [Required(ErrorMessage = "Content is required")]
    [StringLength(5000, ErrorMessage = "Content cannot exceed 5000 characters")]
    public required string Content { get; set; }
}
