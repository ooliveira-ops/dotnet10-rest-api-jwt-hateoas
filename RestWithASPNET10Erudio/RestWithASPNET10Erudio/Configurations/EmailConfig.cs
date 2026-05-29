using RestWithASPNET10Erudio.Mail.Settings;

namespace RestWithASPNET10Erudio.Configurations
{
	public static class EmailConfig				//aqui vai carregar as configurações de email, ele vai carregar o que está no 'appsettings.json' na prte de Email e o objeto(MailSettings)
	{
		public static IServiceCollection AddEmailConfiguration(
			this IServiceCollection services, 
			IConfiguration configuration)
		{ 
			var section = configuration.GetSection("Email");
			var configs = section.Get<EmailSettings>();

			if (configs == null)
				throw new ArgumentNullException(
					nameof(configs),
					"Email configuration section is missing or invalid.");

			configs.Username = Environment.GetEnvironmentVariable("EMAIL_USERNAME")
				?? configs.Username;

			configs.Password = Environment.GetEnvironmentVariable("EMAIL_PASSWORD")
				?? configs.Username; 

			services.AddSingleton(configs);

			return services;
		}
	}
}
