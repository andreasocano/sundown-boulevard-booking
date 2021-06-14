using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SundownBoulevard.Booking.API.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace SundownBoulevard.Booking.API.Services
{
    public class SendEmailService
    {
        private readonly ILogger<SendEmailService> _logger;
        private readonly SMTPConfiguration _smtpConfiguration;
        private readonly string _senderDisplayName;

        public SendEmailService(ILogger<SendEmailService> logger, SMTPConfiguration smtpConfiguration, IConfiguration configuration)
        {
            _logger = logger;
            _smtpConfiguration = smtpConfiguration;
            _senderDisplayName = configuration["EmailSenderDisplayName"];
        }

        /// <summary>
        /// Sends an email
        /// </summary>
        /// <param name="recipients"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public async Task SendEmail(IEnumerable<string> recipients, string subject, string body)
        {
            try
            {
                byte[] bytes = Encoding.Default.GetBytes(body);
                body = Encoding.UTF8.GetString(bytes);

                var mail = new MailMessage
                {
                    From = new MailAddress(_smtpConfiguration.Sender, _senderDisplayName),
                    Subject = subject,
                    IsBodyHtml = true,
                    Body = body
                };

                foreach (var recipient in recipients)
                {
                    mail.To.Add(new MailAddress(recipient));
                }

                using var smtp = new SmtpClient
                {
                    Port = _smtpConfiguration.Port,
                    Host = _smtpConfiguration.Host,
                    EnableSsl = true,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(_smtpConfiguration.Username, _smtpConfiguration.Password),
                    DeliveryMethod = SmtpDeliveryMethod.Network
                };

                await smtp.SendMailAsync(mail);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to send e-mail");
            }
        }
    }
}
