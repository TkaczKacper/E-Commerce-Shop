using System.Net;
using System.Text;
using System.Text.Json;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using ShopAPI.Data;
using ShopAPI.Models;
using ShopAPI.Models.Enums;

namespace ShopApiTest;

public class NegotiationControllerTests : IClassFixture<ShopApiApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly AppDbContext _dbContext;

    public NegotiationControllerTests(ShopApiApplicationFactory factory)
    {
        _client = factory.CreateClient();

        var scope = factory.Services.CreateScope();
        _dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    }

    private int AddProduct()
    {
        var product = new Product
        {
            Name = "Test Product",
            Price = 12
        };
        
        _dbContext.Products.Add(product);
        _dbContext.SaveChanges();
        
        return product.Id;
    }

    private int StartNegotiation(int productId, decimal price)
    {
        var negotiation = new Negotiation
        {
            ProductId = productId,
            ClientId = "test",
            AttemptCount = 0,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            ProposedPrice = price,
            Status = NegotiationStatus.Pending,
        };
        
        _dbContext.Negotiations.Add(negotiation);
        _dbContext.SaveChanges();
        
        return negotiation.Id;
    }

    private int RespondableNegotiation(int productId, decimal price)
    {
        var negotiation = new Negotiation
        {
            ProductId = productId,
            ClientId = "test",
            AttemptCount = 0,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            ProposedPrice = price,
            Status = NegotiationStatus.Rejected,
        };
        
        _dbContext.Negotiations.Add(negotiation);
        _dbContext.SaveChanges();
        
        return negotiation.Id; 
    }
    
    [Fact]
    public async Task StartNegotiation_Success()
    {
        var productId = AddProduct();
        
        var requestBody = new
        {
            proposed_price = 123
        };
        
        var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
        var response = await _client.PostAsync($"/api/v1/negotiation/{productId}", content);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task StartNegotiation_BadRequest()
    {
        var productId = AddProduct();
        
        var requestBody = new
        {
            proposed_price = -123
        };
        
        var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
        var response = await _client.PostAsync($"/api/v1/negotiation/{productId}", content);
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
        var productId = AddProduct();
        
        var requestBody = new
        {
            proposed_price = 123
        };
        
        var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
        var response = await _client.PostAsync($"/api/v1/negotiation/{productId}", content);
        
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        
        var secondResponse = await _client.PostAsync($"/api/v1/negotiation/{productId}", content);
        
        secondResponse.StatusCode.Should().Be(HttpStatusCode.Conflict);
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
        var productId = AddProduct();
        var negotiationId = StartNegotiation(productId, 456);
        
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
        var productId = AddProduct();
        var negotiationId = StartNegotiation(productId, 456);
        
        var requestBody = new
        {
            accept = true
        };
        
        var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
        var response = await _client.PatchAsync($"/api/v1/negotiation/{negotiationId}/respond", content);
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task RespondToNegotiation_NotFound()
    {
        var requestBody = new
        {
            accept = false
        };
        
        var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
        var response = await _client.PatchAsync("/api/v1/negotiation/212123123/respond", content);
        
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task RespondToNegotiation_Gone()
    {
        var negotiationId = StartNegotiation(AddProduct(), 456);

        var requestBody = new
        {
            accept = true
        };
        
        var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
        
        var cancelResponse = await _client.PatchAsync($"/api/v1/negotiation/{negotiationId}/cancel", null);
        
        cancelResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var response = await _client.PatchAsync($"/api/v1/negotiation/{negotiationId}/respond", content);
        response.StatusCode.Should().Be(HttpStatusCode.Gone);
    }

    [Fact]
    public async Task ProposeNewPrice_Success()
    {
        var negotiationId = RespondableNegotiation(AddProduct(), 456);

        var requestBody = new
        {
            new_price = 123123
        };
        
        var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
        var response = await _client.PatchAsync($"/api/v1/negotiation/{negotiationId}/propose", content);
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task ProposeNewPrice_BadRequest()
    {
        var negotiationId = RespondableNegotiation(AddProduct(), 456);

        var requestBody = new
        {
            new_price = -123
        };
        
        var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
        var response = await _client.PatchAsync($"/api/v1/negotiation/{negotiationId}/propose", content);
        
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task ProposeNewPrice_NotFound()
    {
        var requestBody = new
        {
            new_price = 123
        };
        
        var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
        var response = await _client.PatchAsync("/api/v1/negotiation/123123123123123/propose", content);
        
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task ProposeNewPrice_Conflict()
    {
        var negotiationId = RespondableNegotiation(AddProduct(), 456);

        var requestBody = new
        {
            new_price = 123
        };
        
        var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
        
        var firstResponse = await _client.PatchAsync($"/api/v1/negotiation/{negotiationId}/propose", content);
        firstResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var secondResponse = await _client.PatchAsync($"/api/v1/negotiation/{negotiationId}/propose", content);
        secondResponse.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task ProposeNewPrice_Gone()
    {
        var negotiationId = RespondableNegotiation(AddProduct(), 456);

        var requestBody = new
        {
            new_price = 123
        };
        
        var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
        
        var cancelResponse = await _client.PatchAsync($"/api/v1/negotiation/{negotiationId}/cancel", null);
        cancelResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var response = await _client.PatchAsync($"/api/v1/negotiation/{negotiationId}/propose", content);
        response.StatusCode.Should().Be(HttpStatusCode.Gone);
    }

    [Fact]
    public async Task ProposeNewPrice_UnprocessableEntity()
    {
        var negotiationId = StartNegotiation(AddProduct(), 456);
        
        var requestBody = new
        {
            accept = true 
        };
        
        var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
        var response = await _client.PatchAsync($"/api/v1/negotiation/{negotiationId}/respond", content);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var secondRequestBody = new
        {
            new_price = 123
        };
        
        var secondContent = new StringContent(JsonSerializer.Serialize(secondRequestBody), Encoding.UTF8, "application/json");
        var secondResponse = await _client.PatchAsync($"/api/v1/negotiation/{negotiationId}/propose", secondContent);
        
        secondResponse.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
    }

    [Fact]
    public async Task CancelNegotiation_Success()
    {
        var negotiationId = RespondableNegotiation(AddProduct(), 456);
        
        var response = await _client.PatchAsync($"/api/v1/negotiation/{negotiationId}/cancel", null);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task CancelNegotiation_NotFound()
    {
        var response = await _client.PatchAsync("/api/v1/negotiation/123123123123123/cancel", null);
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task CancelNegotiation_Gone()
    {
        var negotiationId = StartNegotiation(AddProduct(), 456);
        
        var response = await _client.PatchAsync($"/api/v1/negotiation/{negotiationId}/cancel", null);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var secondResponse = await _client.PatchAsync($"/api/v1/negotiation/{negotiationId}/cancel", null);
        secondResponse.StatusCode.Should().Be(HttpStatusCode.Gone);
    }
}