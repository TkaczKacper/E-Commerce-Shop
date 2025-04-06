using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ShopAPI.DataTransferObjects;

public class NegotiationDTO
{
    [Required(ErrorMessage = "You must propose new price.")]
    [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive number.")]
    [JsonPropertyName("proposed_price")]
    public decimal? ProposedPrice { get; set; }
    
    [Required(ErrorMessage = "You must indicate product in order to start negotiation.")]
    [JsonPropertyName("product_id")]
    public int ProductId { get; set; }
    
    public string? ClientId { get; set; }
}