using RestWithASPNET10Erudio.Data.DTO.V1;
using RestWithASPNET10Erudio.Model;

namespace RestWithASPNET10Erudio.Data.Converter.Impl
{
	public class PersonConverter : IParser<PersonDTO, Person>, IParser<Person, PersonDTO>
	{
		public object FindAll { get; internal set; }

		public Person Parse(PersonDTO origin)
		{

			if (origin == null) return null;
			return new Person
			{
				Id = origin.Id,
				FirstName = origin.FirstName,
				LastName = origin.LastName,
				Address = origin.Address,
				Gender = origin.Gender,
				Enabled = origin.Enabled
			};
	}

		public PersonDTO Parse(Person origin)
		{
			 
			if (origin == null) return null;
			return new PersonDTO
			{
				Id = origin.Id,
				FirstName = origin.FirstName,
				LastName = origin.LastName,
				Address = origin.Address,
				Gender = origin.Gender,
				Enabled = origin.Enabled
			};
		}

		public object Parse(DTO.V2.PersonDTO personDto)
		{
			throw new NotImplementedException();
		}

		public List<Person> ParseList(List<PersonDTO> origin)
		{
			if (origin == null) return null;
			return origin.Select(item => Parse(item)).ToList();
		}
		public List<PersonDTO> ParseList(List<Person> origin)
		{
			if (origin == null) return null;
			return origin.Select(item => Parse(item)).ToList();
		}

		public object ParseList(object dtoList)
		{
			throw new NotImplementedException();
		}
	}


	public interface IParser<T1, T2>
	{
	}
}
