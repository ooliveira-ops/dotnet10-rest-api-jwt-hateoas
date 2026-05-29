using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using DocumentFormat.OpenXml.Bibliography;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using RestWithASPNET10Erudio.Data.DTO.V1;
using RestWithASPNET10Erudio.DATA.DTO.V1;
using RestWithASPNET10Erudio.Hypermedia.Utils;
using RestWithASPNET10Erudio.Tests.IntegrationTests.Tools;

namespace RestWithASPNET10Erudio.Tests.IntegrationTests.Person.JSON
{

	[TestCaseOrderer(
		TestConfigs.TestCaseOrdererFullName,
		TestConfigs.TestCaseOrdererAssembly)]
	public class PersonControllerJsonTests : IClassFixture<SqlServerFixture>
	{
		private readonly HttpClient  _httpClient;
		private static PersonDTO _person = null!;
		private static TokenDTO? _token; 

		public PersonControllerJsonTests(SqlServerFixture sqlFixture) 
		{
			var factory = new CustomWebApplicationFactory<Program>(
				sqlFixture.ConnectionString);

			_httpClient = factory.CreateClient(
				new WebApplicationFactoryClientOptions
				{
					BaseAddress = new Uri("https://localhost")
				}
			);
		}


		[Fact(DisplayName = "00 - Sign In")]        //vai testar o endpoint de signin
		[TestPriority(0)]
		public async Task SignIn_ShouldReturnToken()
		{
			//Arrange = Preparação
			var credentials = new UserDTO
			{ 
				Username = "leandro",
				Password = "admin123"
			};

			//Act = Ação
			var response = await _httpClient
			.PostAsJsonAsync("api/auth/signin", credentials);


			//Assert = Verificação
			response.EnsureSuccessStatusCode();

			var token = await response.Content
			.ReadFromJsonAsync<TokenDTO>();

			token.Should().NotBeNull();                                 //Token não pode ser nulo

			token.AccessToken.Should().NotBeNullOrWhiteSpace();         //Token não pode ser nulo ou vazio
			token.RefreshToken.Should().NotBeNullOrWhiteSpace();        //Token não pode ser nulo ou vazio
			_token = token;
		}

		[Fact(DisplayName = "01 - Create Person")]
		[TestPriority(1)]
		public async Task CreatePerson_ShouldReturnCreatedPerson()
		{ 
			//Arrange = Preparação
			_httpClient.DefaultRequestHeaders.Authorization
			= new AuthenticationHeaderValue                     //Configura o cabeçalho de autorização com o token de acesso
			("Bearer", _token?.AccessToken);                    //passa o bearer na request 

			var request = new  PersonDTO
			{
				FirstName = "Linus",
				LastName = "Torvalds",
				Address = "Helsinki - Finland",
				Gender = "Male",
				Enabled = true
			};

			//Act = Ação
			var response = await _httpClient
				.PostAsJsonAsync("api/person/v1", request);
			
			//assert

			response.EnsureSuccessStatusCode();

			var created = await response.Content
				.ReadFromJsonAsync<PersonDTO>();
			created.Should().NotBeNull();
			created.Id.Should().BeGreaterThan(0);
			created.FirstName.Should().Be("Linus");
			created.LastName.Should().Be("Torvalds");
			created.Address.Should().Be("Helsinki - Finland");
			created.Enabled.Should().BeTrue();

			_person = created;
		}

		[Fact(DisplayName = "02 - Update Person")]
		[TestPriority(2)]
		public async Task UpdatePerson_ShouldReturnUpdatedPerson()
		{
			//Arrange = Preparação
			_httpClient.DefaultRequestHeaders.Authorization
			= new AuthenticationHeaderValue                     //Configura o cabeçalho de autorização com o token de acesso
			("Bearer", _token?.AccessToken);                    //passa o bearer na request 

			_person.Should().NotBeNull("_person deveria ter sido criado no teste 01");

			_person?.LastName = "Benedict Torvalds";

			//act
			var response = await _httpClient
				.PutAsJsonAsync("api/person/v1", _person);

			//assert

			response.EnsureSuccessStatusCode();

			var updated = await response.Content
				.ReadFromJsonAsync<PersonDTO>();
			updated.Should().NotBeNull();
			updated.Id.Should().BeGreaterThan(0);
			updated.FirstName.Should().Be("Linus");
			updated.LastName.Should().Be("Benedict Torvalds");
			updated.Address.Should().Be("Helsinki - Finland");
			updated.Enabled.Should().BeTrue();

			_person = updated;
		}


