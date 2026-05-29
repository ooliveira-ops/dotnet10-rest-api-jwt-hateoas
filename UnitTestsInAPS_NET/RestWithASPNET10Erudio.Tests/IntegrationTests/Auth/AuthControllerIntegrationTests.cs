using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using RestWithASPNET10Erudio.DATA.DTO.V1;
using RestWithASPNET10Erudio.Tests.IntegrationTests.Tools;

namespace RestWithASPNET10Erudio.Tests.IntegrationTests.Auth
{
	[TestCaseOrderer(
		TestConfigs.TestCaseOrdererFullName,
		TestConfigs.TestCaseOrdererAssembly)]
	public class AuthControllerIntegrationTests : IClassFixture<SqlServerFixture>
	{
		private readonly HttpClient _httpClient;
		private static TokenDTO? _token;
		private static AccountCredentialsDTO? _createduser;

		public AuthControllerIntegrationTests(SqlServerFixture sqlFixture)
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

		[Fact(DisplayName = "01 - Create User")]        //vai testar o endpoint de create
		[TestPriority(1)]
		public async Task CreateUser_ShouldReturnCreatedUser()
		{
			//Arrange = Preparação

			var request = new AccountCredentialsDTO
			{
				UserName = "solomon",
				FullName = "Solomon Hykes",
				Password = "hykes123"
			};

			//Act = Ação

			var response = await _httpClient
				.PostAsJsonAsync("api/auth/create", request);

			//Assert = Verificação

			response.EnsureSuccessStatusCode();

			var result = await response.Content
				.ReadFromJsonAsync<AccountCredentialsDTO>();

			result.Should().NotBeNull();
			result.UserName.Should().Be("solomon");
			result.FullName.Should().Be("Solomon Hykes");

			_createduser = result;
		}


		[Fact(DisplayName = "02 - Sign In")]        //vai testar o endpoint de signin
		[TestPriority(2)]
		public async Task SignIn_ShouldReturnToken()
		{
			//Arrange = Preparação
			var credentials = new UserDTO
			{
				Username = "solomon",
				Password = "hykes123"
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

		[Fact(DisplayName = "03 - Refresh Token")]        //vai testar o endpoint de refresh token
		[TestPriority(3)]
		public async Task RefreshToken_ShouldReturnNewToken()
		{
			//Arrange(Preparação) = não tem, pois ja tem o token completo
			//Usa o token setado acima

			//Act = Ação
			var response = await _httpClient
			.PostAsJsonAsync("api/auth/refresh", _token);


			//Assert = Verificação 
			response.EnsureSuccessStatusCode();

			var newToken = await response.Content
			.ReadFromJsonAsync<TokenDTO>();

			newToken.Should().NotBeNull();                                      //Novo token não pode ser nulo
			newToken.AccessToken.Should().NotBeNullOrWhiteSpace();              //Novo token de acesso não pode ser nulo ou vazio
			newToken.RefreshToken.Should().NotBeNullOrWhiteSpace();             //Novo token de atualização não pode ser nulo ou vazio
			newToken.AccessToken.Should().NotBe(_token?.AccessToken);            //Novo token de acesso deve ser diferente do token anterior
			newToken.RefreshToken.Should().NotBe(_token?.RefreshToken);         //Novo token de atualização deve ser diferente do token anterior

			_token = newToken;
		}

		[Fact(DisplayName = "04 - Revoke Token")]        //vai testar o endpoint de revoke token
		[TestPriority(4)]
		public async Task RevokeToken_ShouldReturnNoContent()
		{
			//Arrange = Preparação
			_httpClient.DefaultRequestHeaders.Authorization
			= new AuthenticationHeaderValue                     //Configura o cabeçalho de autorização com o token de acesso
			("Bearer", _token?.AccessToken);                    //passa o bearer na request 

			//Act = Ação
			var response = await _httpClient
			.PostAsync("api/auth/revoke", null);

			//Assert = Verificação
			response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);     //O status code deve ser NoContent (204)
		}

		[Fact(DisplayName = "05 - Sign In with Invalid Credentials ShouldReturnUnauthorized")]        //vai testar o endpoint de signin com token revogado
		[TestPriority(5)]
		public async Task SignInWithRevokedToken_ShouldReturnUnauthorized()
		{
			//Arrange = Preparação
			var credentials = new UserDTO
			{
				Username = "solomon",
				Password = "wrongpassword"
			};

			// Act = Ação
			var response = await _httpClient
			.PostAsJsonAsync("api/auth/signin", credentials);

			// Assert = Verificação
			response.StatusCode.Should()
			.Be(System.Net.HttpStatusCode.Unauthorized);     //O status code deve ser Unauthorized (401)
		}

		[Fact(DisplayName = "06 - Rovoke Token without Authorization Header ShouldReturnUnauthorized")]        //vai testar o endpoint de revoke token sem passar o token de acesso no cabeçalho de autorização, ou seja, sem autenticação
		[TestPriority(6)]
		public async Task RevokeTokenWithoutAuthorizationHeader_ShouldReturnUnauthorized()
		{
			_httpClient.DefaultRequestHeaders.Authorization = null;     //Remove o cabeçalho de autorização para simular a ausência de autenticação

			//Act = Ação
			var response = await _httpClient
			.PostAsync("api/auth/revoke", null);                //como é "null" ele não tem o token de acesso para revogar

			//Assert = Verificação
			response.StatusCode.Should()
			.Be(System.Net.HttpStatusCode.Unauthorized);     //O status code deve ser Unauthorized (401) 
		}
	}
}
