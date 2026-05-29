using RestWithASPNET10Erudio.FIles.Exporters.Contract;
using RestWithASPNET10Erudio.FIles.Exporters.Impl;
using RestWithASPNET10Erudio.FIles.Importers.Factory;
using Serilog.Core;

namespace RestWithASPNET10Erudio.FIles.Exporters.Factory
{
	public class FileExporterFactory(
		IServiceProvider serviceProvider, ILogger<FileExporterFactory> logger)
	{
		private readonly IServiceProvider _serviceProvider = serviceProvider;
	private readonly ILogger<FileExporterFactory> _logger = logger;


		public IFileExporter GetExporter(string accepctHeader)
		{
			if (string.Equals(accepctHeader, MediaTypes.ApplicationXlsx, StringComparison.OrdinalIgnoreCase))
			{
				_logger.LogInformation("Selected Excel file exporter for media type: {AcceptHeader}", accepctHeader);
				return _serviceProvider.GetService<XlsxExporter>();
			}
			else if (string.Equals(accepctHeader, MediaTypes.ApplicationCsv, StringComparison.OrdinalIgnoreCase))
			{
				_logger.LogInformation("Selected CSV file exporter for media type: {AcceptHeader}", accepctHeader);
				return _serviceProvider.GetService<CsvExporter>();
			}
			else
			{
				_logger.LogError("Unsupported media type: {AcceptHeader}", accepctHeader);
				throw new NotSupportedException($"The media type of '{accepctHeader}' is not supported.");
			}
		}
	}
}

