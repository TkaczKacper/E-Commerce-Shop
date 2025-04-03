namespace ShopAPI.Services.Interfaces;

public interface ITokenService
{
    string GenerateAccessToken(string username, string password);
    string ValidateAccessToken(string token);
}