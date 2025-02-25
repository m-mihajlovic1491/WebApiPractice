using WebApiPractice.Models;

namespace WebApiPractice.DTO
{
    public class ProductDto
    {
        public Guid id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public Double Price { get; set; }
        
    }
}
