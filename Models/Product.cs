namespace WebApiPractice.Models
{
    public class Product
    {
        public Guid id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public Double Price { get; set; }

        public List<Order> Orders { get; set; } = [];
    }
}
