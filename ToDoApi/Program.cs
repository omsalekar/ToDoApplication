using Microsoft.EntityFrameworkCore;
using ToDoApi.Data;

var builder = WebApplication.CreateBuilder(args);

// --------------------
// Services
// --------------------

// PostgreSQL for Render
builder.Services.AddDbContext<ToDoDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS (Vercel → Render)
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

// Swagger in production (OK for demo/interview)
app.UseSwagger();
app.UseSwaggerUI();

// CORS
app.UseCors(policy =>
    policy.AllowAnyOrigin()
          .AllowAnyMethod()
          .AllowAnyHeader());

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();
app.Run();
