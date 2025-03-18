using Scalar.AspNetCore;
using Warehouse.API.Data.Options;
using Warehouse.API.Extensions;

var builder = WebApplication.CreateBuilder(args);
var corsPolicyName = "angularApp";
builder.Services.Configure<JwtConfiguration>(builder.Configuration.GetSection("JwtConfiguration"));


builder.Services.AddOpenApi();

builder.Services.AddAllServices(builder.Configuration, corsPolicyName);
var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("Warehouse API");
        options.WithTheme(ScalarTheme.Kepler);
    });
}

app.UseHttpsRedirection();

app.UseCors(corsPolicyName);
app.UseAuthentication();
app.UseAuthorization();
app.MapAllEndpoints();

app.Run();