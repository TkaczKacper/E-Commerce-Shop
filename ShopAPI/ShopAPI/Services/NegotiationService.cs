using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ShopAPI.Data;
using ShopAPI.DataTransferObjects;
using ShopAPI.Services.Interfaces;
using ShopAPI.Models;
using ShopAPI.Models.Enums;

namespace ShopAPI.Services;

public class NegotiationService : INegotiationService 
{
    private readonly AppDbContext _context;

    public NegotiationService(AppDbContext context)
    {
        _context = context;
    }
    
    public Task<Negotiation> GetNegotiationAsync(int negotiationId)
    {
        throw new System.NotImplementedException();
    }

    public Task<IEnumerable<Negotiation>> GetNegotiationsAsync()
    {
        throw new System.NotImplementedException();
    }

    public async Task<Negotiation> StartNegotiationAsync(NegotiationDTO negotiationDto)
    {
        if (negotiationDto.ProposedPrice is <= 0)
        {
            throw new ArgumentException("Price must be greater than zero.");
        }
        
        var productId = negotiationDto.ProductId;
        var proposedPrice = (decimal)negotiationDto.ProposedPrice;
        
        var product = await _context.Products.FindAsync(negotiationDto.ProductId);

        if (product is null)
        {
            throw new KeyNotFoundException($"Product with id: {productId} not found.");
        }


        var negotiation = new Negotiation
        {
            ProductId = productId,
            ClientId = negotiationDto.ClientId,
            Status = NegotiationStatus.Pending,
            AttemptCount = 1,
            ProposedPrice = proposedPrice,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        
        _context.Negotiations.Add(negotiation);
        await _context.SaveChangesAsync();
        
        return negotiation;
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