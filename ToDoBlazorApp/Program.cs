using MudBlazor.Services;
using ToDoBlazorApp.Components;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

// --------------------
// Services
// --------------------

// MudBlazor
builder.Services.AddMudServices();

// HttpClient for Backend API
builder.Services.AddHttpClient("api", client =>
{
    var apiBaseUrl = builder.Configuration["ApiBaseUrl"];

    if (string.IsNullOrWhiteSpace(apiBaseUrl))
        throw new Exception("ApiBaseUrl environment variable is missing");

    client.BaseAddress = new Uri(apiBaseUrl);
});

// Razor Components (Blazor Server)
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// REQUIRED for Render proxy
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor |
        ForwardedHeaders.XForwardedProto;
});

var app = builder.Build();

// --------------------
// Render PORT binding (CRITICAL)
// --------------------
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
app.Urls.Add($"http://*:{port}");

// --------------------
// Middleware
// --------------------

// MUST be first
app.UseForwardedHeaders();

// DO NOT use HTTPS redirection on Render
// app.UseHttpsRedirection();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStaticFiles();
app.UseRouting();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
