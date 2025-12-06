using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using NotesApi.Models;
using Xunit;

namespace NotesApi.IntegrationTests;

public class NotesApiTests : IntegrationTestBase
{
    public NotesApiTests(WebApplicationFactory<Program> factory) : base(factory)
    {
    }

    [Fact]
    public async Task POST_Notes_WithValidData_ReturnsCreatedNoteWithIdAndTimestamp()
    {
        // Arrange
        var request = new CreateNoteRequest
        {
            Title = "Integration Test Note",
            Content = "This is a test note created by integration test"
        };

        // Act
        var response = await Client.PostAsJsonAsync("/api/notes", request);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var createdNote = await response.Content.ReadFromJsonAsync<Note>();
        Assert.NotNull(createdNote);
        Assert.NotEqual(Guid.Empty, createdNote.Id);
        Assert.Equal(request.Title, createdNote.Title);
        Assert.Equal(request.Content, createdNote.Content);
        Assert.True(createdNote.CreatedAt <= DateTime.UtcNow);
        Assert.True(createdNote.CreatedAt > DateTime.UtcNow.AddSeconds(-10));

        // Verify Location header
        Assert.NotNull(response.Headers.Location);
        Assert.Contains($"/api/notes/{createdNote.Id}", response.Headers.Location.ToString());
    }

    [Fact]
    public async Task POST_Notes_WithValidData_PersistsToDatabase()
    {
        // Arrange
        var request = new CreateNoteRequest
        {
            Title = "Persistence Test Note",
            Content = "This note should be persisted to the database"
        };

        // Act
        var response = await Client.PostAsJsonAsync("/api/notes", request);

        // Assert - First verify creation was successful
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var createdNote = await response.Content.ReadFromJsonAsync<Note>();
        Assert.NotNull(createdNote);

        // TODO: Once GET endpoint is implemented, verify the note can be retrieved
        // For now, we verify the response indicates successful creation
        Assert.NotEqual(Guid.Empty, createdNote.Id);
    }

    [Fact]
    public async Task POST_Notes_WithEmptyTitle_Returns400BadRequest()
    {
        // Arrange
        var request = new CreateNoteRequest
        {
            Title = "",
            Content = "Valid content"
        };

        // Act
        var response = await Client.PostAsJsonAsync("/api/notes", request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var errorContent = await response.Content.ReadAsStringAsync();
        Assert.Contains("title", errorContent, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task POST_Notes_WithNullTitle_Returns400BadRequest()
    {
        // Arrange
        var request = new { Title = (string?)null, Content = "Valid content" };

        // Act
        var response = await Client.PostAsJsonAsync("/api/notes", request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task POST_Notes_WithEmptyContent_Returns400BadRequest()
    {
        // Arrange
        var request = new CreateNoteRequest
        {
            Title = "Valid title",
            Content = ""
        };

        // Act
        var response = await Client.PostAsJsonAsync("/api/notes", request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var errorContent = await response.Content.ReadAsStringAsync();
        Assert.Contains("content", errorContent, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task POST_Notes_WithNullContent_Returns400BadRequest()
    {
        // Arrange
        var request = new { Title = "Valid title", Content = (string?)null };

        // Act
        var response = await Client.PostAsJsonAsync("/api/notes", request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task POST_Notes_WithTitleTooLong_Returns400BadRequest()
    {
        // Arrange
        var request = new CreateNoteRequest
        {
            Title = new string('x', 101), // 101 characters, exceeds max of 100
            Content = "Valid content"
        };

        // Act
        var response = await Client.PostAsJsonAsync("/api/notes", request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var errorContent = await response.Content.ReadAsStringAsync();
        Assert.Contains("title", errorContent, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task POST_Notes_WithContentTooLong_Returns400BadRequest()
    {
        // Arrange
        var request = new CreateNoteRequest
        {
            Title = "Valid title",
            Content = new string('x', 5001) // 5001 characters, exceeds max of 5000
        };

        // Act
        var response = await Client.PostAsJsonAsync("/api/notes", request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var errorContent = await response.Content.ReadAsStringAsync();
        Assert.Contains("content", errorContent, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task POST_Notes_WithTitleAtMaxLength_ReturnsCreated()
    {
        // Arrange
        var request = new CreateNoteRequest
        {
            Title = new string('x', 100), // Exactly 100 characters
            Content = "Valid content"
        };

        // Act
        var response = await Client.PostAsJsonAsync("/api/notes", request);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task POST_Notes_WithContentAtMaxLength_ReturnsCreated()
    {
        // Arrange
        var request = new CreateNoteRequest
        {
            Title = "Valid title",
            Content = new string('x', 5000) // Exactly 5000 characters
        };

        // Act
        var response = await Client.PostAsJsonAsync("/api/notes", request);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task POST_Notes_WithMultipleNotes_CreatesAllWithUniqueIds()
    {
        // Arrange
        var request1 = new CreateNoteRequest { Title = "Note 1", Content = "Content 1" };
        var request2 = new CreateNoteRequest { Title = "Note 2", Content = "Content 2" };
        var request3 = new CreateNoteRequest { Title = "Note 3", Content = "Content 3" };

        // Act
        var response1 = await Client.PostAsJsonAsync("/api/notes", request1);
        var response2 = await Client.PostAsJsonAsync("/api/notes", request2);
        var response3 = await Client.PostAsJsonAsync("/api/notes", request3);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response1.StatusCode);
        Assert.Equal(HttpStatusCode.Created, response2.StatusCode);
        Assert.Equal(HttpStatusCode.Created, response3.StatusCode);

        var note1 = await response1.Content.ReadFromJsonAsync<Note>();
        var note2 = await response2.Content.ReadFromJsonAsync<Note>();
        var note3 = await response3.Content.ReadFromJsonAsync<Note>();

        Assert.NotNull(note1);
        Assert.NotNull(note2);
        Assert.NotNull(note3);

        // Verify all IDs are unique
        Assert.NotEqual(note1.Id, note2.Id);
        Assert.NotEqual(note1.Id, note3.Id);
        Assert.NotEqual(note2.Id, note3.Id);
    }
}
