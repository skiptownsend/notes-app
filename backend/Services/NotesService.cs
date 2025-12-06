using NotesApi.Models;
using NotesApi.Repositories;

namespace NotesApi.Services;

/// <summary>
/// Service implementation for note business logic operations.
/// </summary>
public class NotesService : INotesService
{
    private readonly INotesRepository _repository;

    public NotesService(INotesRepository repository)
    {
        _repository = repository;
    }

    /// <inheritdoc/>
    public async Task<Note> CreateAsync(string title, string content)
    {
        var note = new Note
        {
            Id = Guid.NewGuid(),
            Title = title,
            Content = content,
            CreatedAt = DateTime.UtcNow
        };

        return await _repository.AddAsync(note);
    }
}
