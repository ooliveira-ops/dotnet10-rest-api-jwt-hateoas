using System.Net;
using RestWithASPNET10Erudio.Configurations;
using Testcontainers.MySql;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace RestWithASPNET10Erudio.Tests.IntegrationTests.Tools
{
	public class SqlServerFixture : IAsyncLifetime
	{
		public MySqlContainer Container { get; private set; }

		public string ConnectionString { get; internal set; } = string.Empty;

		public SqlServerFixture()
		{
			Container = new MySqlBuilder("mysql:8.0")
				.WithPassword("Test@123456")         // a senha que a gente colocou bancode teste
				.Build();
		}

		public async Task InitializeAsync()             // aonde add as config.de inicialização dos containers
		{
			await Container.StartAsync();
			ConnectionString = Container.GetConnectionString();
			Console.WriteLine($"[DEBUG] Connection String: {ConnectionString}");
			EvolveConfig.ExecuteMigrations(ConnectionString);
		}

		public async Task DisposeAsync()            //com esses 2: a gente sobe e cria o container
		{
			await Container.DisposeAsync();
		}
	}
}