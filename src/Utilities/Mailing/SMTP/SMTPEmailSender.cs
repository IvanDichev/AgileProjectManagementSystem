using MailKit.Net.Smtp;
using System.Threading.Tasks;

namespace Utilities.Mailing.SMTP
{
    public class SMTPEmailSender : ISMTPEmailSender
    {
        public async Task SendAsync(SMTPEmail email, string password,
            string smtpServer = "smtp.abv.bg", int port = 465)
        {
            using (var client = new SmtpClient())
            {
                client.Connect(smtpServer, port, true);
                await client.AuthenticateAsync(email.Message.From.ToString(), password);

                await client.SendAsync(email.Message);

                client.Disconnect(true);
            }
        }
    }
}
