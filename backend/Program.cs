using Microsoft.EntityFrameworkCore;
using NotesApi.Data;
using NotesApi.Repositories;
using NotesApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Configure CORS for local development
builder.Services.AddCors(options =>
{
    options.AddPolicy("LocalDevelopment", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Configure Entity Framework with In-Memory Database
builder.Services.AddDbContext<NotesDbContext>(options =>
{
    options.UseInMemoryDatabase("NotesDb");
});

// Register repositories
builder.Services.AddScoped<INotesRepository, NotesRepository>();

// Register services
builder.Services.AddScoped<INotesService, NotesService>();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseCors("LocalDevelopment");
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

// Make the Program class public for integration tests
public partial class Program { }
