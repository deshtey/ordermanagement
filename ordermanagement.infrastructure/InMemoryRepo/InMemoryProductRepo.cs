using ordermanagement.application.Contracts;
using ordermanagement.domain.Entities;
using ordermanagement.infrastructure.TestData;

namespace ordermanagement.application.Contracts.InMemoryRepo
{
    public class InMemoryProductRepo : IProductRepository
    {
        private readonly List<Product> _products = SeedData.GetProducts().ToList();

        public Task<IEnumerable<Product>> GetAllAsync()
        {
            return Task.FromResult(_products.AsEnumerable());
        }

        public Task<Product?> GetByIdAsync(int id)
        {
            return Task.FromResult(_products.FirstOrDefault(p => p.Id == id));
        }

        public Task<Product> AddAsync(Product entity)
        {
            if (entity.Id == 0)
            {
                entity.Id = 7;
            }

            entity.CreatedAt = DateTime.UtcNow;
            _products.Add(entity);

            return Task.FromResult(entity);
        }

        public Task UpdateAsync(Product entity)
        {
            var existingProduct = _products.FirstOrDefault(p => p.Id == entity.Id);

            if (existingProduct == null)
            {
                throw new KeyNotFoundException($"Product with ID {entity.Id} not found");
            }

            var index = _products.IndexOf(existingProduct);
           // entity.UpdatedAt = DateTime.UtcNow;
            _products[index] = entity;

            return Task.CompletedTask;
        }

        public Task DeleteAsync(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                throw new KeyNotFoundException($"Product with ID {id} not found");
            }

            _products.Remove(product);

            return Task.CompletedTask;
        }
    }
}