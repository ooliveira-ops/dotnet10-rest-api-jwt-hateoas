using RestWithASPNET10Erudio.Repositories;
using RestWithASPNET10Erudio.Data.Converter.Impl;
using RestWithASPNET10Erudio.Model;
using RestWithASPNET10Erudio.Data.DTO.V2;
using RestWithASPNET10Erudio.Services;

namespace RestWithASPNET10Erudio.Services.Impl
{
	public class PersonServicesV2 : IPersonServicesV2
	{
		private IRepository<Person> _repository;
		private readonly PersonConverterV2 _converter;

		public PersonServicesV2(IRepository<Person> repository)
		{
			_repository = repository;
			_converter = new PersonConverterV2();
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
			entity = _repository.Update(entity);
			return _converter.Parse(entity);
		}

		public void Delete(long id)
		{
			_repository.Delete(id);
		}
	}
}