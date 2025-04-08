using System.Net;
using System.Text;
using System.Text.Json;
using FluentAssertions;

namespace ShopApiTest;

public class NegotiationController : IClassFixture<ShopApiApplicationFactory>
{
    private readonly HttpClient _client;

    public NegotiationController(ShopApiApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    private async Task AddProduct()
    {
        var requestBody = new
        {
            name = "string",
            price = 123
        };

        var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
        
        await _client.PostAsync("/api/v1/product", content);
    }

    private async Task StartNegotiation()
    {
        await AddProduct();
        await AddProduct();
        await AddProduct();
        await AddProduct();
        await AddProduct();

        var requestBody = new
        {
            proposed_price = 123123
        };
        
        var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
        await _client.PostAsync("/api/v1/negotiation/5", content);
    }

    [Fact]
    public async Task StartNegotiation_Success()
    {
        await AddProduct();
        var requestBody = new
        {
            proposed_price = 123
        };
        
        var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
        var response = await _client.PostAsync("/api/v1/negotiation/1", content);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task StartNegotiation_BadRequest()
    {
        await AddProduct();
        var requestBody = new
        {
            proposed_price = -123
        };
        
        var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
        var response = await _client.PostAsync("/api/v1/negotiation/1", content);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task StartNegotiation_NotFound()
    {
        var requestBody = new
        {
            proposed_price = 12123
        };
        
        var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
        var response = await _client.PostAsync("/api/v1/negotiation/10000", content);
        
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task StartNegotiation_Conflict()
    {
        await StartNegotiation();
        
        var requestBody = new
        {
            proposed_price = 123
        };
        
        var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
        var response = await _client.PostAsync("/api/v1/negotiation/1", content);
        
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task GetNegotiations_Success()
    {
        var response = await _client.GetAsync("/api/v1/negotiation");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetNegotiation_Success()
    {
        await StartNegotiation();
        
        var response = await _client.GetAsync("/api/v1/negotiation/1");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetNegotiation_NotFound()
    {
        var response = await _client.GetAsync("/api/v1/negotiation/10000");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task RespondToNegotiation_Success()
    {
        var requestBody = new
        {
            accept = true
        };
        
        var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
        var response = await _client.PostAsync("/api/v1/negotiation/1", content);
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task RespondToNegotiation_BadRequest()
    {
        var requestBody = new
        {
            proposed_price = 13
        };
        
        var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
        var response = await _client.PatchAsync("/api/v1/negotiation/5/respond", content);
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}