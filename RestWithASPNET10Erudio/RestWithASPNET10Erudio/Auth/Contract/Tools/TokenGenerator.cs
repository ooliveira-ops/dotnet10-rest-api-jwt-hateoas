using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Pqc.Crypto.Crystals.Dilithium;
using RestWithASPNET10Erudio.Auth.Configuration;

namespace RestWithASPNET10Erudio.Auth.Contract.Tools
{
	public class TokenGenerator(TokenConfiguration configurations) : ITokenGenerator
	{
		private readonly TokenConfiguration _configurations = configurations;
		public string GenerateAccessToken(IEnumerable<Claim> claims)
		{
			var secretKey = new SymmetricSecurityKey(Encoding.UTF8
				.GetBytes(_configurations.Secret));

			var sigingCredentials = new 
				SigningCredentials(secretKey, 
				SecurityAlgorithms.HmacSha256);

			var tokenOptions = new JwtSecurityToken(
				issuer: _configurations.Issuer,
				audience: _configurations.Audience,
				claims: claims,
				expires: DateTime.Now.AddMinutes(_configurations.Minutes), 
				signingCredentials: sigingCredentials);

			return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
		}

		public string GenerateRefreshToken()
		{
			var randonNumber = new byte[32];
			using (var rng = RandomNumberGenerator.Create())
			{
				rng.GetBytes(randonNumber);
				return Convert.ToBase64String(randonNumber);
			}
		}

		public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
		{
			var tokenValidationParameters = new TokenValidationParameters
			{
				ValidateAudience = false,
				ValidateIssuer = false,
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = new SymmetricSecurityKey(
					Encoding.UTF8.GetBytes(_configurations.Secret)),
				ValidateLifetime = false									//ignorar a expiração do token
			};

			var tokenHandler = new JwtSecurityTokenHandler();
			var principal = tokenHandler.ValidateToken(token,				//valida o token
				tokenValidationParameters, out var securityToken);

			if (securityToken is not JwtSecurityToken jwtSecurityToken
				|| !jwtSecurityToken.Header.Alg
				.Equals(SecurityAlgorithms.HmacSha256, StringComparison
				.InvariantCultureIgnoreCase))

				throw new SecurityTokenException("Invalid token");
			return principal;
			
		}
	}
}
