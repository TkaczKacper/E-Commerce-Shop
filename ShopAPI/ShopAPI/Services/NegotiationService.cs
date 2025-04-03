using System.Collections.Generic;
using System.Threading.Tasks;
using ShopAPI.DataTransferObjects;
using ShopAPI.Services.Interfaces;
using ShopAPI.Models;

namespace ShopAPI.Services;

public class NegotiationService : INegotiationService 
{
    public Task<Negotiation> GetNegotiationAsync(int negotiationId)
    {
        throw new System.NotImplementedException();
    }

    public Task<IEnumerable<Negotiation>> GetNegotiationsAsync()
    {
        throw new System.NotImplementedException();
    }

    public Task<Negotiation> StartNegotiationAsync(NegotiationDTO negotiationDto)
    {
        throw new System.NotImplementedException();
    }

    public Task<Negotiation> UpdateNegotiationAsync(NegotiationDTO negotiationDto)
    {
        throw new System.NotImplementedException();
    }

    public Task<Negotiation> CancelNegotiationAsync(NegotiationDTO negotiationDto)
    {
        throw new System.NotImplementedException();
    }
}