using MimeKit;

namespace Helper.Mailing
{
    public interface IEmail
    {
        string Subject { get; set; }
        string To { get; set; }
        TextPart Body { get; set; }
    }
}
