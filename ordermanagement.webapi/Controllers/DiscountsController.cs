using Microsoft.AspNetCore.Mvc;
using ordermanagement.application.Contracts;
using ordermanagement.application.Services;
using ordermanagement.domain.Entities;

namespace ordermanagement.webapi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DiscountsController : ControllerBase
    {
        private readonly IDiscountService _discountService;
        private readonly ICustomerRepository _customerRepository;

        public DiscountsController(
            IDiscountService discountService,
            ICustomerRepository customerRepository)
        {
            _discountService = discountService;
            _customerRepository = customerRepository;
        }
        /// <summary>
        /// Gets all applicable discounts for a customer
        /// </summary>
        /// <param name="customerId">Customer ID</param>
        /// <returns>List of applicable discounts</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Discount>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<Discount>>> GetAllDiscounts()
        {  

            var discounts = await _discountService.GetAllDiscountsAsync();
            return Ok(discounts);
        }
        /// <summary>
        /// Gets all applicable discounts for a customer
        /// </summary>
        /// <param name="customerId">Customer ID</param>
        /// <returns>List of applicable discounts</returns>
        [HttpGet("customer/{customerId}")]
        [ProducesResponseType(typeof(IEnumerable<Discount>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<Discount>>> GetCustomerDiscounts(int customerId)
        {
            var customer = await _customerRepository.GetByIdAsync(customerId);

            if (customer == null)
            {
                return NotFound();
            }

            var discounts = await _discountService.GetApplicableDiscountsAsync(customer);
            return Ok(discounts);
        }

        /// <summary>
        /// Validates a promo code
        /// </summary>
        /// <param name="promoCode">Promo code to validate</param>
        /// <returns>Discount details if valid</returns>
        [HttpGet("validate/{promoCode}")]
        [ProducesResponseType(typeof(Discount), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Discount>> ValidatePromoCode(string promoCode)
        {
            var discount = await _discountService.GetDiscountByPromoCodeAsync(promoCode);

            if (discount == null)
            {
                return NotFound();
            }

            return Ok(discount);
        }
    }
}