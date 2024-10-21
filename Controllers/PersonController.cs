using Microsoft.AspNetCore.Mvc;
using WebApiPractice.Data;
using WebApiPractice.Models;

namespace WebApiPractice.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonController : ControllerBase
    {
        
        public ApplicationDbContext _context { get; set; }

        private readonly ILogger<PersonController> _logger;

        public PersonController(ILogger<PersonController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        [Route("GetAllPerson")]
        public IActionResult Get()
        {
            var person = _context.Person.ToList();
            return Ok(person);
        }

        [HttpPost]
        [Route("person")]
        public IActionResult Post([FromBody] Person person) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            if (person == null) {
                return BadRequest("person should not be null");
            }
            person.id = Guid.NewGuid();
            _context.Person.Add(person);
            _context.SaveChanges();
            return Ok($"Person {person.Name} {person.LastName} aged {person.Age} created ");
        }

        [HttpDelete]
        [Route("person")]
        public IActionResult Delete(Guid guid) {

            var person = _context.Person.SingleOrDefault(p => p.id == guid);
            if (person == null)
            {
                return BadRequest($"Person with the guid : {guid} does not exist ");
            }
            
            _context.Person.Remove(person);
            _context.SaveChanges();
            return Ok($"Person with guid {guid} removed");
        }

        [HttpGet("{guid:guid}")]
        public IActionResult GetSingle(Guid guid) {
            var person = _context.Person.SingleOrDefault(p => p.id == guid);
            if (person == null) {
                return NotFound("Person not found");
            }
            return Ok(person);
        }
        
    }
}
