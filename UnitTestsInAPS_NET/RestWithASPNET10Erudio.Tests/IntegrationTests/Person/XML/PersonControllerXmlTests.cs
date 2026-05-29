using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using RestWithASPNET10Erudio.Data.DTO.V1;
using RestWithASPNET10Erudio.DATA.DTO.V1;
using RestWithASPNET10Erudio.Hypermedia.Utils;
using RestWithASPNET10Erudio.Tests.IntegrationTests.Tools;

namespace RestWithASPNET10Erudio.Tests.IntegrationTests.Person.XML
{

	[CollectionDefinition("Sequential", DisableParallelization = true)]  
	public class SequentialCollection : ICollectionFixture<SqlServerFixture> { }

	[Collection("Sequential")]
	[TestCaseOrderer("RestWithASPNET10Erudio.Tests.IntegrationTests.Tools.PriorityOrderer", "RestWithASPNET10Erudio.Tests")]
	public class PersonControllerXmlTests 
	{
		private readonly HttpClient  _httpClient;
		private static PersonDTO _person = null!;
		private static TokenDTO? _token;

		public PersonControllerXmlTests(SqlServerFixture sqlFixture) 
		{
			var factory = new CustomWebApplicationFactory<Program>(
				sqlFixture.ConnectionString);

			_httpClient = factory.CreateClient(
				new WebApplicationFactoryClientOptions
				{
					BaseAddress = new Uri("https://localhost")
				}
			);

			_httpClient.DefaultRequestHeaders.Accept.Clear();
			_httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue ("application/xml"));
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

			var content = XmlHelper.SerializeToXml(credentials);

			//Act = Ação
			var response = await _httpClient
			.PostAsync("api/auth/signin", content);
			if (!response.IsSuccessStatusCode)
			{
				var errorBody = await response.Content.ReadAsStringAsync();
				Console.WriteLine("=========== ERRO DO SERVIDOR ===========");
				Console.WriteLine($"STATUS: {(int)response.StatusCode} {response.StatusCode}");
				Console.WriteLine($"BODY: {errorBody}");
				Console.WriteLine("========================================");
			}
			//Assert = Verificação
			response.EnsureSuccessStatusCode();

			//asserção para verificar se o token foi retornado corretamente
			var token = await XmlHelper
			.ReadFromXmlAsync<TokenDTO>(response);

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
				.PostAsync("api/person/v1", 
				XmlHelper.SerializeToXml(request));

			//Assert = Verificação
			response.EnsureSuccessStatusCode();

			var created = await XmlHelper.ReadFromXmlAsync<PersonDTO>(response);

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

			_person.LastName = "Benedict Torvalds";

			//Act
			var response = await _httpClient
				.PutAsync("api/person/v1", XmlHelper.SerializeToXml(_person));
			
			//assert
			response.EnsureSuccessStatusCode();

			var updated = await XmlHelper.ReadFromXmlAsync<PersonDTO>(response);

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

			//Act
			var response = await _httpClient
				.PatchAsync($"api/person/v1/{_person.Id}", null);

			//assert
			response.EnsureSuccessStatusCode();

			var disabled = await XmlHelper.ReadFromXmlAsync<PersonDTO>(response);

			disabled.Should().NotBeNull();
			disabled.Id.Should().Be(_person.Id);
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

			//act
			var response = await _httpClient
				.GetAsync($"api/person/v1/{_person.Id}");

			// Assert
			response.EnsureSuccessStatusCode();

			var found = await XmlHelper.ReadFromXmlAsync<PersonDTO>(response);

			found.Should().NotBeNull();
			found.Id.Should().Be(_person.Id);
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
			("Bearer", _token?.AccessToken);                    //passa o bearer na request 
			
			//Act
			var response = await _httpClient
				.DeleteAsync($"api/person/v1/{_person.Id}");

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
			("Bearer", _token?.AccessToken);                    //passa o bearer na request 

			//Act
			var response = await _httpClient
				.GetAsync("api/person/v1/asc/10/1");

			//assert
			response.EnsureSuccessStatusCode();

			var page = await XmlHelper
				.ReadFromXmlAsync<PagedSearchDTO<PersonDTO>>(response);

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

