using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopAPI.DataTransferObjects;
using ShopAPI.Models;
using ShopAPI.Services.Interfaces;

namespace ShopAPI.Controllers;

[ApiController]
[Route("api/v1/negotiation")]
public class NegotiationController : ControllerBase
{
    public readonly INegotiationService _negotiationService;

    public NegotiationController(INegotiationService negotiationService)
    {
        _negotiationService = negotiationService;
    }

    [HttpPost]
    public async Task<IActionResult> StartNegotiation([FromBody] NegotiationDTO negotiationDto)
    {
        var clientId = HttpContext.Connection.RemoteIpAddress?.ToString();
        negotiationDto.ClientId = clientId;
        
        var negotiation = await _negotiationService.StartNegotiationAsync(negotiationDto);
        
        return Ok(negotiation);
    }

    [HttpPatch("{negotiationId:int}/respond")]
    [Authorize(Roles = "Employee")]
    public async Task<IActionResult> RespondToNegotiation(int negotiationId, [FromBody] bool accept)
    {
        var res = await _negotiationService.RespondToNegotiation(negotiationId, accept);

        return Ok(res);
    }

    [HttpPatch("{negotiationId:int}/propose")]
    public async Task<IActionResult> ProposeNewPrice(int negotiationId, [FromBody] decimal newPrice)
    {
        var negotiation = await _negotiationService.ProposeNewPrice(negotiationId, newPrice);
        
        return Ok(negotiation);
    }

    [HttpGet("{negotiationId:int}")]
    public async Task<IActionResult> GetNegotiation(int negotiationId)
    {
        var negotiation = await _negotiationService.GetNegotiationAsync(negotiationId);
        
        return Ok(negotiation);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllNegotiations()
    {
        var negotiations = await _negotiationService.GetNegotiationsAsync();
        
        return Ok(negotiations);
    }

    [HttpPatch("{negotiationId:int}/cancel")]
    public async Task<IActionResult> CancelNegotiation(int negotiationId)
    {
        var res = await _negotiationService.CancelNegotiationAsync(negotiationId);
        
        return Ok(res);
    }
}