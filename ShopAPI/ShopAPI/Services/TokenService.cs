using ShopAPI.Services.Interfaces;

namespace ShopAPI.Services;

public class TokenService : ITokenService
{
    public string GenerateAccessToken(string username, string password)
    {
        throw new System.NotImplementedException();
    }

    public string ValidateAccessToken(string token)
    {
        throw new System.NotImplementedException();
    }
}