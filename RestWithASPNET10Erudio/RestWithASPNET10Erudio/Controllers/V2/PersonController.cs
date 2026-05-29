using Microsoft.AspNetCore.Mvc;
using RestWithASPNET10Erudio.Data.DTO.V2;
using RestWithASPNET10Erudio.Services;

namespace RestWithASPNET10Erudio.Controllers.V2
{
	[ApiController]
	[Route("api/[controller]/v2")]
	public class PersonController : ControllerBase
	{
		private IPersonServicesV2 _personService;
		private readonly ILogger<PersonController> _logger;

		public PersonController(IPersonServicesV2 personServices,
			ILogger<PersonController> logger)
		{
			_personService = personServices;
			_logger = logger;
		}

		[HttpGet]
		[ProducesResponseType(200, Type = typeof(PersonDTO))]
		[ProducesResponseType(400)]
		[ProducesResponseType(401)]
		public IActionResult Get()
		{
			return Ok(_personService.FindALL());
		}

		[HttpGet("{id}")]
		[ProducesResponseType(200, Type = typeof(PersonDTO))]
		[ProducesResponseType(400)]
		[ProducesResponseType(401)]
		public IActionResult Get(long id)
		{
			var person = _personService.FindByID(id);
			if (person == null) return NotFound();
			return Ok(person);
		}

		[HttpPost]
		[ProducesResponseType(200, Type = typeof(PersonDTO))]
		[ProducesResponseType(400)]
		[ProducesResponseType(401)]
		public IActionResult Post([FromBody] PersonDTO person)
		{
			_logger.LogInformation("Creating new Person: {firstname}", person.FirstName);
			var createdPerson = _personService.Create(person);
			if (createdPerson == null)
			{
				_logger.LogError("Failed to create person with name {firstname}", person.FirstName);
				return NotFound();
			}
			//createdPerson.LastName = null;
			//createdPerson.Age = 20;
			return Ok(createdPerson);
		}

		[HttpPut]
		public IActionResult Put([FromBody] PersonDTO person)
		{
			_logger.LogInformation("Updating person with ID {id}", person.Id);
			var updatedPerson = _personService.Update(person);
			if (updatedPerson == null)
			{
				_logger.LogError("Failed to update person with ID {id}", person.Id);
				return NotFound();
			}
			return Ok(updatedPerson); 
		}

		[HttpDelete("{id}")]
		public IActionResult Delete(long id)
		{
			_logger.LogInformation("Deleting person with ID {id}", id);
			_personService.Delete(id);
			return NoContent();
		}
	}
}