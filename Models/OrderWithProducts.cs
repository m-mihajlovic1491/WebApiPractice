namespace WebApiPractice.Models
{
    public class OrderWithProducts
    {
        public Guid OrderId { get; set; }           // Unique identifier for the order
        public string OrderNumber { get; set; }      // Order number
        public double TotalPrice { get; set; }       // Total price of the order
        public string? Reference1 { get; set; }      // Optional reference field
        public List<Product> Products { get; set; } = new();
    }
}
