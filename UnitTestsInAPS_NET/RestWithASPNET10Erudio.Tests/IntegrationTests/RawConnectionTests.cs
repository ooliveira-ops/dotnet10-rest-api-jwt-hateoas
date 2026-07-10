using MySqlConnector;
using RestWithASPNET10Erudio.Configurations;
using RestWithASPNET10Erudio.Tests.IntegrationTests.Tools;
using Xunit;
using Xunit.Abstractions;

namespace RestWithASPNET10Erudio.Tests.IntegrationTests
{
	[Collection("Sequential")]
	public class RawConnectionTests
	{
		private readonly SqlServerFixture _fixture;
		private readonly ITestOutputHelper _output;

		public RawConnectionTests(SqlServerFixture fixture, ITestOutputHelper output)
		{
			_fixture = fixture;
			_output = output;
		}

		[Fact]
		public void DeveConectarDiretoSemEvolve()
		{
			_output.WriteLine($"Connection String: {_fixture.ConnectionString}");
			using var conn = new MySqlConnection(_fixture.ConnectionString);
			conn.Open();
			_output.WriteLine($"Conectou! Estado: {conn.State}");
			Assert.Equal(System.Data.ConnectionState.Open, conn.State);
		}

		[Fact]
		public void DeveRodarEvolveSemWebApplicationFactory()
		{
			_output.WriteLine($"Connection String: {_fixture.ConnectionString}");

			// Chama o Evolve direto, sem subir a aplicação inteira
			EvolveConfig.ExecuteMigrations(_fixture.ConnectionString);

			_output.WriteLine("Evolve rodou sem erro!");
		}
	}
}