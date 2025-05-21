using ordermanagement.domain.Enums;

namespace ordermanagement.webapi.DTO
{
    public class UpdateOrderStatus
    { 
        public OrderStatus NewStatus { get; set; }

        public string? Notes { get; set; }
        public string UpdatedBy { get; set; }
    }
}
