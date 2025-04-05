using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http.ModelBinding;
using Microsoft.EntityFrameworkCore;
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
    
    public async Task<Product> AddProduct(ProductDTO? productDto)
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

    public async Task<Product?> GetProductById(int id)
    {
        var product = await _context.Products.FindAsync(id);

        return product;
    }

    public Task<List<Product>> GetProducts()
    {
        var products = _context.Products.ToListAsync();
        
        return products;
    }

    public Task<Product?> UpdateProduct(int productId, ProductDTO productDto)
    {
        throw new System.NotImplementedException();
    }

    public async Task<bool> DeleteProduct(int id)
    {
        var product = await _context.Products.FindAsync(id);

        if (product is null)
        {
            return false;
        }
        
        _context.Products.Remove(product);
        await _context.SaveChangesAsync();

        return true;
    }
}