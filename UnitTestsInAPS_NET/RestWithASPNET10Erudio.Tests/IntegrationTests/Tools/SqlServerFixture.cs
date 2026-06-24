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
				.WithPassword("SENHA_REMOVIDA")         // a senha que a gente colocou no SQL(antigo banco)
				.Build();
		}

		public async Task InitializeAsync()             // aonde add as config.de inicialização dos containers
		{
			await Container.StartAsync();
			ConnectionString = Container.GetConnectionString();
			EvolveConfig.ExecuteMigrations(ConnectionString);
		}

		public async Task DisposeAsync()            //com esses 2: a gente sobe e cria o container
		{
			await Container.DisposeAsync();
		}
	}
}