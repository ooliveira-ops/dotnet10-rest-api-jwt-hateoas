using Microsoft.Data.SqlClient;
using Serilog;

namespace RestWithASPNET10Erudio.Configurations
{
	public static class DatabaseInitializer
	{
		public static void EnsureDatabaseExists(string connectionString)
		{
			try
			{
				// Extrair server e database da connection string
				var builder = new SqlConnectionStringBuilder(connectionString);
				var databaseName = builder.InitialCatalog;

				// Remover database da connection string para conectar ao master
				builder.InitialCatalog = "master";
				var masterConnectionString = builder.ConnectionString;

				Log.Information($"Ensuring database '{databaseName}' exists...");

				// Tentar conectar e criar banco se não existir
				for (int i = 0; i < 10; i++)
				{
					try
					{
						using (var connection = new SqlConnection(masterConnectionString))
						{
							connection.Open();

							using (var command = connection.CreateCommand())
							{
								command.CommandText = $@"
                                    IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = '{databaseName}')
                                    BEGIN
                                        CREATE DATABASE [{databaseName}];
                                    END";

								command.ExecuteNonQuery();
								Log.Information($"Database '{databaseName}' is ready!");
								return;
							}
						}
					}
					catch (SqlException ex) when (i < 9)
					{
						Log.Warning($"Attempt {i + 1} to create database failed. Retrying... ({ex.Message})");
						System.Threading.Thread.Sleep(2000);
					}
				}

				throw new Exception($"Failed to create database after 10 attempts");
			}
			catch (Exception ex)
			{
				Log.Error(ex, "Error ensuring database exists");
				throw;
			}
		}
	}
}