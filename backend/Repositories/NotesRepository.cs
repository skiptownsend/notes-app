using NotesApi.Data;
using NotesApi.Models;

namespace NotesApi.Repositories;

/// <summary>
/// Repository implementation for Note data access operations.
/// </summary>
public class NotesRepository : INotesRepository
{
    private readonly NotesDbContext _context;

    public NotesRepository(NotesDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc/>
    public async Task<Note> AddAsync(Note note)
    {
        _context.Notes.Add(note);
        await _context.SaveChangesAsync();
        return note;
    }
}
