using ordermanagement.domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ordermanagement.application.Contracts
{
    public interface IOrderStatusService
    {
        Task UpdateOrderStatusAsync(int orderId, OrderStatus newStatus);
    }
}
