using AspNetWebApi_order_product.Entity;

namespace AspNetWebApi_order_product.Service
{
    public interface IProductService
    {
        
        Task<Product?> Add(Product product);

      
        Task<Product?> GetById(int id);

      
        Task<List<Product>> ListAll();

       
        Task<Product?> RemoveById(int id);

       
        Task<Product?> UpdateById(int id, Product product);
    }
}
