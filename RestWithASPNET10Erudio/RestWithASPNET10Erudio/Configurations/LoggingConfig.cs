using Serilog;

namespace RestWithASPNET10Erudio.Configurations
{
	public static class LoggingConfig
	{

		public static void AddSerilogLogging(this WebApplicationBuilder builder)
		{
			Log.Logger = new LoggerConfiguration()									// Inicia a configuração do motor de logs
				.ReadFrom.Configuration(builder.Configuration)						// Busca configurações (como níveis de log) do seu appsettings.json
				.Enrich.FromLogContext()											// Adiciona informações extras automaticamente (como ID da requisição ou usuário)
				.WriteTo.Console()													// Envia os logs para a janela do Console (terminal)
				.WriteTo.Debug()													// Envia os logs para a janela de 'Output' do Visual Studio enquanto você debuga
				.CreateLogger();                                                    // Finaliza a montagem e cria o logger pronto para uso
			builder.Host.UseSerilog();
		}
	}
}
