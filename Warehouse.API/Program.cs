using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Warehouse.API.Connections;
using Warehouse.API.Data;
using Warehouse.API.Data.Options;
using Warehouse.API.Extensions;
using Warehouse.API.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);
const string corsPolicyName = "frontend-policy";

builder.Services.AddOptions<JwtConfiguration>()
    .Bind(builder.Configuration.GetSection("JwtConfiguration"))
    .Validate(o => o.SecretKey.Length >= 64, "JwtConfiguration:SecretKey не задан или короче 64 символов")
    .ValidateOnStart();

builder.Services.AddAllServices(builder.Configuration, corsPolicyName);
var app = builder.Build();

// Применяем миграции при старте, чтобы схема БД в контейнере всегда совпадала с кодом
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<DataContext>();
    await db.Database.MigrateAsync();
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("Warehouse API");
        options.WithTheme(ScalarTheme.Kepler);
    });
    app.UseHttpsRedirection();
}

app.UseDefaultFiles();
app.UseStaticFiles();
app.UseRouting();
app.UseCors(corsPolicyName);
app.UseAuthentication();
app.UseAuthorization();

app.MapAllEndpoints();
app.MapHub<ProgressHub>("/pdfUploadProgressHub");

// Короткие ссылки живут под /s/, чтобы не перехватывать маршруты SPA (/home, /paper и т.д.)
app.MapGet("/s/{shortCode}", async (string shortCode, IUrlShortenerService service) =>
    {
        var originalUrl = await service.GetOriginalUrlAsync(shortCode);
        return originalUrl is null ? Results.NotFound() : Results.Redirect(originalUrl);
    })
    .WithName("RedirectToOriginalUrl");

app.MapFallbackToFile("index.html");

app.Run();
