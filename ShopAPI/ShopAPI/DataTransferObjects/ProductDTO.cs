using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ShopAPI.DataTransferObjects;

public class ProductDTO
{
    [Required]
    [MinLength(3, ErrorMessage = "Product name must be at least 3 characters long.")]
    [JsonPropertyName("name")]
    public required string ProductName { get; set; }
    
    [Required]
    [JsonPropertyName("price")]
    [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive number.")]
    public decimal Price { get; set; }
}