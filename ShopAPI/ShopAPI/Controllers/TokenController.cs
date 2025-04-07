using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopAPI.DataTransferObjects;
using ShopAPI.Services;
using ShopAPI.Services.Interfaces;

namespace ShopAPI.Controllers;

[ApiController]
[Route("api/v1/token")]
public class TokenController : ControllerBase
{
    private readonly ITokenService _tokenService;

    public TokenController(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginDTO? loginDto)
    {
        if (loginDto is null)
            return BadRequest("Authentication failed.");

        var accessToken = _tokenService.GenerateAccessToken(loginDto.Username, loginDto.Email);
        
        Response.Headers.Append("Authorization", $"Bearer {accessToken}");
        
        return Ok(accessToken);
    }
}