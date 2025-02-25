using System.Reflection.Metadata.Ecma335;

namespace WebApiPractice.Models
{
    public class Order
    {
        public Guid id { get; set; }
        public string OrderNumber { get; set; }

        public double TotalPrice { get; set; }

        public List<Product> Products { get; set; } = [];

        public string? Reference1 { get; set; }
    }
}
