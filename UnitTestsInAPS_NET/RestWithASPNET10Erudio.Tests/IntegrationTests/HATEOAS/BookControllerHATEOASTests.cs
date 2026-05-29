using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.RegularExpressions;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using RestWithASPNET10Erudio.Data.DTO.V1;
using RestWithASPNET10Erudio.DATA.DTO.V1;
using RestWithASPNET10Erudio.Tests.IntegrationTests.Tools;

namespace RestWithASPNET10Erudio.Tests.IntegrationTests.HATEOAS
{
	[TestCaseOrderer(
		TestConfigs.TestCaseOrdererFullName,
		TestConfigs.TestCaseOrdererAssembly)]
	public class BookControllerHATEOASTests : IClassFixture<SqlServerFixture>
	{
		private readonly HttpClient _httpClient;
		private static BookDTO _book = null!;
		private static TokenDTO? _token;

		public BookControllerHATEOASTests(SqlServerFixture sqlFixture)
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
			var pattern = $@"""rel"":\s*""{rel}"".*?""href"":\s*""https?://.+/api/book/v1.*?""";
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
			.ReadFromJsonAsync<TokenDTO>();                             //Lê o conteúdo da resposta e desserializa para um objeto TokenDTO

			token.Should().NotBeNull();                                 //Token não pode ser nulo

			token.AccessToken.Should().NotBeNullOrWhiteSpace();         //Token não pode ser nulo ou vazio
			token.RefreshToken.Should().NotBeNullOrWhiteSpace();        //Token não pode ser nulo ou vazio
			_token = token;
		}

		[Fact(DisplayName = "01 - Create Book")]
		[TestPriority(1)]
		public async Task CreateBook_ShouldContainHateoasLinks()
		{
			//Arrange = Preparação
			_httpClient.DefaultRequestHeaders.Authorization
			= new AuthenticationHeaderValue                     //Configura o cabeçalho de autorização com o token de acesso
			("Bearer", _token?.AccessToken);                    //passa o bearer na request 

			var request = new BookDTO
			{
				Title = "The Pragmatic Programmer",
				Author = "Andrew Hunt",
				Price = 49.99m,
				LaunchDate = new DateTime(1999, 10, 20),
			};

			var response = await _httpClient.PostAsJsonAsync(
				"/api/book/v1", request);

			response.EnsureSuccessStatusCode();
			var content = await response.Content.ReadAsStringAsync();
			_book = await response.Content.ReadFromJsonAsync<BookDTO>();

			AssertLinkPattern(content, "collection");
			AssertLinkPattern(content, "self");
			AssertLinkPattern(content, "create");
			AssertLinkPattern(content, "update");
			AssertLinkPattern(content, "delete");
		}

		[Fact(DisplayName = "02 - Update Book")]
		[TestPriority(2)]
		public async Task UpdateBook_ShouldContainHateoasLinks()
		{
			//Arrange = Preparação
			_httpClient.DefaultRequestHeaders.Authorization
			= new AuthenticationHeaderValue                     //Configura o cabeçalho de autorização com o token de acesso
			("Bearer", _token?.AccessToken);                    //passa o bearer na request 

			_book!.Title= "Docker Deep Dive - 2º Edition";
				
			//act = ação
			var response = await _httpClient.PutAsJsonAsync(
				"/api/book/v1", _book);

			response.EnsureSuccessStatusCode();
			var content = await response.Content.ReadAsStringAsync();
			_book = await response.Content.ReadFromJsonAsync<BookDTO>();

			AssertLinkPattern(content, "collection");
			AssertLinkPattern(content, "self");
			AssertLinkPattern(content, "create");
			AssertLinkPattern(content, "update");
			AssertLinkPattern(content, "delete");
		}

		[Fact(DisplayName = "03 - Get Book By Id")]
		[TestPriority(3)]
		public async Task GetBookById_ShouldContainHateoasLinks()
		{
			//Arrange = Preparação
			_httpClient.DefaultRequestHeaders.Authorization
			= new AuthenticationHeaderValue                     //Configura o cabeçalho de autorização com o token de acesso
			("Bearer", _token?.AccessToken);                    //passa o bearer na request 

			//act = ação
			var response = await _httpClient.GetAsync(
				$"/api/book/v1/{_book!.Id}");
			
			//assert
			response.EnsureSuccessStatusCode();
			var content = await response.Content.ReadAsStringAsync();
			_book = await response.Content.ReadFromJsonAsync<BookDTO>();

			AssertLinkPattern(content, "collection");
			AssertLinkPattern(content, "self");
			AssertLinkPattern(content, "create");
			AssertLinkPattern(content, "update");
			AssertLinkPattern(content, "delete");
		}

		[Fact(DisplayName = "04 - Find All Books")]
		[TestPriority(4)]
		public async Task FindAllBooks_ShouldReturnBooks()
		{
			//Arrange = Preparação
			_httpClient.DefaultRequestHeaders.Authorization
			= new AuthenticationHeaderValue                     //Configura o cabeçalho de autorização com o token de acesso
			("Bearer", _token?.AccessToken);                    //passa o bearer na request 

			//act = ação
			var response = await _httpClient.GetAsync("/api/book/v1");

			//assert
			response.EnsureSuccessStatusCode();
			var books = await response.Content.ReadFromJsonAsync<List<BookDTO>>();


			books.Should().NotBeNull();
			books.Should().NotBeEmpty();
		}

		[Fact(DisplayName = "05 - Delete Book By Id")]
		[TestPriority(5)]
		public async Task DeleteBookById_ShouldReturnNoContent()
		{
			//Arrange = Preparação
			_httpClient.DefaultRequestHeaders.Authorization
			= new AuthenticationHeaderValue                     //Configura o cabeçalho de autorização com o token de acesso
			("Bearer", _token?.AccessToken);                    //passa o bearer na request 

			//act = ação
			var response = await _httpClient.DeleteAsync(
				$"/api/book/v1/{_book!.Id}");

			//assert
			response.EnsureSuccessStatusCode();
			response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
		}
	}
}