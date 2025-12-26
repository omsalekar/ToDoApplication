using Microsoft.EntityFrameworkCore;
using ToDoApi.Data;

var builder = WebApplication.CreateBuilder(args);

// --------------------
// Services
// --------------------

builder.Services.AddDbContext<ToDoDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS (required for Vercel → Render)
builder.Services.AddCors();

var app = builder.Build();

// --------------------
// Render PORT binding
// --------------------
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
app.Urls.Add($"http://*:{port}");

// --------------------
// Middleware
// --------------------

// Enable Swagger in ALL environments (important for Render)
app.UseSwagger();
app.UseSwaggerUI();

// CORS must be BEFORE MapControllers
app.UseCors(policy =>
    policy.AllowAnyOrigin()
          .AllowAnyMethod()
          .AllowAnyHeader());

// HTTPS redirection (safe on Render)
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
