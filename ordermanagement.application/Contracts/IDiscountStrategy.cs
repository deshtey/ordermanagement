using ordermanagement.domain.Entities;

namespace ordermanagement.application.Contracts
{
    public interface IDiscountStrategy
    {
        string Name { get; }
        string Description { get; }
        Task<decimal> CalculateDiscountAsync(Order order, Customer customer);
        bool IsApplicable(Order order, Customer customer);
    }
}