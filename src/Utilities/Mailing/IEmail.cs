using MimeKit;

namespace Utilities.Mailing
{
    public interface IEmail
    {
        public MimeMessage Message { get; set; }
    }
}
