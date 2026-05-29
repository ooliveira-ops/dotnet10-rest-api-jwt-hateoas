//  using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using RestWithASPNET10Erudio.Data.DTO.V1;
using RestWithASPNET10Erudio.DATA.DTO.V1;
using RestWithASPNET10Erudio.FIles.Exporters.Factory;
using RestWithASPNET10Erudio.Hypermedia.Utils;
using RestWithASPNET10Erudio.Services;
using RestWithASPNET10Erudio.Services.Impl;

namespace RestWithASPNET10Erudio.Controllers.V1                                 //"Controle de Pessoas!
{
	[ApiController]
	[Route("api/[controller]/v1")]
	[Authorize("Bearer")]
	//[EnableCors("LocalPolicy")]
	public class PersonController : ControllerBase                                  //ele vai permitir que cadastre, atualize, liste, recupere 1 apenas e delete
	{

		private IPersonServices _personService;
		private readonly ILogger<PersonController> _logger;
		public PersonController(IPersonServices personServices,
			ILogger<PersonController> logger)
		{
			_personService = personServices;
			_logger = logger;
		}



		[HttpGet]
		[ProducesResponseType(200, Type = typeof(List<PersonDTO>))]
		[ProducesResponseType(400)]
		[ProducesResponseType(401)]
		public IActionResult Get()
		{
			_logger.LogInformation("Fetching all persons");                                             // "buscando todas as pessoas
			return Ok(_personService.FindALL());
		}

		[HttpGet("find-by-name")]
		[ProducesResponseType(200, Type = typeof(List<PersonDTO>))]
		[ProducesResponseType(400)]
		[ProducesResponseType(401)]
		public IActionResult GetByName(
			[FromQuery] string firstName,
			[FromQuery] string lastName
			)
		{
			_logger.LogInformation("Fetching persons by name: {firstName}, {lastName}", firstName, lastName);                                           // buscando todas as pessoas
			return Ok(_personService.FindByName(firstName, lastName));
		}


		[HttpGet("{id}")]
		[ProducesResponseType(200, Type = typeof(PersonDTO))]
		[ProducesResponseType(400)]
		[ProducesResponseType(401)]
		public IActionResult Get(long id)
		{
			_logger.LogInformation("Fetching person with ID {id}", id);
			var person = _personService.FindByID(id);
			if (person == null)
			{
				_logger.LogWarning("Person with ID {id} not found", id);
				return NotFound();
			}
			return Ok(person);
		}
		

		[HttpGet("{sortDirection}/{pageSize}/{page}")]
		[ProducesResponseType(200, Type = typeof(PagedSearchDTO<PersonDTO>))]       //"PagedSearchDTO" = o paged search serve para paginar as pessoas. um exeplo de PagedSearch na prática é no Postman com a url "api/person/v1/find-with-pagedsearch" e com o Query Param: ?name
		[ProducesResponseType(400)]
		[ProducesResponseType(401)]
		// [EnableCors("LocalPolicy")]
		public IActionResult Get(
			[FromQuery] string name,
			string sortDirection,
			int pageSize,
			int page
			)
		{
			_logger.LogInformation("Fetching person with paged search name: {name}, {sortDirection}, {pageSize}, {page})",
			name, sortDirection, pageSize, page);
			return Ok(_personService.FindWithPagedSearch(name, sortDirection, pageSize, page));               
			
			
			
			//if (person == null)
			//{
			//	_logger.LogWarning("Person with ID {id} not found", id);        //vai ser o log de aviso: "Pessoa nao encontrada ID (parametro id: {id}) e o 'id' para aparecer o ID.
			//	return NotFound();
			//} 
			//return Ok(person);

		}


		[HttpPost]                                                              //criar
		[ProducesResponseType(200, Type = typeof(PersonDTO))]
		[ProducesResponseType(400)]
		[ProducesResponseType(401)]
		// [EnableCors("MultipleOriginPolicy")]
		public IActionResult Post([FromBody] PersonDTO person)
		{
			_logger.LogInformation("Creating new Person: {firstname}", person.FirstName);                   //Criar uma nossa pessoa com o 1ºNome e o comando para setar a pessoa pelo 1º nome.
			var createdPerson = _personService.Create(person);
			if (createdPerson == null)
			{
				_logger.LogError("Failed to create person with name {firstname}", person.FirstName);        //"Falha ao criar pessoa com nome (1º nome)" e o comando para setar p 1º nome.
				return NotFound();
			}
			return Ok(createdPerson);
		}



