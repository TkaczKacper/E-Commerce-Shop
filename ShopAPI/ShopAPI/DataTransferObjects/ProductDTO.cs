using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ShopAPI.DataTransferObjects;

public class ProductDTO
{
    [Required(ErrorMessage = "Product name is required.")]
    [MinLength(3, ErrorMessage = "Product name must be at least 3 characters long.")]
    [JsonPropertyName("name")]
    public required string ProductName { get; set; }
    
    [Required(ErrorMessage = "Product price is required.")]
    [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive number.")]
    [JsonPropertyName("price")]
    public required decimal Price { get; set; }
}