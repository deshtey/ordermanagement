using ordermanagement.domain.Entities;

namespace ordermanagement.application.Contracts
{
    public interface IDiscountRepository
    {

        Task<Discount?> GetByPromoCodeAsync(string promoCode);
        Task<IEnumerable<Discount>> GetByCustomerSegmentAsync(CustomerSegment segment);
        Task<IEnumerable<Discount>> GetAllAsync();
    }
}
