using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestWithASPNET10Erudio.Data.DTO.V1;
using RestWithASPNET10Erudio.Services;

namespace RestWithASPNET10Erudio.Controllers.V1
{
	[ApiController]
	[Route("api/[controller]/v1")]
	[Authorize("Bearer")]
	public class BookController : ControllerBase
	{
		private readonly IBookServices _bookService;
		private readonly ILogger<BookController> _logger;

		public BookController(IBookServices bookService, ILogger<BookController> logger)
		{
			_bookService = bookService;
			_logger = logger;
		}

		[HttpDelete("{id}")]
		[ProducesResponseType(204, Type = typeof(PersonDTO))]
		[ProducesResponseType(400)]
		[ProducesResponseType(401)]
		public IActionResult Delete(long id)
		{
			_bookService.Delete(id);
			return NoContent();
		}

		[HttpGet]
		[ProducesResponseType(200, Type = typeof(BookDTO))]
		[ProducesResponseType(400)]
		[ProducesResponseType(401)]
		public IActionResult FindAll()
		{
			return Ok(_bookService.FindAll());
		}

		[HttpGet("{id}")]
		[ProducesResponseType(200, Type = typeof(BookDTO))]
		[ProducesResponseType(400)]
		[ProducesResponseType(401)]
		public IActionResult FindByID(long id)
		{
			var book = _bookService.FindByID(id);
			if (book == null) return NotFound();
			return Ok(book);
		}

		[HttpPost]
		[ProducesResponseType(200, Type = typeof(BookDTO))]
		[ProducesResponseType(400)]
		[ProducesResponseType(401)]
		public IActionResult Post([FromBody] BookDTO book)
		{
			_logger.LogInformation("Creating new Book: {title}", book.Title);
			var createdBook = _bookService.Create(book);
			if (createdBook == null)
			{
				_logger.LogError("Failed to create book with title {title}", book.Title);
				return NotFound();
			}
			Response.Headers.Add("X-API-Deprecated", "true");
			Response.Headers.Add("X-API-Deprecation-Date", "2026-12-31");
			return Ok(createdBook);
		}

		[HttpPut]
		[ProducesResponseType(200, Type = typeof(BookDTO))]
		[ProducesResponseType(400)]
		[ProducesResponseType(401)]
		public IActionResult Update([FromBody] BookDTO book)
		{
			return Ok(_bookService.Update(book));
		}
	}
}