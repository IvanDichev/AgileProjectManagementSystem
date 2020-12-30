using MimeKit;

namespace Utilities.Mailing.SMTP
{
    public interface ISMTPEmail
    {
        public MimeMessage Message { get; set; }
    }
}
