using WebApiPractice.Models;

namespace WebApiPractice.DTO
{
    public class OrderDto
    {
        public Guid id { get; set; }
        public string OrderNumber { get; set; }

        public double TotalPrice { get; set; }

        public List<ProductDto> Products { get; set; } = [];

        public string? Reference1 { get; set; }
    }
}
