using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using RestWithASPNET10Erudio.Auth.Configuration;
using RestWithASPNET10Erudio.Auth.Contract;
using RestWithASPNET10Erudio.DATA.DTO.V1;
using RestWithASPNET10Erudio.Model;

namespace RestWithASPNET10Erudio.Services.Impl
{
	public class LoginServiceImpl : ILoginService
	{
		private const string DATE_FORMAT = "yyyy-MM-dd HH:mm:ss";

		private readonly IUserAuthService _userAuthServices;
		private readonly IPasswordHasher _passwordHasher;
		private readonly ITokenGenerator _tokenService;
		private readonly TokenConfiguration _configurations;

		public LoginServiceImpl(
			IUserAuthService userAuthServices,
			IPasswordHasher passwordHasher, 
			ITokenGenerator tokenService, 
			TokenConfiguration configurations)
		{
			_userAuthServices = userAuthServices;
			_passwordHasher = passwordHasher;
			_tokenService = tokenService;
			_configurations = configurations;
		}

		public TokenDTO ValidateCredentials(UserDTO userDto)
		{
			var user = _userAuthServices.FindByUsername(userDto.Username);

			if (user == null) return null;

			if (!_passwordHasher.Verify(userDto.Password, 
				user.Password)) return null;

			return GenerateToken(user);
		}


		public TokenDTO ValidateCredentials(TokenDTO token)
		{
			var principal = _tokenService.GetPrincipalFromExpiredToken(token.AccessToken);
			var username = principal.Identity?.Name;

			var user = _userAuthServices.FindByUsername(username);
				if (user == null ||
				user.RefreshToken != token.RefreshToken ||
				user.RefreshTokenExpirationTime <= DateTime.Now) return null;
			return GenerateToken(user, principal.Claims);
		}
		public AccountCredentialsDTO Create(AccountCredentialsDTO dto)
		{
			var user = _userAuthServices.Create(dto);
			return new AccountCredentialsDTO
			{
				UserName = user.UserName,
				FullName = user.FullName,
				Password = "************"
			};
		}

		public bool RevokeToken(string username)
		{
			return _userAuthServices.RevokeToken(username);
		}

		private TokenDTO GenerateToken(User user, 
			 IEnumerable<Claim>? existingClaims = null)
		{
			var claims = existingClaims?.ToList() ?? 
				// new List<Claim>
				[ 
					new Claim(JwtRegisteredClaimNames.Jti,
					Guid.NewGuid().ToString("N")),
					new Claim(JwtRegisteredClaimNames.UniqueName,
						user.UserName),
			];

		var accessToken = _tokenService
			.GenerateAccessToken(claims);

			var refreshToken = _tokenService
				.GenerateRefreshToken();

			user.RefreshToken = refreshToken;
			user.RefreshTokenExpirationTime = DateTime.Now
				.AddDays(_configurations.DaysToExpiry);

			_userAuthServices.Update(user);

			var createdDate = DateTime.Now;
			var expirationDate = createdDate
				.AddMinutes(_configurations.Minutes);
				
			return new TokenDTO
			{
				Authenticated = true,
				Created = createdDate.ToString(DATE_FORMAT),
				Expiration = expirationDate
				.ToString(DATE_FORMAT),

				AccessToken = accessToken,
				RefreshToken = refreshToken
			};
		}

	}
}
