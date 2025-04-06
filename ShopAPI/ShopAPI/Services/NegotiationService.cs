using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ShopAPI.Data;
using ShopAPI.DataTransferObjects;
using ShopAPI.Helpers.Exceptions;
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
    
    public async Task<Negotiation> GetNegotiationAsync(int negotiationId)
    {
        var negotiation = await _context.Negotiations.FindAsync(negotiationId);

        if (negotiation == null)
            throw new KeyNotFoundException("Requested negotiation was not found.");
        
        return negotiation;
    }

    public async Task<IEnumerable<Negotiation>> GetNegotiationsAsync()
    {
        var negotiations = await _context.Negotiations.ToListAsync();
        
        return negotiations;
    }

    public async Task<Negotiation> StartNegotiationAsync(NegotiationDTO negotiationDto)
    {
        if (negotiationDto.ProposedPrice is <= 0)
        {
            throw new ArgumentException("Price must be greater than zero.");
        }
        
        var productId = negotiationDto.ProductId;
        var proposedPrice = (decimal)negotiationDto.ProposedPrice;
        var clientId = negotiationDto.ClientId;

        var negotiation = await _context.Negotiations.FirstOrDefaultAsync(n =>
            n.ProductId == productId &&
            n.ClientId == clientId);
        
        if (negotiation is not null)
            throw new ConflictException($"You already started negotiation for this product. Check negotiation id: {negotiation.Id}");
        
        var product = await _context.Products.FindAsync(negotiationDto.ProductId);

        if (product is null)
        {
            throw new KeyNotFoundException($"Product with id: {productId} not found.");
        }


        var newNegotiation = new Negotiation
        {
            ProductId = productId,
            ClientId = negotiationDto.ClientId,
            Status = NegotiationStatus.Pending,
            AttemptCount = 1,
            ProposedPrice = proposedPrice,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        
        _context.Negotiations.Add(newNegotiation);
        await _context.SaveChangesAsync();
        
        return newNegotiation;
    }

    public Task<Negotiation> ProposeNewPrice(NegotiationDTO negotiationDto)
    {
        throw new System.NotImplementedException();
    }

    public Task<Negotiation> RespondToNegotiation(int negotiationId, bool accepted)
    {
        throw new System.NotImplementedException();
    }

    public Task<Negotiation> CancelNegotiationAsync(NegotiationDTO negotiationDto)
    {
        throw new System.NotImplementedException();
    }
}