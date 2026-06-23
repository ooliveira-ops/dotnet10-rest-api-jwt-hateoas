using EvolveDb;
using MySqlConnector;
using Serilog;

namespace RestWithASPNET10Erudio.Configurations
{
	public static class EvolveConfig
	{
		public static IServiceCollection AddEvolveConfiguration(
			this IServiceCollection services,
			IConfiguration configuration,
			IWebHostEnvironment environment)
		{
			if (environment.IsDevelopment())
			{
				var connectionString = configuration.GetConnectionString(
					"MySQLServerSqlConnectionStrings");
				if (string.IsNullOrEmpty(connectionString))
				{
					throw new ArgumentNullException(
						"Connection string 'MySQLServerSqlConnectionStrings' not found");
				}

				try
				{
					ExecuteMigrationsWithRetry(connectionString, maxRetries: 5);
				}
				catch (Exception ex)
				{
					Log.Error(ex, "An error occurred while migrating the database after multiple retries.");
					throw;
				}
			}
			return services;
		}

		public static void ExecuteMigrations(string connectionString)
		{
			using var evolveConnection = new MySqlConnection(connectionString);
			var evolve = new Evolve(
				evolveConnection,
				msg => Log.Information(msg))
			{
				Locations = new List<string> { "db/migrations", "db/dataset" },
				IsEraseDisabled = true,
			};
			evolve.Migrate();
		}

		public static void ExecuteMigrationsWithRetry(string connectionString, int maxRetries = 5)
		{
			int attempt = 0;
			int delayMs = 2000;

			while (attempt < maxRetries)
			{
				try
				{
					attempt++;
					Log.Information($"Attempting to execute migrations (Attempt {attempt}/{maxRetries})...");
					ExecuteMigrations(connectionString);
					Log.Information("Migrations executed successfully!");
					return;
				}
				catch (MySqlException ex) when (attempt < maxRetries)
				{
					Log.Warning(ex, $"Database connection failed. Retrying in {delayMs}ms... (Attempt {attempt}/{maxRetries})");
					System.Threading.Thread.Sleep(delayMs);
					delayMs = Math.Min(delayMs * 2, 10000);
				}
				catch (Exception ex)
				{
					Log.Error(ex, "Non-recoverable error during migrations. Aborting.");
					throw;
				}
			}

			throw new Exception($"Failed to execute migrations after {maxRetries} attempts.");
		}
	}
}