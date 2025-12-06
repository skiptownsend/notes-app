using NotesApi.Models;

namespace NotesApi.Repositories;

/// <summary>
/// Repository interface for Note data access operations.
/// </summary>
public interface INotesRepository
{
    /// <summary>
    /// Adds a new note to the database.
    /// </summary>
    /// <param name="note">The note to add.</param>
    /// <returns>The added note with database-generated values.</returns>
    Task<Note> AddAsync(Note note);
}
