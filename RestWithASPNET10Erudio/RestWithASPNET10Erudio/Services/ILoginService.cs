using RestWithASPNET10Erudio.DATA.DTO.V1;

namespace RestWithASPNET10Erudio.Services
{
	public interface ILoginService
	{
		TokenDTO ValidateCredentials(UserDTO user);
		TokenDTO ValidateCredentials(TokenDTO token);
		bool RevokeToken(string token);
		AccountCredentialsDTO Create(AccountCredentialsDTO user); 
	}
}
