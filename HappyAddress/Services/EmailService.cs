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
            string smtpHost = _configuration["EmailSettings:SmtpHost"];
            int smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"]);
            string smtpUser = _configuration["EmailSettings:SmtpUser"];
            string smtpPassword = _configuration["EmailSettings:SmtpPassword"];
            string fromEmail = _configuration["EmailSettings:FromEmail"];

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
                DeliveryMethod = SmtpDeliveryMethod.Network
            };

            client.Send(message);
        }
    }
}