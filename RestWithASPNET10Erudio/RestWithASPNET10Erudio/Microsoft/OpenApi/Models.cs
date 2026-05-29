
namespace Microsoft.OpenApi
{
	internal class Models
	{
		internal class OpenApiInfo : OpenApi.OpenApiInfo
		{
			public new string Title { get; set; }
			public new string Version { get; set; }
			public new string Description { get; set; }
			public new object Contact { get; set; }
		}

		internal class OpenApiContact
		{
			public string Name { get; set; }
			public Uri Url { get; set; }
		}
	}
}