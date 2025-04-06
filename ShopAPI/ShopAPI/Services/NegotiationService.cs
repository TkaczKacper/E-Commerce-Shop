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
using ArgumentException = System.ArgumentException;

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
            throw new ArgumentException("Price must be greater than zero.");
        
        var productId = negotiationDto.ProductId;
        var proposedPrice = (decimal)negotiationDto.ProposedPrice;
        var clientId = negotiationDto.ClientId;

        var negotiation = await _context.Negotiations.FirstOrDefaultAsync(n =>
            n.ProductId == productId &&
            n.ClientId == clientId);
        
        if (negotiation is not null)
            throw new ConflictException($"You already started negotiation for this product. Check negotiation id: {negotiation.Id} for more details.");
        
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

    public async Task<Negotiation> ProposeNewPrice(int negotiationId, decimal proposedPrice)
    {
        if (proposedPrice is <= 0)
            throw new ArgumentException("Price must be greater than zero.");
        
        var negotiation = await _context.Negotiations.FindAsync(negotiationId);
        
        if (negotiation is null)
            throw new KeyNotFoundException("Requested negotiation was not found.");

        if (negotiation.Status == NegotiationStatus.Pending)
            throw new ConflictException("Please wait for the response in this negotiation process.");
        if (negotiation.Status == NegotiationStatus.Canceled)
            throw new GoneException(negotiation.CancellationReason ?? "Negotiation has ben cancelled.");
        if (negotiation.Status == NegotiationStatus.Accepted)
            throw new UnprocessableContentException("Negotiation has been already accepted.");

        if ((DateTime.UtcNow - negotiation.UpdatedAt).TotalSeconds > 30)
        {
            negotiation.Status = NegotiationStatus.Canceled;
            negotiation.UpdatedAt = DateTime.UtcNow;
            negotiation.CancellationReason = "Negotiation expired.";
            await _context.SaveChangesAsync();
            
            throw new GoneException("Negotiation expired.");
        }
        
        negotiation.AttemptCount++;
        negotiation.ProposedPrice = proposedPrice;
        negotiation.UpdatedAt = DateTime.UtcNow;
        negotiation.Status = NegotiationStatus.Pending;
        
        _context.Negotiations.Update(negotiation);
        await _context.SaveChangesAsync();

        return negotiation;
    }

    public async Task<string> RespondToNegotiation(int negotiationId, bool accepted)
    {
        var negotiation = await _context.Negotiations.FindAsync(negotiationId);
        
        if (negotiation is null)
            throw new KeyNotFoundException("Requested negotiation was not found.");

        if (negotiation.Status == NegotiationStatus.Canceled)
            throw new GoneException("Negotiation has been canceled.");
        
        if (negotiation.Status != NegotiationStatus.Pending)
            throw new ConflictException("This negotiation has already been responded to.");

        var respondMessage = "";
        
        if (accepted)
        {
            negotiation.Status = NegotiationStatus.Accepted;
            respondMessage = "Accepted.";
        }
        else
        {
            if (negotiation.AttemptCount >= 3)
            {
                negotiation.CancellationReason = "Maximum number of attempts reached.";
                negotiation.Status = NegotiationStatus.Canceled;
            }
            else
            {
                negotiation.Status = NegotiationStatus.Rejected;
            }
            
            respondMessage = "Rejected.";
        }
        
        negotiation.UpdatedAt = DateTime.UtcNow;
        _context.Negotiations.Update(negotiation);
        await _context.SaveChangesAsync();
        
        return respondMessage;
    }

    public Task<Negotiation> CancelNegotiationAsync(NegotiationDTO negotiationDto)
    {
        throw new System.NotImplementedException();
    }
}