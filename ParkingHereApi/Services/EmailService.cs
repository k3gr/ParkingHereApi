using Microsoft.Extensions.Options;
using ParkingHereApi.Common.Models;
using static ParkingHereApi.Services.EmailService;
using MimeKit;
using MailKit.Net.Smtp;

namespace ParkingHereApi.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;
        public EmailService(IOptions<EmailSettings> mailSettingsOptions)
        {
            _emailSettings = mailSettingsOptions.Value;
        }

        public bool SendMail(CreateEmail createEmail)
        {
            try
            {
                using (MimeMessage emailMessage = new MimeMessage())
                {
                    MailboxAddress emailFrom = new MailboxAddress(_emailSettings.SenderName, _emailSettings.SenderEmail);
                    emailMessage.From.Add(emailFrom);
                    MailboxAddress emailTo = new MailboxAddress(createEmail.EmailToName, createEmail.EmailTo);
                    emailMessage.To.Add(emailTo);

                    emailMessage.Subject = createEmail.EmailSubject;

                    BodyBuilder emailBodyBuilder = new BodyBuilder();
                    emailBodyBuilder.TextBody = createEmail.EmailBody;

                    emailMessage.Body = emailBodyBuilder.ToMessageBody();
                    using (SmtpClient mailClient = new SmtpClient())
                    {
                        mailClient.Connect(_emailSettings.Server, _emailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
                        mailClient.Authenticate(_emailSettings.UserName, _emailSettings.Password);
                        mailClient.Send(emailMessage);
                        mailClient.Disconnect(true);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}