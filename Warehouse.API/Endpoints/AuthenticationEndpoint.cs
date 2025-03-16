using Microsoft.AspNetCore.Mvc;
using Warehouse.API.Data.Models.DTO_s.Requests.Authentication;
using Warehouse.API.Extensions;
using Warehouse.API.Services.Interfaces;

namespace Warehouse.API.Endpoints;

public static class AuthenticationEndpoint
{
    public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("api/identity");
        group.MapPost("/login", Authenticate);
        group.MapPost("/registration", Registration);
        group.MapPost("/refresh-token", RefreshToken);
        return group;
    }

    private static async Task<IResult> Authenticate(LoginRequest model, IAuthService authService)
    {
        var response = await authService.AuthenticateAsync(model);
        return response.ToHttpResponse();
    }

    private static async Task<IResult> Registration(RegistrationRequest model, IAuthService authService)
    {
        var response = await authService.RegisterAsync(model);
        return response.ToHttpResponse();
    }

    private static async Task<IResult> RefreshToken(RefreshTokenRequest model, IAuthService authService)
    {
        var response = await authService.RefreshTokenAsync(model);
        return response.ToHttpResponse();
    }
}