		[HttpPut]                                                           //atualizar
		[ProducesResponseType(200, Type = typeof(PersonDTO))]
		[ProducesResponseType(400)]
		[ProducesResponseType(401)]
		public IActionResult Put([FromBody] PersonDTO person)
		{
			_logger.LogInformation("Uptating person with ID {id}", person.Id);              //"atualizando pessoa pelo ID" = parametro id {id} e o comando para buscar ela pelo id 'person.Id'
			var createdPerson = _personService.Update(person);
			if (createdPerson == null)
			{
				_logger.LogError("Failed to uptade person with ID {id}", person.Id);        //"Falha ao criar pessoa pelo ID" = parametro id {id} e o comando para buscar pelo id 'person.Id'		
				return NotFound();
			}
			_logger.LogDebug("Person updated successfully: {firstname}", createdPerson.FirstName);         //"Pessoa criada com suacesso; 1º nome e o comando para setar o 1º nome.
			return Ok(createdPerson);
		}



		[HttpDelete("{id}")]
		[ProducesResponseType(204, Type = typeof(PersonDTO))]
		[ProducesResponseType(400)]
		[ProducesResponseType(401)]
		public IActionResult Delete(long id)
		{
			_logger.LogInformation("Deleting person with ID {id}", id);                //"Deletando pessoa pelo ID" = parametro {id} e o 'id' para aparecer o ID.
			_personService.Delete(id);
			_logger.LogDebug("Person with ID {id} deleted successfully", id);           //"Pessoa com ID excluída com sucesso" = param. {id} e o 'id' para setar o ID da pessoa.
			return NoContent();
		}



		[HttpPatch("{id}")]                                                          //atualilzar parcial
		[ProducesResponseType(200, Type = typeof(PersonDTO))]
		[ProducesResponseType(400)]
		[ProducesResponseType(401)]
		public IActionResult Disable(long id)
		{
			_logger.LogInformation("Disabling person with ID {id}", id);                //"desabilitando pessoa pelo ID" = parametro id {id} e o comando para buscar ela pelo id 'person.Id'
			var disabledPerson = _personService.Disable(id);
			if (disabledPerson == null)
			{
				_logger.LogError("Failed to disable person with ID {id}", id);      //"Falha ao desabilitar pessoa pelo ID" = parametro id {id} e o comando para buscar pelo id 'person.Id'
				return NotFound();
			}
			_logger.LogDebug("Person with ID {id} disabled successfully: {firstname}", id, disabledPerson.FirstName);         //"Pessoa desabilitada com suacesso; 1º nome e o comando para setar o 1º nome.
			return Ok(disabledPerson);
		}

		[HttpPost("massCreation")]
		[ProducesResponseType(200, Type = typeof(List<PersonDTO>))]
		[ProducesResponseType(400)]
		[ProducesResponseType(401)] 
		public async Task<IActionResult> MassCreation(
		   [FromForm] FileUploadDTO input)
		{
			if (input.File == null || input.File.Length == 0)
			{
				_logger.LogWarning("No file uploaded for mass creation");
				return BadRequest("File is Required!");
			}
			_logger.LogInformation("Starting mass creation from uploaded file: {fileName}", input.File.FileName);

			var people = await _personService.MassCreationAsync(input.File);

			if (people == null)
			{
				_logger.LogError("Mass creation failed for file: {fileName}", input.File.FileName);
				return NoContent();
			}
			_logger.LogInformation("Mass creation completed successfully with {count} records", people.Count);
			return Ok(people);
		}

		[HttpGet("exportPage/{sprtDirection}/{pageSize}/{page}")]
		[ProducesResponseType(200, Type = typeof(FileContentResult))]
		[ProducesResponseType(400)]
		[ProducesResponseType(401)]
		[ProducesResponseType(415)]
		[Produces(
			MediaTypes.ApplicationXlsx,     //com isso, conseguimos garantir que ele exporta nos 2 formatos
			MediaTypes.ApplicationCsv
		)]
		public IActionResult ExportPage(            // corpo do método
			string sortDirection,
			int pageSize,
			int page,
			[FromQuery] string name = ""
		)
		{
			var acceptHeader = Request.Headers["Accept"].ToString();
			if (string.IsNullOrWhiteSpace(acceptHeader))
			{
				return BadRequest("Accept header is required");
			}

			_logger.LogInformation("Exporting persons with paged search: {name}, {sortDirection}, {pageSize}, {page}, {acceptHeader}",
			     name, sortDirection, pageSize, page, acceptHeader);

			try
			{
				var fileResult = _personService.ExportPage(
					page,
					pageSize,
					sortDirection,
					acceptHeader,
					name);

				return fileResult;
			}

			catch (NotSupportedException ex)
			{
				_logger.LogWarning(ex, "Unsupported export format" + "requested: {AcceptHeader}", acceptHeader);
				return StatusCode(
					StatusCodes.Status415UnsupportedMediaType, ex.Message);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Unexpected error" +  "while exporting persons");

				return StatusCode(
					StatusCodes.Status500InternalServerError, ex.Message);
			}
		}
	}
}
