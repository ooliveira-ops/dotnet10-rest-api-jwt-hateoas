using Org.BouncyCastle.Security;
using RestWithASPNET10Erudio.DATA.DTO.V1;
using RestWithASPNET10Erudio.Mail;

namespace RestWithASPNET10Erudio.Services.Impl
{
	public class EmailServiceImpl : IEmailService
	{
		private readonly EmailSender _emailSender;			//enviar email
		private readonly ILogger<EmailServiceImpl> _logger;

		// ADICIONE ESSE CONSTRUTOR:
		public EmailServiceImpl(EmailSender emailSender, ILogger<EmailServiceImpl> logger)
		{
			_emailSender = emailSender;
			_logger = logger;
		}

		public void SendSimpleEmail(EmailRequestDTO emailRequest)
		{
			_emailSender
				.To(emailRequest.To)
				.WithSubject(emailRequest.Subject)
				.WithMessage(emailRequest.Body)
				.Send();
		}

		public async Task SendEmailWithAttachment(EmailRequestDTO emailRequest, IFormFile attachment)		//enviar arquivo
		{
			if(attachment == null || attachment.Length == 0)
			{
				_logger.LogWarning("Attachment is null or empty");
				throw new ArgumentException("Attachment is null or empty");
			}

			string tempFilePath = Path.Combine(Path.GetTempPath(),
				attachment.FileName);

			try
			{
				await using (var stream = new FileStream(tempFilePath, FileMode.Create))
				{
					await attachment.CopyToAsync(stream);
				}
				_emailSender
				.To(emailRequest.To)
				.WithSubject(emailRequest.Subject)
				.WithMessage(emailRequest.Body)
				.Attach(tempFilePath)
				.Send();
			}
			catch (Exception ex) {
				_logger.LogError(ex,"Error sending email with attachment");
				throw;
			}
			finally
			{
				if (File.Exists(tempFilePath))
				{
					File.Delete(tempFilePath);
				}
			}
		}
	}
}