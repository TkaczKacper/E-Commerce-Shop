using System.Net;
using System.Text;
using System.Text.Json;
using FluentAssertions;

namespace ShopApiTest;

public class AuthenticationTests : IClassFixture<ShopApiApplicationFactory>
{
    private readonly HttpClient _client;

    public AuthenticationTests(ShopApiApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }
    
    [Fact]
    public async Task GetToken_GeneratesAccessToken()
    {
        var requestBody = new
        {
            username = "testuser",
            email = "testuser@test.com"
        };
        
        var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
        
        var response = await _client.PostAsync("/api/v1/token", content);
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetToken_ReturnsBadRequest()
    {
        var response = await _client.PostAsync("/api/v1/token", null);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}