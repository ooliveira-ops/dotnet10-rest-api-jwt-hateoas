using RestWithASPNET10Erudio.Auth.Configuration;

namespace RestWithASPNET10Erudio.Auth.Configuration
{
	public class TokenConfiguration             //setou as config do appsettingssettings.json para o token
	{
		public string Audience { get; set; }
		public string Issuer { get; set; }
		public string Secret { get; set; }
		public int Minutes { get; set; }
		public int DaysToExpiry { get; set; }

	}
}
