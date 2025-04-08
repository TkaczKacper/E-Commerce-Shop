using System.Net;
using System.Text;
using System.Text.Json;
using FluentAssertions;

namespace ShopApiTest;

public class AuthenticationTests : IClassFixture<ShopApiApplicationFactory>
{
    [Fact]
    public async Task GetToken_GeneratesAccessToken()
    {
        var application = new ShopApiApplicationFactory();
        var client = application.CreateClient();
        
        
        var requestBody = new
        {
            username = "testuser",
            email = "testuser@test.com"
        };
        
        var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
        
        var response = await client.PostAsync("/api/v1/token", content);
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}