using RestWithASPNET10Erudio.DATA.DTO.V1;

namespace RestWithASPNET10Erudio.Services
{
	public interface IEmailService
	{
		void SendSimpleEmail(EmailRequestDTO emailRequest);
		Task SendEmailWithAttachment(EmailRequestDTO emailRequest,					//método responsavel por enviar o email com anexos
			IFormFile attachment);
	}
}
