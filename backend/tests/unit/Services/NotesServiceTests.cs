using Moq;
using NotesApi.Models;
using NotesApi.Repositories;
using NotesApi.Services;
using Xunit;

namespace NotesApi.UnitTests.Services;

public class NotesServiceTests
{
    private readonly Mock<INotesRepository> _mockRepository;
    private readonly NotesService _service;

    public NotesServiceTests()
    {
        _mockRepository = new Mock<INotesRepository>();
        _service = new NotesService(_mockRepository.Object);
    }

    [Fact]
    public async Task CreateAsync_WithValidData_CreatesNoteWithGeneratedIdAndTimestamp()
    {
        // Arrange
        var title = "Test Note";
        var content = "Test Content";
        Note? capturedNote = null;

        _mockRepository
            .Setup(r => r.AddAsync(It.IsAny<Note>()))
            .Callback<Note>(note => capturedNote = note)
            .ReturnsAsync((Note note) => note);

        // Act
        var result = await _service.CreateAsync(title, content);

        // Assert
        Assert.NotNull(result);
        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal(title, result.Title);
        Assert.Equal(content, result.Content);
        Assert.True(result.CreatedAt <= DateTime.UtcNow);
        Assert.True(result.CreatedAt > DateTime.UtcNow.AddSeconds(-5)); // Created within last 5 seconds

        _mockRepository.Verify(r => r.AddAsync(It.IsAny<Note>()), Times.Once);
        Assert.NotNull(capturedNote);
        Assert.Equal(title, capturedNote.Title);
        Assert.Equal(content, capturedNote.Content);
    }

    [Fact]
    public async Task CreateAsync_CallsRepositoryAddAsync()
    {
        // Arrange
        var title = "Test Note";
        var content = "Test Content";

        _mockRepository
            .Setup(r => r.AddAsync(It.IsAny<Note>()))
            .ReturnsAsync((Note note) => note);

        // Act
        await _service.CreateAsync(title, content);

        // Assert
        _mockRepository.Verify(
            r => r.AddAsync(It.Is<Note>(n =>
                n.Title == title &&
                n.Content == content &&
                n.Id != Guid.Empty &&
                n.CreatedAt != default
            )),
            Times.Once
        );
    }

    [Fact]
    public async Task CreateAsync_ReturnsNoteReturnedByRepository()
    {
        // Arrange
        var title = "Test Note";
        var content = "Test Content";
        var expectedNote = new Note
        {
            Id = Guid.NewGuid(),
            Title = title,
            Content = content,
            CreatedAt = DateTime.UtcNow
        };

        _mockRepository
            .Setup(r => r.AddAsync(It.IsAny<Note>()))
            .ReturnsAsync(expectedNote);

        // Act
        var result = await _service.CreateAsync(title, content);

        // Assert
        Assert.Same(expectedNote, result);
    }
}
