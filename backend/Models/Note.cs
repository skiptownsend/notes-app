namespace NotesApi.Models;

/// <summary>
/// Represents a note entity stored in the database.
/// </summary>
public class Note
{
    /// <summary>
    /// Unique identifier for the note.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Title of the note (max 100 characters).
    /// </summary>
    public required string Title { get; set; }

    /// <summary>
    /// Content of the note (max 5000 characters).
    /// </summary>
    public required string Content { get; set; }

    /// <summary>
    /// Timestamp when the note was created (UTC).
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
