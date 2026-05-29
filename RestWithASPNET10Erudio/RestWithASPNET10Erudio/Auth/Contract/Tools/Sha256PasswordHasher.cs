using System.Security.Cryptography;
using System.Text;

namespace RestWithASPNET10Erudio.Auth.Contract.Tools
{
	public class Sha256PasswordHasher : IPasswordHasher						//essa classe é sobre encriptação de senha
	{
		public string Hash(string password)									//Hash a senha fica encriptada
		{
			var inputBytes = Encoding.UTF8.GetBytes(password);				//Converte a senha em bytes
			var hashBytes = SHA256.HashData(inputBytes);					//Faz o hash da senha

			var builder = new StringBuilder();								//Cria um StringBuilder

			foreach (var b in hashBytes)								//Percorre o hash da senha
			{
				builder.Append(b.ToString("X2"));							//Converte o hash da senha em hexadecimal e adiciona no StringBuilder
			}
			return builder.ToString();										//Retorna o hash da senha
		}

		public bool Verify(string password, string hashedPassword)          //Verify a senha também; 
		{
			return string.Equals(                                           //Se o Hash da senha for igual ao Hash da senha encriptada, então ela é válida
			Hash(password),
			hashedPassword,
			StringComparison.OrdinalIgnoreCase
		);
	}
	}
}
