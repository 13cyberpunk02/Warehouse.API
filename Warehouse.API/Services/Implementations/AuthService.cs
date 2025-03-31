using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Warehouse.API.Data.Entities;
using Warehouse.API.Data.Models.DTO_s.Requests.Authentication;
using Warehouse.API.Data.Models.DTO_s.Responses.Authentication;
using Warehouse.API.Data.Models.Error.ErrorTypes;
using Warehouse.API.Data.Models.Error.ErrorTypes.AuthErrors;
using Warehouse.API.Data.Models.Result;
using Warehouse.API.Data.Options;
using Warehouse.API.Data.Validators.AuthValidators;
using Warehouse.API.Services.Interfaces;

namespace Warehouse.API.Services.Implementations;

public class AuthService(
    UserManager<AppUser> userManager,
    RoleManager<IdentityRole> roleManager,
    IJwtService jwtService,
    IOptions<JwtConfiguration> jwtOptions,
    RegistrationRequestValidator registrationRequestValidator,
    LoginRequestValidator loginRequestValidator,
    RefreshTokenValidator refreshTokenValidator) : IAuthService
{
    private readonly JwtConfiguration _jwtConfiguration = jwtOptions.Value;
    
    public async Task<Result> AuthenticateAsync(LoginRequest request)
    {
        var modelValidation = await loginRequestValidator.ValidateAsync(request);
        if (!modelValidation.IsValid)
            return Result.Failure(ErrorsCollection.ErrorCollection(modelValidation.Errors.Select(x => x.ErrorMessage)));
        
        var user = await userManager.FindByNameAsync(request.Username);
        if(user is null)
            return Result.Failure(AuthErrors.UserNotFound);
        
        var isPasswordValid = await userManager.CheckPasswordAsync(user, request.Password);
        if(!isPasswordValid)
            return Result.Failure(AuthErrors.InvalidLoginRequest);
        
        var roles = await userManager.GetRolesAsync(user);
        var accessToken = await jwtService.GenerateJwtTokenAsync(user);
        var refreshToken = jwtService.GenerateRefreshToken();
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiry = DateTimeOffset.UtcNow.AddDays(_jwtConfiguration.RefreshTokenExpirationInDays);
        
        var result = await userManager.UpdateAsync(user);
        if(!result.Succeeded)
            return Result.Failure(ErrorsCollection.ErrorCollection(result.Errors.Select(e => e.Description)));
        
        return Result.Success(new LoginResponse(accessToken, refreshToken));
    }

    public async Task<Result> RefreshTokenAsync(RefreshTokenRequest request)
    {
        var modelValidation = await refreshTokenValidator.ValidateAsync(request);
        if (!modelValidation.IsValid)
            return Result.Failure(ErrorsCollection.ErrorCollection(modelValidation.Errors.Select(x => x.ErrorMessage)));
        
        var principal = jwtService.GetPrincipalFromExpiredToken(request.AccessToken);
        
        if(principal is null || principal.Identity is null)
            return Result.Failure(AuthErrors.ErrorRequest);
        
        var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);
        if(userId is null)
            return Result.Failure(AuthErrors.ErrorRequest);
        
        var user = await userManager.FindByIdAsync(userId);
        if(user is null)
            return Result.Failure(AuthErrors.UserNotFound);
        
        var roles = await userManager.GetRolesAsync(user);
        
        if(user.RefreshToken != request.RefreshToken)
            return Result.Failure(AuthErrors.InvalidRefreshToken);
        
        if(user.RefreshTokenExpiry < DateTimeOffset.UtcNow)
            return Result.Failure(AuthErrors.RefreshTokenExpired);
        
        var newAccessToken = await jwtService.GenerateJwtTokenAsync(user);
        var newRefreshToken = jwtService.GenerateRefreshToken();
        
        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiry = DateTimeOffset.UtcNow.AddDays(_jwtConfiguration.RefreshTokenExpirationInDays);
        
        var result = await userManager.UpdateAsync(user);
        if(!result.Succeeded)
            return Result.Failure(ErrorsCollection.ErrorCollection(result.Errors.Select(e => e.Description)));
        
        return Result.Success(new LoginResponse(newAccessToken, newRefreshToken));
    }

    public async Task<Result> RegisterAsync(RegistrationRequest request)
    {
        var modelValidation = await registrationRequestValidator.ValidateAsync(request);
        if(!modelValidation.IsValid)
            return Result.Failure(ErrorsCollection.ErrorCollection(modelValidation.Errors.Select(x => x.ErrorMessage)));
        
        var role = await roleManager.FindByNameAsync(request.RoleName);
        if(role is null)
            return Result.Failure(AuthErrors.RoleNotFound);
        
        var isUserExist = await userManager.FindByNameAsync(request.Username);
        if(isUserExist is not null)
            return Result.Failure(AuthErrors.UserAlreadyExists);

        var newUser = new AppUser
        {
            UserName = request.Username,
            Firstname = request.Firstname,
            Lastname = request.Lastname,
            DepartmentId = request.DepartmentId
        };
        
        var result = await userManager.CreateAsync(newUser, request.Password);
        if(!result.Succeeded)
            return Result.Failure(ErrorsCollection.ErrorCollection(result.Errors.Select(e => e.Description)));
        
        var addingToRole =  await userManager.AddToRoleAsync(newUser, role.Name);
        if(!addingToRole.Succeeded)
            return Result.Failure(ErrorsCollection.ErrorCollection(addingToRole.Errors.Select(e => e.Description)));
        
        return Result.Success("Регистрация прошла успешно, вы можете теперь авторизоваться");
    }
}