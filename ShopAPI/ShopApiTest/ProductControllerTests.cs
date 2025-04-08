using System.Net;
using System.Text;
using System.Text.Json;
using FluentAssertions;

namespace ShopApiTest;

public class ProductControllerTests : IClassFixture<ShopApiApplicationFactory>
{
    [Fact]
    public async Task AddProduct()
    {
        var client = new ShopApiApplicationFactory().CreateClient();

        var requestBody = new
        {
            name = "string",
            price = 123
        };

        var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
        
        var response = await client.PostAsync("/api/v1/product", content);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }
}