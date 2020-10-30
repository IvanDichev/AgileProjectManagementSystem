using System.Threading.Tasks;

namespace Utilities.Mailing
{
    public interface IEmailSender
    {
        Task SendAsync(Email email, string password, string smtpServer, int port);
    }
}
