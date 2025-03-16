using System.ComponentModel.DataAnnotations;

namespace Warehouse.API.Data.Models.DTO_s.Requests.Authentication;

public record RefreshTokenRequest(string AccessToken, string RefreshToken);