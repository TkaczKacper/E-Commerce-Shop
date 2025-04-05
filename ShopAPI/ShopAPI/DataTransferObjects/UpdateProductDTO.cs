using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ShopAPI.DataTransferObjects;

public class UpdateProductDTO
{
    [MinLength(3, ErrorMessage = "Product name must be at least 3 characters long.")]
    [JsonPropertyName("name")]
    public string? ProductName { get; set; }
    
    [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive number.")]
    [JsonPropertyName("price")]
    public decimal? Price { get; set; }
}