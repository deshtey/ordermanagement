using ordermanagement.application.Contracts;
using ordermanagement.domain.Entities;
using ordermanagement.infrastructure.TestData;

namespace ordermanagement.infrastructure.InMemoryRepo
{
    public class InMemoryDiscountRepository : IDiscountRepository
    {
        private readonly List<Discount> _discounts = SeedData.GetDiscounts();

        public Task<IEnumerable<Discount>> GetAllAsync()
        {
            return Task.FromResult(_discounts.AsEnumerable());
        }

        public Task<Discount?> GetByIdAsync(int id)
        {
            return Task.FromResult(_discounts.FirstOrDefault(d => d.Id == id));
        }

        public Task<Discount?> GetByPromoCodeAsync(string promoCode)
        {
            return Task.FromResult(_discounts.FirstOrDefault(d =>
                d.PromoCode.Equals(promoCode, StringComparison.OrdinalIgnoreCase)));
        }

        public Task<IEnumerable<Discount>> GetByCustomerSegmentAsync(CustomerSegment segment)
        {
            return Task.FromResult(_discounts.Where(d =>
                d.CustomerSegments.Any(s => s == segment)).AsEnumerable());
        }

        public Task<Discount> AddAsync(Discount entity)
        {
            if (entity.Id == 0)
            {
                entity.Id = _discounts.Max(d => d.Id) + 1;
            }

            _discounts.Add(entity);
            return Task.FromResult(entity);
        }

        public Task UpdateAsync(Discount entity)
        {
            var existingDiscount = _discounts.FirstOrDefault(d => d.Id == entity.Id);

            if (existingDiscount == null)
            {
                throw new KeyNotFoundException($"Discount with ID {entity.Id} not found");
            }

            var index = _discounts.IndexOf(existingDiscount);
            _discounts[index] = entity;

            return Task.CompletedTask;
        }

        public Task DeleteAsync(int id)
        {
            var discount = _discounts.FirstOrDefault(d => d.Id == id);

            if (discount == null)
            {
                throw new KeyNotFoundException($"Discount with ID {id} not found");
            }

            _discounts.Remove(discount);

            return Task.CompletedTask;
        }
    }
}