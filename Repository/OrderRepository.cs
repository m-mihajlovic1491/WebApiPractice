using Microsoft.EntityFrameworkCore;
using WebApiPractice.Data;
using WebApiPractice.DTO;
using WebApiPractice.Extensions;
using WebApiPractice.Interfaces;
using WebApiPractice.Models;

namespace WebApiPractice.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context ;
        public OrderRepository(ApplicationDbContext context)
        {
            _context = context ;
        }
        public ICollection<Order> GetAllOrders()
        {
            return _context.Order.OrderBy(x=>x.TotalPrice).ToList();
        }

        public ICollection<OrderDto> GetOrdersWithLines() 
        {
            var orders = _context.Order
                .Include(p=>p.Products)
                .Select(p=>p.ToDto())
                .ToList();

            return orders;
        
        }
    }
}
