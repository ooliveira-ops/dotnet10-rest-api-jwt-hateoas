using System.ComponentModel.DataAnnotations.Schema;
using RestWithASPNET10Erudio.Model.Base;

namespace RestWithASPNET10Erudio.Model
{

	[Table("users")]
	public class User : BaseEntity
	{
		[Column("user_name")]
		public string UserName { get; set; }

		[Column("full_name")]
		public string FullName { get; set; } = string.Empty;

		[Column("password")]
		public string Password { get; set; } = string.Empty;

		[Column("refresh_token")]
		public string? RefreshToken { get; set; }


		[Column("refresh_token_expiration_time")]
		public DateTime? RefreshTokenExpirationTime { get; set; }

		
	}
}
