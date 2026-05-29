using RestWithASPNET10Erudio.Hypermedia.Enricher;
using RestWithASPNET10Erudio.Hypermedia.Filters;

namespace RestWithASPNET10Erudio.Configurations
{
	public static class HATEOASConfig
	{
		public static IServiceCollection AddHATEOASConfiguration(
			this IServiceCollection services)
		{
			var filteroptions = new HypermediaFilterOptions();
			filteroptions.ContentResponseEnricherList.Add(
				new BookEnricher());
			filteroptions.ContentResponseEnricherList.Add(
				new PersonEnricher());
			services.AddSingleton(filteroptions);

			services.AddScoped<HypermediaFilter>();
			return services;
		}

		public static void UseHATEOASRoutes(
			this IEndpointRouteBuilder app)
		{
			app.MapControllerRoute("Default", "{controller=values}/v1/{id?}");
		}
	}
}
