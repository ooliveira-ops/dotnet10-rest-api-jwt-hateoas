using Microsoft.Net.Http.Headers;

namespace RestWithASPNET10Erudio.Configurations
{
	public static class ContentNegotiacionConfig
	{
		public static IMvcBuilder AddContentNegotiacion(
			this IMvcBuilder builder)
		{
			return builder.AddMvcOptions(options =>
			{
				options.RespectBrowserAcceptHeader = true;
				options.ReturnHttpNotAcceptable = true;

				options.FormatterMappings.SetMediaTypeMappingForFormat(
					"xml", MediaTypeHeaderValue.Parse("aplication/xml"));
				options.FormatterMappings.SetMediaTypeMappingForFormat(
					"json", MediaTypeHeaderValue.Parse("aplication/json"));
			})
				.AddXmlSerializerFormatters(); 
		}
	}
}
