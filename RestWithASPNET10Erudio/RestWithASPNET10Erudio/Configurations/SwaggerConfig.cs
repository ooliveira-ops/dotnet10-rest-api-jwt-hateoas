using Microsoft.OpenApi;

namespace RestWithASPNET10Erudio.Configurations
{
	public static class SwaggerConfig
	{
		private static readonly string AppName = "Curso UDEMY: ASP.NET 2026 REST API's from 0 to Azure and GCP with .NET 10, Docker e Kubernetes";
		private static readonly string AppDescription = $"REST API RESTful developed in course {AppName}";


		public static IServiceCollection AddSwaggerConfig(
			this IServiceCollection services)
		{
			services.AddSwaggerGen(options =>
			{
				options.SwaggerDoc("v1", new OpenApiInfo
				{
					Title = AppName,
					Version = "v1",
					Description = AppDescription,
					Contact = new OpenApiContact
					{
						Name = "Meu GH",
						Url = new Uri("https://github.com/ooliveira-ops")
					},
					License = new OpenApiLicense
					{
						Name = "Erudio GH",
						Url = new Uri("https://github.com/leandrocgsi")
					}
				});
				options.CustomSchemaIds(type => type.FullName);
			});
			return services;
		}

	}
}
