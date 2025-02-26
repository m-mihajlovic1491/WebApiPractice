using WebApiPractice.DTO;
using WebApiPractice.Models;

namespace WebApiPractice.Interfaces
{
    public interface IOrderRepository
    {
        ICollection<Order> GetAllOrders();
        ICollection<OrderDto> GetOrdersWithLines();
    }
}
