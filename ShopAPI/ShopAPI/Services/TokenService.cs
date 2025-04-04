using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ShopAPI.Services.Interfaces;

namespace ShopAPI.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;
    private readonly SymmetricSecurityKey _key;

    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Authentication:JwtBearer:SecurityKey"]));
    }
    
    public string GenerateAccessToken(string username, string email)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Email, username),
            new Claim(ClaimTypes.Role, "Employee"),
        };
        
        var credentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

        var tokenOptions = new JwtSecurityToken(
            issuer: _configuration["Authentication:JwtBearer:Issuer"],
            audience: _configuration["Authentication:JwtBearer:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(15),
            signingCredentials: credentials
        );
        
        var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        
        return tokenString;
    }

    public string ValidateAccessToken(string? token)
    {
        if (token is null) return "";

        var accessToken = token.Substring("Bearer ".Length).Trim();
        var handler = new JwtSecurityTokenHandler();

        try
        {
            handler.ValidateToken(accessToken, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _key,
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var role = jwtToken.Claims.First(x => x.Type == ClaimTypes.Role).Value;

            return role;
        }
        catch
        {
            return "";
        }
    }
}