using System.Text.Json.Serialization;

namespace ShopAPI.DataTransferObjects;

public class RespondToNegotiationDTO
{
    [JsonPropertyName("accept")]
    public bool Accept { get; set; }
}