using Microsoft.AspNetCore.Mvc;
using Moq;
using NotesApi.Controllers;
using NotesApi.Models;
using NotesApi.Services;
using Xunit;

namespace NotesApi.UnitTests.Controllers;

public class NotesControllerTests
{
    private readonly Mock<INotesService> _mockNotesService;
    private readonly NotesController _controller;

    public NotesControllerTests()
    {
        _mockNotesService = new Mock<INotesService>();
        _controller = new NotesController(_mockNotesService.Object);
    }

    [Fact]
    public async Task Create_WithValidNote_ReturnsCreatedResult()
    {
        // Arrange
        var request = new CreateNoteRequest
        {
            Title = "Test Note",
            Content = "Test Content"
        };

        var createdNote = new Note
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Content = request.Content,
            CreatedAt = DateTime.UtcNow
        };

        _mockNotesService
            .Setup(s => s.CreateAsync(request.Title, request.Content))
            .ReturnsAsync(createdNote);

        // Act
        var result = await _controller.Create(request);

        // Assert
        var createdResult = Assert.IsType<CreatedResult>(result);
        Assert.Equal(201, createdResult.StatusCode);
        var returnedNote = Assert.IsType<Note>(createdResult.Value);
        Assert.Equal(createdNote.Id, returnedNote.Id);
        Assert.Equal(createdNote.Title, returnedNote.Title);
        Assert.Equal(createdNote.Content, returnedNote.Content);
    }

    [Fact]
    public async Task Create_WithEmptyTitle_ReturnsBadRequest()
    {
        // Arrange
        var request = new CreateNoteRequest
        {
            Title = "",
            Content = "Test Content"
        };

        // Act
        var result = await _controller.Create(request);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, badRequestResult.StatusCode);
        _mockNotesService.Verify(s => s.CreateAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task Create_WithEmptyContent_ReturnsBadRequest()
    {
        // Arrange
        var request = new CreateNoteRequest
        {
            Title = "Test Note",
            Content = ""
        };

        // Act
        var result = await _controller.Create(request);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, badRequestResult.StatusCode);
        _mockNotesService.Verify(s => s.CreateAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task Create_WithTitleTooLong_ReturnsBadRequest()
    {
        // Arrange
        var request = new CreateNoteRequest
        {
            Title = new string('x', 101), // 101 characters, exceeds max of 100
            Content = "Test Content"
        };

        // Act
        var result = await _controller.Create(request);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, badRequestResult.StatusCode);
        _mockNotesService.Verify(s => s.CreateAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task Create_WithContentTooLong_ReturnsBadRequest()
    {
        // Arrange
        var request = new CreateNoteRequest
        {
            Title = "Test Note",
            Content = new string('x', 5001) // 5001 characters, exceeds max of 5000
        };

        // Act
        var result = await _controller.Create(request);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, badRequestResult.StatusCode);
        _mockNotesService.Verify(s => s.CreateAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task Create_WhenServiceThrowsException_ReturnsInternalServerError()
    {
        // Arrange
        var request = new CreateNoteRequest
        {
            Title = "Test Note",
            Content = "Test Content"
        };

        _mockNotesService
            .Setup(s => s.CreateAsync(request.Title, request.Content))
            .ThrowsAsync(new Exception("Database error"));

        // Act
        var result = await _controller.Create(request);

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, statusCodeResult.StatusCode);
    }
}
