using ordermanagement.application.Contracts;
using ordermanagement.domain.Entities;

namespace ordermanagement.application.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product?> GetProductByIdAsync(int id);
        Task<Product> CreateProductAsync(Product product);
        Task UpdateProductAsync(Product product);
        Task DeleteProductAsync(int id);
    }

    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return _productRepository.GetAllAsync();
        }

        public Task<Product?> GetProductByIdAsync(int id)
        {
            return _productRepository.GetByIdAsync(id);
        }

        public Task<Product> CreateProductAsync(Product product)
        {
            return _productRepository.AddAsync(product);
        }

        public Task UpdateProductAsync(Product product)
        {
            return _productRepository.UpdateAsync(product);
        }

        public Task DeleteProductAsync(int id)
        {
            return _productRepository.DeleteAsync(id);
        }
    }
}