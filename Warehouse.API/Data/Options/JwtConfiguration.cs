﻿namespace Warehouse.API.Data.Options;

public class JwtConfiguration
{
    public string SecretKey { get; init; } = string.Empty; 
    public string Issuer { get; init; } = string.Empty; 
    public string Audience { get; init; } = string.Empty; 
    public int ExpirationInMinutes { get; init; } 
    public int RefreshTokenExpirationInDays { get; init; }
}