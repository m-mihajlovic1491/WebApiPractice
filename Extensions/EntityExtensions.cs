using WebApiPractice.DTO;
using WebApiPractice.Models;

namespace WebApiPractice.Extensions
{
    public static class EntityExtensions
    {
        // Convert Order to OrderDto
        public static OrderDto ToDto(this Order order)
        {
            return new OrderDto
            {
                id = order.id,
                OrderNumber = order.OrderNumber,
                TotalPrice = order.TotalPrice,
                Reference1 = order.Reference1,
                Products = order.Products.Select(p => new ProductDto
                {
                    id = p.id,
                    Name = p.Name,
                    Price = p.Price,
                    Description = p.Description,
                }).ToList(),

            };
        }

       
    }

}
