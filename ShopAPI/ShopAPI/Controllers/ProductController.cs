using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopAPI.DataTransferObjects;
using ShopAPI.Models;
using ShopAPI.Services.Interfaces;

namespace ShopAPI.Controllers;

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
        return null;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllProducts()
    {
        return null;
    }

    [HttpGet("{productId:int}")]
    public async Task<IActionResult> GetProductById(int productId)
    {
        return null;
    }

    [HttpPatch("{productId:int}")]
    public async Task<IActionResult> UpdateProduct(int productId, [FromBody] ProductDTO productDto)
    {
        return null;
    }

    [HttpDelete("{productId:int}")]
    public async Task<IActionResult> DeleteProduct(int productId)
    {
        return null;
    }
}