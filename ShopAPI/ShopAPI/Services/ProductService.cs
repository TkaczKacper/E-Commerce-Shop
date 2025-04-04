using System.Collections.Generic;
using System.Threading.Tasks;
using ShopAPI.DataTransferObjects;
using ShopAPI.Services.Interfaces;
using ShopAPI.Models;

namespace ShopAPI.Services;

public class ProductService : IProductService
{
    
    public Task<Product> AddProduct(ProductDTO productDto)
    {
        throw new System.NotImplementedException();
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