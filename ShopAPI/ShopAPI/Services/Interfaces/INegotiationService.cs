using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using ShopAPI.DataTransferObjects;
using ShopAPI.Models;

namespace ShopAPI.Services.Interfaces;

public interface INegotiationService
{
    Task<Negotiation> GetNegotiationAsync(int negotiationId);
    Task<IEnumerable<Negotiation>> GetNegotiationsAsync();
    Task<Negotiation> StartNegotiationAsync(NegotiationDTO negotiationDto);
    Task<Negotiation> ProposeNewPrice(int negotiationId, ProposeNewPriceDTO proposeNewPriceDto);
    Task<string> RespondToNegotiation(int negotiationId, RespondToNegotiationDTO respondToNegotiationDto);
    Task<string> CancelNegotiationAsync(int negotiationId);
}