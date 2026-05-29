using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

namespace RestWithASPNET10Erudio.Configurations
{
	public static class CorsConfig
	{
		private static string[] GetAllowedOrigins(
			IConfiguration configuration)
		{
			return configuration.GetSection("Cors:Origins")
				.Get<string[]>() ?? Array.Empty<string>();
		}

		public static IServiceCollection AddCorsConfiguration(
			this IServiceCollection services,
			IConfiguration configuration)
		{
			services.AddCors(options =>
			{
				options.AddPolicy("LocalPolicy",
					policy => policy
						.WithOrigins("http://localhost:3000")
						.AllowAnyMethod()
						.AllowAnyHeader()
						.AllowCredentials());

				options.AddPolicy("MultipleOriginPolicy",
					policy => policy
						.WithOrigins(
							"http://localhost:3000",
							"http://localhost:8080",
							"https://erudio.com.br"
						)
						.AllowAnyMethod()
						.AllowAnyHeader()
						.AllowCredentials());

				options.AddPolicy("DefaultPolicy",
					policy => policy
						.AllowAnyOrigin()
						.AllowAnyMethod()
						.AllowAnyHeader());
			});
			return services;
		}

		public static IApplicationBuilder UseCorsConfiguration(
	this IApplicationBuilder app,
	IConfiguration configuration)
		{
			// ✅ Isso adiciona o header "Access-Control-Allow-Origin" na resposta
			app.UseCors("MultipleOriginPolicy");

			var origins = GetAllowedOrigins(configuration);
			app.Use(async (context, next) =>
			{
				var selfOrigin = $"{context.Request.Scheme}://{context.Request.Host}";
				var origin = context.Request.Headers["Origin"].ToString();
				if (!string.IsNullOrEmpty(origin) &&
					!origin.Equals(selfOrigin, StringComparison.OrdinalIgnoreCase) &&
					!origins.Contains(origin, StringComparer.OrdinalIgnoreCase))
				{
					context.Response.StatusCode = StatusCodes.Status403Forbidden;
					await context.Response.WriteAsync("CORS origin not allowed");
					return;
				}
				await next();
			});

			return app; 
		}
	}
}