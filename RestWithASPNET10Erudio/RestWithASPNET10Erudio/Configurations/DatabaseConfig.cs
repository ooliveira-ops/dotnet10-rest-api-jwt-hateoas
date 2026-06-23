using Microsoft.EntityFrameworkCore;
using RestWithASPNET10Erudio.Model.Context;

namespace RestWithASPNET10Erudio.Configurations
{
	public static class DatabaseConfiguration
	{
		public static IServiceCollection AddDatabaseConfiguration(
			this IServiceCollection services, IConfiguration configuration)
		{
			var connectionString = configuration.GetConnectionString("MySQLServerSqlConnectionStrings");
			if (string.IsNullOrEmpty(connectionString))
			{
				throw new ArgumentNullException("Connection string 'MySQLServerSqlConnectionStrings' not found");
			}

			services.AddDbContext<MSSQLContext>(options =>
				options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 0))));
			return services;
		}
	}
}