using NotesApi.Models;

namespace NotesApi.Services;

/// <summary>
/// Service interface for note business logic operations.
/// </summary>
public interface INotesService
{
    /// <summary>
    /// Creates a new note with the specified title and content.
    /// </summary>
    /// <param name="title">The title of the note.</param>
    /// <param name="content">The content of the note.</param>
    /// <returns>The created note with generated ID and timestamp.</returns>
    Task<Note> CreateAsync(string title, string content);
}
