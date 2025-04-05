using System.Collections.Generic;
using System.Threading.Tasks;
using ShopAPI.DataTransferObjects;
using ShopAPI.Models;

namespace ShopAPI.Services.Interfaces;

public interface IProductService
{
        Task<Product> AddProduct(ProductDTO productDto);
        Task<Product?> GetProductById(int id);
        Task<List<Product>> GetProducts();
        Task<Product?> UpdateProduct(int id, ProductDTO productDto);
        Task<bool> DeleteProduct(int id);
}