using Microsoft.EntityFrameworkCore;
using ToDoApi.Data;

var builder = WebApplication.CreateBuilder(args);

// ---- Database ----
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new Exception("Database connection string is missing");
}

builder.Services.AddDbContext<ToDoDbContext>(options =>
    options.UseNpgsql(connectionString));

// ---- Services ----
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();

var app = builder.Build();

// ---- Apply migrations automatically (CRITICAL) ----
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ToDoDbContext>();
    db.Database.Migrate(); // <-- THIS CREATES TABLES
}

// ---- Render Port ----
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
app.Urls.Add($"http://*:{port}");

// ---- Middleware ----
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors(p =>
    p.AllowAnyOrigin()
     .AllowAnyMethod()
     .AllowAnyHeader());

app.UseAuthorization();
app.MapControllers();
app.Run();
