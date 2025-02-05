using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;
using Tutor.Models;

namespace Tutor.Email
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailSettings _emailSettings;

        public EmailSender(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

		public async Task SendEmailAsync(string email, string subject, string message)
		{
			using var client = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.SmtpPort)
			{
				UseDefaultCredentials = false,
				Credentials = new NetworkCredential(_emailSettings.SmtpUser, _emailSettings.SmtpPass),
				EnableSsl = true,
				DeliveryMethod = SmtpDeliveryMethod.Network
			};

			var mailMessage = new MailMessage
			{
				From = new MailAddress(_emailSettings.FromEmail, _emailSettings.FromName),
				Subject = subject,
				Body = message,
				IsBodyHtml = true
			};

			try
			{
				if (string.IsNullOrWhiteSpace(email))
				{
					throw new ArgumentException("Email получателя не может быть пустым.", nameof(email));
				}

				mailMessage.To.Add(email);
				await client.SendMailAsync(mailMessage);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Ошибка при отправке письма: {ex.Message}");
				throw;
			}
		}

	}
}
