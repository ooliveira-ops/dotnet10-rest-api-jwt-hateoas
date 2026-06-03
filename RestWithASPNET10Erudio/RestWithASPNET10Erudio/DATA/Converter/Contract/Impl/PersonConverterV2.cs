using RestWithASPNET10Erudio.Data.DTO.V2;
using RestWithASPNET10Erudio.Model;

namespace RestWithASPNET10Erudio.Data.Converter.Impl
{
	public class PersonConverterV2 : IParser<PersonDTO, Person>, IParser<Person, PersonDTO>
	{
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
				//BirthDay = origin.BirthDay
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
				//BirthDay = origin.BirthDay.HasValue ? origin.BirthDay : null
			};
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
	}
}
