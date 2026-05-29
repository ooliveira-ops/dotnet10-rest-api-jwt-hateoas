using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using RestWithASPNET10Erudio.Data.Converter.Impl;
using RestWithASPNET10Erudio.Data.DTO.V1;
using RestWithASPNET10Erudio.FIles.Exporters.Factory;
using RestWithASPNET10Erudio.FIles.Importers.Factory;
using RestWithASPNET10Erudio.Hypermedia.Utils;
using RestWithASPNET10Erudio.Model;
using RestWithASPNET10Erudio.Repositories;
using RestWithASPNET10Erudio.Services;

namespace RestWithASPNET10Erudio.Services.Impl
{
	public class PersonServicesImplV1 : IPersonServices
	{
		private IPersonRepository _repository;
		private readonly FileImporterFactory _fileImporterFactory;
		private readonly FileExporterFactory _fileExporterFactory;
		private readonly PersonConverter _converter;
		private readonly ILogger<PersonServicesImplV1> _logger;

		public PersonServicesImplV1(
			IPersonRepository repository,
			FileImporterFactory fileImporterFactory,
			FileExporterFactory fileExporterFactory,
			ILogger<PersonServicesImplV1> logger)
		{
			_fileExporterFactory = fileExporterFactory;
			_fileImporterFactory = fileImporterFactory;
			_repository = repository;
			_converter = new PersonConverter();
			_logger = logger;
		}

		public List<PersonDTO> FindALL()
		{
			return _converter.ParseList(_repository.FindALL());
		}

		public PersonDTO FindByID(long id)
		{
			return _converter.Parse(_repository.FindByID(id));
		}

		public PersonDTO Create(PersonDTO person)
		{
			var entity = _converter.Parse(person);
			entity = _repository.Create(entity);
			return _converter.Parse(entity);
		}

		public PersonDTO Update(PersonDTO person)
		{
			var entity = _converter.Parse(person);
			entity = _repository.Update(entity); // Uptade -> Update
			return _converter.Parse(entity);
		}

		public void Delete(long id)
		{
			_repository.Delete(id);
		}

		public PersonDTO Disable(long id)
		{
			var entity = _repository.Disable(id);
			return entity.Adapt<PersonDTO>();
		}

		public List<PersonDTO> FindByName(
			string firstName, string lastName)
		{
			return _converter.ParseList(_repository.FindByName(firstName, lastName));
		}

		public PagedSearchDTO<PersonDTO> FindWithPagedSearch(
			string name, string sortDirection, int pageSize, int page)
		{
			var result = _repository.FindWithPagedSearch(name, sortDirection, pageSize, page);
			return result.Adapt<PagedSearchDTO<PersonDTO>>();
		}

		public async Task<List<PersonDTO>> MassCreationAsync(IFormFile file)
		{
			if (file == null || file.Length == 0)
			{
				_logger.LogError("File Is null or Empty");
				throw new ArgumentException("File Is null or Empty");
			}

			using var stream = file.OpenReadStream();
			var fileName = file.FileName;
			try
			{
				var importer = _fileImporterFactory.GetImporter(file.FileName);
				var persons = await importer.ImportFileAsync(stream);

				var entities = persons
					.Select(dto => _repository.Create(dto.Adapt<Person>())).ToList();
				return entities.Adapt<List<PersonDTO>>();
			}
			catch (Exception ex) {
				_logger.LogError(ex,"Error during mass creation from file");
				throw;
			}
		}

		public FileContentResult ExportPage(
			int page,
			int pageSize,
			string sortDirection,
			string acceptHeader,
			string name)
		{
			//			_logger.LogInformation("Exporting page: {page}, {pageSize}, {sortDirection}, {aceeptHeader");
			var content = FindWithPagedSearch(
				name, sortDirection, pageSize, page);

			try
			{
				var exporter = _fileExporterFactory.GetExporter(acceptHeader);
				var people = content.List.Adapt<List<PersonDTO>>();
				return exporter.ExportFile(people); 
			}
			catch (NotSupportedException ex)
			{
				_logger.LogError(ex, "Unsupported export format requested: {AcceptHeader}", acceptHeader);
				throw;
			}
		}
	}
}