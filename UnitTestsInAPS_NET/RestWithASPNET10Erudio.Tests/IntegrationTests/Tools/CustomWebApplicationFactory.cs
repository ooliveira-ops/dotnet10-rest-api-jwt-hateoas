using System.Reflection;
using Docker.DotNet.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RestWithASPNET10Erudio.Model.Context;

namespace RestWithASPNET10Erudio.Tests.IntegrationTests.Tools
{
	public class CustomWebApplicationFactory<TProgram>
		: WebApplicationFactory<TProgram> where TProgram : class
	{
		private readonly string _connectionString;

		public CustomWebApplicationFactory(string connectionString)
		{
			_connectionString = connectionString;
		}

		protected override void ConfigureWebHost(IWebHostBuilder builder)
		{
			builder.ConfigureAppConfiguration((context, config) =>
			{
				var testConfigPath = Path.Combine(
					Path.GetDirectoryName(
						Assembly.GetExecutingAssembly().Location)!,
					"appsettings.Test.json");

				config.Sources.Clear();
				config.AddJsonFile(testConfigPath, optional: false, reloadOnChange: true);
			});

			builder.ConfigureServices(services =>
			{
				var descriptor = services.SingleOrDefault(
					d => d.ServiceType == typeof(DbContextOptions<MSSQLContext>));
				if (descriptor != null)
					services.Remove(descriptor);

				services.AddDbContext<MSSQLContext>(options =>
					options.UseSqlServer(_connectionString));

				// ✅ Redefine o CORS para o ambiente de teste
				services.AddCors(options =>
				{
					options.AddPolicy("MultipleOriginPolicy",
						policy => policy
							.WithOrigins(
								"http://localhost:3000",
								"http://localhost:8080",
								"https://erudio.com.br"  // ✅ origem usada no teste
							)
							.AllowAnyMethod()
							.AllowAnyHeader()
							.AllowCredentials());
				});
			});
		}
	}
}