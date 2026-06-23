namespace RestWithASPNET10Erudio.DATA.DTO.V1
{
	public class UserDTO
	{
		public UserDTO() {}						// construtor vazio porque o serializador do json não aceita construtores com parâmetros
		public string Username { get; set; }
		public string Password { get; set; }
	}
}
