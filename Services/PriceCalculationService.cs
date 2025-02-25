
using WebApiPractice.Data;

namespace WebApiPractice.Services
{
    public class PriceCalculationService : IPriceRecalculationService
    {
        private readonly ApplicationDbContext _context;        

        public PriceCalculationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public double CalculateTotalPrice(Guid orderId)
        {
            var totalPrice = _context.OrderProduct
                .Where(x => x.OrderId == orderId)
                .Join(_context.Product,
                x => x.ProductId,
                p => p.id,
                (x, p) => p.Price)
                .Sum();

            return totalPrice;
        }
    }
}
