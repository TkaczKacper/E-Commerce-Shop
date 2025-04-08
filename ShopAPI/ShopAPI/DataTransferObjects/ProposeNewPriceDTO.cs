using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ShopAPI.DataTransferObjects;

public class ProposeNewPriceDTO
{
    [Required(ErrorMessage = "You must propose new price.")]
    [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive number.")]
    [JsonPropertyName("new_price")]
    public decimal NewPrice { get; set; }
}