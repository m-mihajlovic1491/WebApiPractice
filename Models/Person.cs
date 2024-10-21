namespace WebApiPractice.Models
{
    public class Person
    {
        public Guid id  { get; set; }
        public string? Name { get; set; }
        public string? LastName { get; set; }
        public int? Age { get; set; }
    }
}
