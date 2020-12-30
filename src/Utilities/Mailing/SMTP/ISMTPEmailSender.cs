using System.Threading.Tasks;

namespace Utilities.Mailing.SMTP
{
    public interface ISMTPEmailSender
    {
        Task SendAsync(SMTPEmail email, string password, string smtpServer, int port);
    }
}
