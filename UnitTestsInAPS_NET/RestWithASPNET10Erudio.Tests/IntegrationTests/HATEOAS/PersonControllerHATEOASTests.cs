using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.RegularExpressions;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Identity.Client;
using RestWithASPNET10Erudio.Data.DTO.V1;
using RestWithASPNET10Erudio.DATA.DTO.V1;
using RestWithASPNET10Erudio.Tests.IntegrationTests.Tools;

namespace RestWithASPNET10Erudio.Tests.IntegrationTests.HATEOAS
{
	[TestCaseOrderer(
		TestConfigs.TestCaseOrdererFullName,
		TestConfigs.TestCaseOrdererAssembly)]
	public class PersonControllerHATEOASTests : IClassFixture<SqlServerFixture>
	{
		private readonly HttpClient _httpClient;
		private static PersonDTO _person = null!;
		private static TokenDTO? _token;


		public PersonControllerHATEOASTests(SqlServerFixture sqlFixture)
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

		private void AssertLinkPattern(string content, string rel)
		{
			var pattern = $@"""rel"":\s*""{rel}"".*?""href"":\s*""https?://.+/api/person/v1.*?""";
			Regex.IsMatch(content, pattern).Should().BeTrue($"Link with rel='{rel}' should exist and have valid href");
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

			//asserção para verificar se o token foi retornado corretamente
			var token = await response.Content
			.ReadFromJsonAsync<TokenDTO>();

			token.Should().NotBeNull();                                 //Token não pode ser nulo

			token.AccessToken.Should().NotBeNullOrWhiteSpace();         //Token não pode ser nulo ou vazio
			token.RefreshToken.Should().NotBeNullOrWhiteSpace();        //Token não pode ser nulo ou vazio
			_token = token;
		}

		[Fact(DisplayName = "01 - Create Person")]
		[TestPriority(1)]
		public async Task CreatePerson_ShouldContainHateoasLinks()
		{
			//Arrange = Preparação
			_httpClient.DefaultRequestHeaders.Authorization
			= new AuthenticationHeaderValue                     //Configura o cabeçalho de autorização com o token de acesso
			("Bearer", _token?.AccessToken);                    //passa o bearer na request 

			var request = new PersonDTO
			{
				FirstName = "David",
				LastName = "Heinemeier",
				Address = "Copenhagen, Denmark",
				Gender = "Male",
				Enabled = true,
			};

			//Act = Ação
			var response = await _httpClient.PostAsJsonAsync(
				"/api/person/v1", request);


			//Assert = Verificação
			response.EnsureSuccessStatusCode();
			var content = await response.Content.ReadAsStringAsync();
			_person = await response.Content.ReadFromJsonAsync<PersonDTO>();

			AssertLinkPattern(content, "collection");
			AssertLinkPattern(content, "self");
			AssertLinkPattern(content, "create");
			AssertLinkPattern(content, "update");
			AssertLinkPattern(content, "delete");

		}


		[Fact(DisplayName = "02 - Update Person")]
		[TestPriority(2)]
		public async Task UpdatePerson_ShouldContainHateoasLinks()
		{
			//Arrange = Preparação
			_httpClient.DefaultRequestHeaders.Authorization
			= new AuthenticationHeaderValue                     //Configura o cabeçalho de autorização com o token de acesso
			("Bearer", _token?.AccessToken);                    //passa o bearer na request 

			_person!.LastName = "Heinemeier Hansson";

			//Act = Ação
			var response = await _httpClient.PutAsJsonAsync(
				"/api/person/v1", _person);


			//Assert = Verificação
			response.EnsureSuccessStatusCode();
			var content = await response.Content.ReadAsStringAsync();
			_person = await response.Content.ReadFromJsonAsync<PersonDTO>();

			AssertLinkPattern(content, "collection");
			AssertLinkPattern(content, "self");
			AssertLinkPattern(content, "create");
			AssertLinkPattern(content, "update");
			AssertLinkPattern(content, "delete");
		}

		[Fact(DisplayName = "03 - Disable Person By Id")]
		[TestPriority(3)]
		public async Task DisablePersonById_ShouldContainHateoasLinks()
		{
			//Arrange = Preparação
			_httpClient.DefaultRequestHeaders.Authorization
			= new AuthenticationHeaderValue                     //Configura o cabeçalho de autorização com o token de acesso
			("Bearer", _token?.AccessToken);                    //passa o bearer na request 


			//Act = Ação
			var response = await _httpClient.PatchAsync(
				$"/api/person/v1/{_person!.Id}", null);

			//Assert = Verificação
			response.EnsureSuccessStatusCode();

			var content = await response.Content.ReadAsStringAsync();

			_person = await response.Content.ReadFromJsonAsync<PersonDTO>();

			AssertLinkPattern(content, "collection");
			AssertLinkPattern(content, "self");
			AssertLinkPattern(content, "create");
			AssertLinkPattern(content, "update");
			AssertLinkPattern(content, "delete");
		}


		[Fact(DisplayName = "05 - Find Paged Persons with HATEOAS")]
		[TestPriority(5)]
		public async Task FindAll_ShouldReturnLinksForEachPerson()
		{
			//Arrange = Preparação
			_httpClient.DefaultRequestHeaders.Authorization
			= new AuthenticationHeaderValue                     //Configura o cabeçalho de autorização com o token de acesso
			("Bearer", _token?.AccessToken);                    //passa o bearer na request 
			
			//Act = Ação
			var response = await _httpClient
				.GetAsync("api/person/v1/asc/10/1");// Ensures the response status code is 2xx.

			// Read the response content as a string.
			var content = await response.Content.ReadAsStringAsync();

			
			// Assert = Verificação
			// Extract all "id" values from the response JSON using Regex.
			var idMatches = Regex.Matches(content, @"""list"":\s*\[\s*{[^}]*""id"":\s*(\d+)");
			idMatches.Count.Should().BeGreaterThan(0, "There should be at least one person");

			// Iterate through each person id found in the response.
			foreach (Match match in idMatches)
			{
				var id = match.Groups[1].Value;

				// Expected hypermedia relations (HATEOAS links).
				var expectedRels = new[] { "collection", "self", "create", "update", "patch", "delete" };

				foreach (var rel in expectedRels)
				{
					// Build the expected regex pattern depending on the relation.
					// For "self" and "delete", the link must contain the specific id.
					// For others, the link points to the base endpoint.
					var pattern = rel switch
					{
						"self" or "delete" or "patch" =>
							$@"""rel"":\s*""{rel}"".*?""href"":\s*""https?://.+/api/person/v1/{id}""",
						_ =>
							$@"""rel"":\s*""{rel}"".*?""href"":\s*""https?://.+/api/person/v1"""
					};

					// Assert that the link with the correct "rel" and "href" exists.
					Regex.IsMatch(content, pattern, RegexOptions.IgnoreCase)
						 .Should().BeTrue($"Link '{rel}' should exist for person {id}");

					// Assert that each link also contains a "type" attribute.
					var typePattern = $@"""rel"":\s*""{rel}"".*?""type"":\s*""[^""]+""";
					Regex.IsMatch(content, typePattern)
						 .Should().BeTrue($"Link '{rel}' must have a type for person {id}");
				}
			}
		}
	}
}

	