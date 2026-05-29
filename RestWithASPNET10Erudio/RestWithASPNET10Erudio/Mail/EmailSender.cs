
using CsvHelper;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using RestWithASPNET10Erudio.Mail.Settings;

namespace RestWithASPNET10Erudio.Mail
{
	public class EmailSender(EmailSettings settings,
		ILogger<EmailSender> logger)
	{
		private readonly EmailSettings _settings = settings;
		private readonly ILogger<EmailSender> _logger = logger;

		private string _to;
		private string _subject;
		private string _body;
		private readonly List<MailboxAddress> _recipients = new ();
		private string _attachment;

		public EmailSender To(string to)		//envia "PARA" (to)
		{
			_to = to;
			_recipients.Clear();
			_recipients.AddRange(ParseRecipients(to));
			return this;
		}

		public EmailSender WithSubject(string subject)		// com o Assunto
		{
			_subject = subject;
			return this;
		}

		public EmailSender WithMessage(string body)			//com a menssagem
		{
			_body = body;
			return this;
		}

		public EmailSender Attach(string filePath)			//pode anexar um arquivo
		{
			if (File.Exists(filePath))
			{
				_attachment = filePath;
			}
			else
			{
				_logger.LogWarning("Attachment file not found: {FilePath}", filePath);
			}

			return this;
		}

		public void Send()
		{
			var message = new MimeMessage();

			message.From.Add(new MailboxAddress(_settings.From, _settings.Username));

			message.To.AddRange(_recipients);

			message.Subject = _subject ?? _settings.Subject ?? "No subject";

			var builder = new BodyBuilder()
			{
				TextBody = _body ?? _settings.Message ?? ""
			};

			if (!string.IsNullOrEmpty(_attachment))
			{
				var filename = Path.GetFileName(_attachment);
				builder.Attachments.Add(filename, File.ReadAllBytes(_attachment));
			}
			message.Body = builder.ToMessageBody();

			try
			{
				using var client = new SmtpClient();

				client.Connect(
					_settings.Host,
					_settings.Port,
					_settings.Ssl ? SecureSocketOptions.StartTls : SecureSocketOptions.None);
					
				client.Authenticate(_settings.Username, _settings.Password); 
				client.Send(message);
				client.Disconnect(true);

				_logger.LogInformation("E-mail successfully sent to {Recipients}", string.Join (";", _recipients));
				// email1@gmail.com; email2@gmail.com; email3@gmail.com .....
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "E-mail not sent to {Recipients}", string.Join(";", _recipients));
				throw;
			}
			finally
			{
				Reset();
			}
		}


		private IEnumerable<MailboxAddress> ParseRecipients(string to)
		{
			// emial1@gmail.com; email2@gmail.com; email3@gmail.com

			var tosWithoutSpaces = to.Replace(" ", string.Empty);											//vai remover os espaços do email por uma string vazia
			var recipients = tosWithoutSpaces.Split(';', StringSplitOptions.RemoveEmptyEntries);				//vai separar os emails por ";" , vai criar um array com os emails, e se tiver ex. 2 ";", ele vai ignorar

			var list = new List<MailboxAddress>();															//convertsando o array em uma lista
			foreach (var address in recipients)
			{
				try
				{
					var mailbox = MailboxAddress.Parse(address);
					list.Add(mailbox);
				}
				catch (Exception ex)
				{
					_logger.LogWarning(ex, "Invalid email address: {Recipient}", address);
				}
			}
				return list;

			throw new NotImplementedException();
		}
		private void Reset()
		{
			_to = null;
			_subject = null;
			_body = null;
			_recipients.Clear();
			_attachment = null;
		}

		internal object To(object to)
		{
			throw new NotImplementedException();
		}
	}
}
