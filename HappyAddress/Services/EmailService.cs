using System.Net;
using System.Net.Mail;
using System.Text;

namespace HappyAddress.Services
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void SendEmailConfirmationCode(string toEmail, string code)
        {
            string smtpHost = GetRequiredSetting("EmailSettings:SmtpHost");
            int smtpPort = int.Parse(GetRequiredSetting("EmailSettings:SmtpPort"));
            string smtpUser = GetRequiredSetting("EmailSettings:SmtpUser");
            string smtpPassword = GetRequiredSetting("EmailSettings:SmtpPassword");
            string fromEmail = GetRequiredSetting("EmailSettings:FromEmail");

            using MailMessage message = new MailMessage();

            message.From = new MailAddress(fromEmail, "HappyAddress", Encoding.UTF8);
            message.To.Add(toEmail);
            message.Subject = "Код подтверждения email";
            message.SubjectEncoding = Encoding.UTF8;
            message.BodyEncoding = Encoding.UTF8;
            message.IsBodyHtml = false;

            message.Body = $@"Здравствуйте!

Вы начали регистрацию на сайте HappyAddress.

Ваш код подтверждения email: {code}

Код действует 15 минут. Никому не сообщайте этот код.

Если вы не регистрировались на HappyAddress, просто проигнорируйте это письмо.

С уважением,
команда HappyAddress";

            using SmtpClient client = new SmtpClient(smtpHost, smtpPort)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(smtpUser, smtpPassword),
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Timeout = 15000
            };

            client.Send(message);
        }

        private string GetRequiredSetting(string key)
        {
            string? value = _configuration[key];
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new InvalidOperationException($"Не задана настройка {key}.");
            }

            return value;
        }
    }
}
