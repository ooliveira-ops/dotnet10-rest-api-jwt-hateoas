using RestWithASPNET10Erudio.Files.Importers.Impl;
using RestWithASPNET10Erudio.FIles.Importers.Contract;

namespace RestWithASPNET10Erudio.FIles.Importers.Factory
{
	public class FileImporterFactory(
		IServiceProvider serviceProvider, ILogger<FileImporterFactory> logger)
	{
		private readonly IServiceProvider _serviceProvider = serviceProvider;
		private readonly ILogger<FileImporterFactory> _logger = logger;

		public IFileImporter GetImporter(string fileName)
		{
			if (fileName.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
			{
				_logger.LogInformation("Selected CSV file importer for file: {FileName}", fileName);
				return _serviceProvider.GetRequiredService<CsvImporter>();
			}
			else if (fileName.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase))
			{
				_logger.LogInformation("Selected Excel file importer for file: {FileName}", fileName);
				return _serviceProvider.GetRequiredService<XlsxImporter>();
			}
			else
			{
				_logger.LogError("Unsupported file format: {FileName}", fileName);
				throw new NotSupportedException($"The file format '{fileName}' is not supported.");
			}
		}
	}
}
