using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace ShopApiTest;

public class ProductControllerTests : IClassFixture<ShopApiApplicationFactory>
{
    private readonly HttpClient _client;
    public ProductControllerTests(ShopApiApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }
    
    [Fact]
    public async Task AddProduct_Success()
    {
        var requestBody = new
        {
            name = "string",
            price = 123
        };

        var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
        
        var response = await _client.PostAsync("/api/v1/product", content);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task AddProduct_BadRequest()
    {
        var requestBody = new
        {
            name = "string",
            price = -123
        };
        
        var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
        var response = await _client.PostAsync("/api/v1/product", content);
        
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetProducts_Success()
    {
        var response = await _client.GetAsync("/api/v1/product");
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetProduct_Success()
    {
        await AddProduct_Success();
        var response = await _client.GetAsync("/api/v1/product/1");
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetProduct_NotFound()
    {
        var response = await _client.GetAsync("/api/v1/product/10000");
        
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task UpdateProduct_Success()
    {
        await AddProduct_Success();
        var requestBody = new
        {
            name = "string",
            price = 123
        };
        
        var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
        var response = await _client.PatchAsync("/api/v1/product/1", content);
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task UpdateProduct_BadRequest()
    {
        await AddProduct_Success();

        var requestBody = new
        {
            name = "string",
            price = -123
        };
        var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
        var response = await _client.PatchAsync("/api/v1/product/1", content);
        
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task UpdateProduct_NotFound()
    {
        var requestBody = new
        {
            name = "string",
            price = 123
        };
        var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
        var response = await _client.PatchAsync("/api/v1/product/10000", content); 
        
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteProduct_Success()
    {
        await AddProduct_Success();
        
        var response = await _client.DeleteAsync("/api/v1/product/1");
        
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task DeleteProduct_NotFound()
    {
        var response = await _client.DeleteAsync("/api/v1/product/10000");
        
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}