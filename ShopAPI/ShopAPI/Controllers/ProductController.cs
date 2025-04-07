using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopAPI.DataTransferObjects;
using ShopAPI.Models;
using ShopAPI.Services.Interfaces;

namespace ShopAPI.Controllers;

[ApiController]
[Route("api/v1/product")]
[Authorize(Roles = "Employee")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] AddProductDTO productDto)
    {
        var res = await _productService.AddProduct(productDto);
        
        return StatusCode(201, res);
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllProducts()
    {
        var res = await _productService.GetProducts();
       
        return Ok(res);
    }

    [HttpGet("{productId:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetProductById(int productId)
    {
        var product = await _productService.GetProductById(productId);
        
        return Ok(product);
    }

    [HttpPatch("{productId:int}")]
    public async Task<IActionResult> UpdateProduct(int productId, [FromBody] UpdateProductDTO updateProductDto)
    {
        var updatedProduct = await _productService.UpdateProduct(productId, updateProductDto);
        
        return Ok(updatedProduct);
    }

    [HttpDelete("{productId:int}")]
    public async Task<IActionResult> DeleteProduct(int productId)
    {
        var res = await _productService.DeleteProduct(productId);

        return NoContent();
    }
}