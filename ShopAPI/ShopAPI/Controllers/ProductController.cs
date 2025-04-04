using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopAPI.DataTransferObjects;
using ShopAPI.Models;
using ShopAPI.Services.Interfaces;

namespace ShopAPI.Controllers;

[ApiController]
[Route("api/v1/product")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpPost]
    [Authorize(Roles = "Employee")]
    public async Task<IActionResult> CreateProduct([FromBody] ProductDTO productDto)
    {
        var res = await _productService.AddProduct(productDto);
        
        return Ok(res);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllProducts()
    {
        var res = await _productService.GetProducts();
       
        return Ok(res);
    }

    [HttpGet("{productId:int}")]
    public async Task<IActionResult> GetProductById(int productId)
    {
        var res = await _productService.GetProductById(productId);
        
        if (res is null)
            return NotFound("Product not found");
        
        return Ok(res);
    }

    [HttpPatch("{productId:int}")]
    public async Task<IActionResult> UpdateProduct(int productId, [FromBody] ProductDTO productDto)
    {
        var res = await _productService.UpdateProduct(productId, productDto);
        
        if (res is null)
            return NotFound("Product not found.");
        
        return Ok(res);
    }

    [HttpDelete("{productId:int}")]
    public async Task<IActionResult> DeleteProduct(int productId)
    {
        var res = await _productService.DeleteProduct(productId);

        if (res)
            return NoContent();
        
        return NotFound("Product not found.");
    }
}