using Microsoft.EntityFrameworkCore;
using NotesApi.Models;

namespace NotesApi.Data;

/// <summary>
/// Entity Framework database context for the Notes API.
/// </summary>
public class NotesDbContext : DbContext
{
    public NotesDbContext(DbContextOptions<NotesDbContext> options) : base(options)
    {
    }

    /// <summary>
    /// Notes table in the database.
    /// </summary>
    public DbSet<Note> Notes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Note entity
        modelBuilder.Entity<Note>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.Content)
                .IsRequired()
                .HasMaxLength(5000);

            entity.Property(e => e.CreatedAt)
                .IsRequired();
        });
    }
}
