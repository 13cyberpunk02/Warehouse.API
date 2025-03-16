using Warehouse.API.Data.Models.DTO_s.Requests.Authentication;
using Warehouse.API.Data.Models.Result;
using LoginRequest = Warehouse.API.Data.Models.DTO_s.Requests.Authentication.LoginRequest;

namespace Warehouse.API.Services.Interfaces;

public interface IAuthService
{
    Task<Result> AuthenticateAsync(LoginRequest request);
    Task<Result> RefreshTokenAsync(RefreshTokenRequest request);
    Task<Result> RegisterAsync(RegistrationRequest request);
}