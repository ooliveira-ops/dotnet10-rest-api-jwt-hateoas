
using Microsoft.EntityFrameworkCore;
using RestWithASPNET10Erudio.Model.Context;

namespace RestWithASPNET10Erudio.Configurations
{
	public static class DatabaseConfiguration
	{
		public static IServiceCollection AddDatabaseConfiguration(
			this IServiceCollection services, IConfiguration configuration)												//recebeu 2 parametros(configuration e services)
		{
			var connectionString = configuration.GetConnectionString("MySQLServerSQLConnectionStrings");
			if (string.IsNullOrEmpty(connectionString))
			{
				throw new ArgumentNullException("Connection string 'MSSQLServerSQLConnectionStrings");
			}


			services.AddDbContext<MSSQLContext>(options =>
				options.UseSqlServer(connectionString));
			return services;
		}
	}
}
