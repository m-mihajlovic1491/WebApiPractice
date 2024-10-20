using Microsoft.AspNetCore.Mvc;
using WebApiPractice.Models;

namespace WebApiPractice.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonController : ControllerBase
    {
        public static List<Person> DbFake { get; set; } = new List<Person>();

        private readonly ILogger<PersonController> _logger;

        public PersonController(ILogger<PersonController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("GetAllPerson")]
        public IActionResult Get()
        {
            var person = DbFake.Select(x=>x);
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
            person.guid = Guid.NewGuid();
            DbFake.Add(person);
            return Ok($"Person {person.Name} {person.LastName} aged {person.Age} created ");
        }

        [HttpDelete]
        [Route("person")]
        public IActionResult Delete(Guid guid) {
            if (!DbFake.Any(p => p.guid == guid))
            {
                return BadRequest($"Person with the guid : {guid} does not exist ");
            }
            DbFake.RemoveAll(p => p.guid == guid);
            return Ok($"Persn with guid {guid} removed");
        }

        [HttpGet("{guid:guid}")]
        public IActionResult GetSingle(Guid guid) {
            var person = DbFake.SingleOrDefault(p => p.guid == guid);
            if (person == null) {
                return NotFound("Person not found");
            }
            return Ok(person);
        }
        
    }
}
