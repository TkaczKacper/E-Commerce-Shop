using System.Collections.Generic;
using System.Threading.Tasks;
using ShopAPI.Data;
using ShopAPI.DataTransferObjects;
using ShopAPI.Services.Interfaces;
using ShopAPI.Models;

namespace ShopAPI.Services;

public class ProductService : IProductService
{
    private readonly AppDbContext _context;

    public ProductService(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<Product> AddProduct(ProductDTO productDto)
    {
        var productToAdd = new Product
        {
            Name = productDto.ProductName,
            Price = productDto.Price
        };
        
        var product = _context.Products.Add(productToAdd);
        await _context.SaveChangesAsync();

        return product.Entity;
    }

    public Task<Product?> GetProductById(int id)
    {
        throw new System.NotImplementedException();
    }

    public Task<IEnumerable<Product>> GetProducts()
    {
        throw new System.NotImplementedException();
    }

    public Task<Product?> UpdateProduct(int productId, ProductDTO productDto)
    {
        throw new System.NotImplementedException();
    }

    public Task<bool> DeleteProduct(int id)
    {
        throw new System.NotImplementedException();
    }
}