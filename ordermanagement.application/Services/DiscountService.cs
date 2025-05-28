using ordermanagement.application.Contracts;
using ordermanagement.domain.Entities;

namespace ordermanagement.application.Services
{
    public interface IDiscountService
    {
        Task<Discount?> GetDiscountByPromoCodeAsync(string promoCode);
        Task<IEnumerable<Discount>> GetApplicableDiscountsAsync(Customer customer);
        Task<decimal> CalculateDiscountAmountAsync(Order order, string? promoCode = null);
        Task<IEnumerable<Discount>> GetAllDiscountsAsync();
    }

    public class DiscountService : IDiscountService
    {
        private readonly IDiscountRepository _discountRepository;
        private readonly IOrderRepository _orderRepository;

        public DiscountService(
            IDiscountRepository discountRepository,
            IOrderRepository orderRepository)
        {
            _discountRepository = discountRepository;
            _orderRepository = orderRepository;
        }

        public Task<IEnumerable<Discount>> GetAllDiscountsAsync()
        {
            return _discountRepository.GetAllAsync();
        }

        public Task<Discount?> GetDiscountByPromoCodeAsync(string promoCode)
        {
            return _discountRepository.GetByPromoCodeAsync(promoCode);
        }

        public async Task<IEnumerable<Discount>> GetApplicableDiscountsAsync(Customer customer)
        {
            var allDiscounts = await _discountRepository.GetAllAsync();
            var applicableDiscounts = new List<Discount>();
            var customerOrders = await _orderRepository.GetOrdersByCustomerIdAsync(customer.Id);
            var orderCount = customerOrders.Count();
            foreach (var discount in allDiscounts)
            {
                // Check if the discount is applicable to this customer
                if (IsDiscountApplicableToCustomer(discount, customer, orderCount))
                {
                    applicableDiscounts.Add(discount);
                }
            }

            return applicableDiscounts;
        }

        public async Task<decimal> CalculateDiscountAmountAsync(Order order, string? promoCode = null)
        {
            var discounts = new List<Discount>();

            // If promo code is provided, try to apply it
            if (!string.IsNullOrEmpty(promoCode))
            {
                var promoDiscount = await GetDiscountByPromoCodeAsync(promoCode);

                if (promoDiscount != null && order.Customer != null)
                {
                    // Check if discount is applicable to this customer
                    if (IsDiscountApplicableToCustomer(promoDiscount, order.Customer))
                    {
                        discounts.Add(promoDiscount);
                    }
                }
            }
            // If no promo code provided, apply best available discount
            else if (order.Customer != null)
            {
                var applicableDiscounts = await GetApplicableDiscountsAsync(order.Customer);

                if (applicableDiscounts.Any())
                {
                    // Apply the best discount (highest value)
                    var bestDiscount = applicableDiscounts.OrderByDescending(d => d.Value).First();
                    //order.AppliedDiscounts.Add(bestDiscount);
                    discounts.Add(bestDiscount);
                }
            }
            order.ApplyDiscounts(discounts);

            return order.DiscountAmount;
        }

        // Helper method to check if a discount applies to a specific customer
        private bool IsDiscountApplicableToCustomer(Discount discount, Customer customer, int orderCount = 0)
        {
            // If no customer segments specified, discount is universal
            if (discount.CustomerSegments.Count == 0)
            {
                return IsWithinSignupDateRange(discount, customer);
            }

            // Check if customer belongs to any of the specified segments
            bool isInSegment = discount.CustomerSegments.Contains(customer.Segment);

            // Must be in segment AND within signup date range
            return isInSegment && IsWithinSignupDateRange(discount, customer) && orderCount >= discount.MinOrders;
        }

        private bool IsWithinSignupDateRange(Discount discount, Customer customer)
        {
            if (!discount.MaxSignupDate.HasValue && !discount.MaxSignupDate.HasValue)
                return true;

            if (discount.MaxSignupDate.HasValue && customer.RegisteredOn < discount.MaxSignupDate.Value)
                return false;

            if (discount.MaxSignupDate.HasValue && customer.RegisteredOn > discount.MaxSignupDate.Value)
                return false;

            return true;
        }
    }
}