		[Fact(DisplayName = "03 - Disable Person By ID")]
		[TestPriority(3)]
		public async Task DisablePerson_ShouldReturnDisabledPerson()
		{
			//Arrange = Preparação
			_httpClient.DefaultRequestHeaders.Authorization
			= new AuthenticationHeaderValue                     //Configura o cabeçalho de autorização com o token de acesso
			("Bearer", _token?.AccessToken);                    //passa o bearer na request 


			//act
			var response = await _httpClient
				.PatchAsync($"api/person/v1/{_person?.Id}", null);

			//assert
			response.EnsureSuccessStatusCode();

			var disabled = await response.Content
				.ReadFromJsonAsync<PersonDTO>();

			disabled.Should().NotBeNull();
			disabled.Id.Should().BeGreaterThan(0);
			disabled.FirstName.Should().Be("Linus");
			disabled.LastName.Should().Be("Benedict Torvalds");
			disabled.Address.Should().Be("Helsinki - Finland");
			disabled.Enabled.Should().BeFalse();

			_person = disabled;
		}



		[Fact(DisplayName = "04 - Get Person By ID ")]
		[TestPriority(4)]
		public async Task GetPersonById_ShouldReturnPerson()
		{
			//Arrange = Preparação
			_httpClient.DefaultRequestHeaders.Authorization
			= new AuthenticationHeaderValue                     //Configura o cabeçalho de autorização com o token de acesso
			("Bearer", _token?.AccessToken);                    //passa o bearer na request 

			// Act
			var response = await _httpClient
				.GetAsync($"api/person/v1/{_person?.Id}");

			// Assert
			response.EnsureSuccessStatusCode();

			var found = await response.Content
				.ReadFromJsonAsync<PersonDTO>();

			found.Should().NotBeNull();
			found.Id.Should().Be(_person?.Id);
			found.FirstName.Should().Be("Linus");
			found.LastName.Should().Be("Benedict Torvalds");
			found.Address.Should().Be("Helsinki - Finland");
			found.Enabled.Should().BeFalse();
		}

		[Fact(DisplayName = "05 - Delete Person By ID ")]
		[TestPriority(5)]
		public async Task DeletePersonById_ShouldReturnNoContent()
		{
			//Arrange = Preparação
			_httpClient.DefaultRequestHeaders.Authorization
			= new AuthenticationHeaderValue                     //Configura o cabeçalho de autorização com o token de acesso
			("Bearer", _token?.AccessToken);                    //passa o bearer na request  & Act

			//act
			var response = await _httpClient
				.DeleteAsync($"api/person/v1/{_person?.Id}");
			
			//assert 
			response.StatusCode.Should().Be(HttpStatusCode.NoContent);
		}
		
		[Fact(DisplayName = "06 - Find All Persons")]
		[TestPriority(6)]
		public async Task FindAllPersons_ShouldReturnListOfPerson()
		{
		//Arrange = Preparação
			_httpClient.DefaultRequestHeaders.Authorization
			= new AuthenticationHeaderValue                     //Configura o cabeçalho de autorização com o token de acesso
			("Bearer", _token?.AccessToken);                    //passa o bearer na request  & Act Act

			//act
			var response = await _httpClient
				.GetAsync("api/person/v1/asc/10/1"); 

			//assert
			response.EnsureSuccessStatusCode();

			var page = await response.Content
				.ReadFromJsonAsync<PagedSearchDTO<PersonDTO>>();


			page.Should().NotBeNull();
			page.CurrentPage.Should().Be(1);

			var list = page?.List;

			list.Should().NotBeNull();
			list.Count.Should().BeGreaterThan(0);

			var names = string.Join(", ", list.Select(p => p.FirstName));
			Console.WriteLine($"Nomes retornados: {names}");

			var first = list.First(p => p.FirstName == "Abbie");
			first.LastName.Should().Be("Ruddock");
			first.Address.Should().Be("Suite 85");
			first.Enabled.Should().BeTrue();
			first.Gender.Should().Be("Female");

			var third = list.First(p => p.FirstName == "Abrahan");
			third.LastName.Should().Be("Ingarfield");
			third.Address.Should().Be("Apt 1342");
			third.Enabled.Should().BeTrue();
			third.Gender.Should().Be("Male"); 

			page?.CurrentPage.Should().BeGreaterThan(0);
			page?.TotalResults.Should().BeGreaterThan(0);
			page?.PageSize.Should().BeGreaterThan(0);
			page?.SortDirections.Should().NotBeNull();

		}
	}
}

