using System.Collections.Generic;
using System.Threading.Tasks;
using ShopAPI.DataTransferObjects;
using ShopAPI.Models;

namespace ShopAPI.Services.Interfaces;

public interface IProductService
{
        Task<Product> AddProduct(ProductDTO productDto);
        Task<Product> GetProductById(int id);
        Task<IEnumerable<Product>> GetProducts();
        Task<Product> UpdateProduct(ProductDTO productDto);
        Task<bool> DeleteProduct(int id);
}