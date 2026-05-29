using FluentAssertions;
using RestWithASPNET10Erudio.Data.Converter.Impl;
using RestWithASPNET10Erudio.Data.DTO.V1;
using RestWithASPNET10Erudio.Model;
using RestWithASPNET10Erudio.Services.Impl;

namespace RestWithASPNET10Erudio.Tests.UnitTests
{
	public class PersonConverterTests
	{

		private readonly PersonConverter _converter;

		public PersonConverterTests()
		{
			_converter = new PersonConverter();
		}

		// PersonDTO to Person conversions tests
		[Fact]
		public void Parse_ShouldConvertPersonDTOToPerson()
		{
			// entrada
			var dto = new PersonDTO
			{
				Id = 1,
				FirstName = "Mahatma",
				LastName = "Gandhi",
				Address = "Porbandan - India",
				Gender = "Male",
				BirthDay = new DateTime(1869, 10, 2)
			};


			var expectedPerson = new Person
			{
				Id = 1,
				FirstName = "Mahatma",
				LastName = "Gandhi",
				Address = "Porbandan - India",
				Gender = "Male"
			};

			// Act: chama a funçaoe aciona ela
			var person = _converter.Parse(dto);

			person.Should().NotBeNull();
			person.Id.Should().Be(expectedPerson.Id);
			person.FirstName.Should().Be(expectedPerson.FirstName);
			person.LastName.Should().Be(expectedPerson.LastName);
			person.Gender.Should().Be(expectedPerson.Gender);
			person.Should().BeEquivalentTo(expectedPerson);
		}
		[Fact]
		public void Parse_NullPersonDTOShouldReturnNull()
		{
			PersonDTO? dto = null;
			var person = _converter.Parse(dto);
			person.Should().BeNull();
		}
		[Fact]
		public void Parse_ShouldConvertPersonToPersonDTO()
		{
			// entrada
			var entity = new Person
			{
				Id = 1,
				FirstName = "Mahatma",
				LastName = "Gandhi",
				Address = "Porbandan - India",
				Gender = "Male",
				//BirthDay = new DateTime(1869, 10, 2)
			};


			var expectedPerson = new PersonDTO
			{
				Id = 1,
				FirstName = "Mahatma",
				LastName = "Gandhi",
				Address = "Porbandan - India",
				Gender = "Male"
			};

			// Act: chama a funçaoe aciona ela
			var person = _converter.Parse(entity);

			person.Should().NotBeNull();
			person.Id.Should().Be(expectedPerson.Id);
			person.FirstName.Should().Be(expectedPerson.FirstName);
			person.LastName.Should().Be(expectedPerson.LastName);
			person.Gender.Should().Be(expectedPerson.Gender);
			person.Should().BeEquivalentTo(expectedPerson);
			person.Should().BeEquivalentTo(expectedPerson,
				options => options.Excluding(person => person.BirthDay));

		}



		[Fact]
		public void Parse_NullPersonShouldReturnNull()
		{
			Person? dto = null;
			var person = _converter.Parse(dto);
			person.Should().BeNull();
		}


		[Fact]
		public void ParseList_ShouldConvertPersonDTOListToPersonList()
		{
			var dtoList = new List<PersonDTO>
	{
		new PersonDTO
		{
			Id = 1,
			FirstName = "Mahatma",
			LastName = "Gandhi",
			Address = "Porbandar",
			Gender = "Male"
		},
		new PersonDTO
		{
			Id = 2,
			FirstName = "Indira",
			LastName = "Gandhi",
			Address = "Allahabad - India",
			Gender = "Female"
		}
	};

			// Act
			var personList = _converter.ParseList(dtoList);  // ← dentro do método!
			personList.Should().NotBeNull();
			personList.Should().HaveCount(2);

			personList[0].Should().BeEquivalentTo(new Person
			{
				Id = 1,
				FirstName = "Mahatma",
				LastName = "Gandhi",
				Address = "Porbandar",
				Gender = "Male",
			});
			personList[1].Should().BeEquivalentTo(new Person 
			{
				Id = 2,
				FirstName = "Indira",
				LastName = "Gandhi",
				Address = "Allahabad - India",
				Gender = "Female"
			});


			personList[0].FirstName.Should().Be("Mahatma");
			personList[1].FirstName.Should().Be("Indira");
			personList[1].LastName.Should().Be("Gandhi");

		}

		[Fact]
		public void Parse_NullListPersonShouldReturnNull()
		{
			List<PersonDTO>? dto = null;
			var listPerson = _converter.ParseList(dto);
			listPerson.Should().BeNull();
		}


	}
}
