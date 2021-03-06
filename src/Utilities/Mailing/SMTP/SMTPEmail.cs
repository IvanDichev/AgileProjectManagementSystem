﻿using MimeKit;
using System.Collections.Generic;
using System.Linq;

namespace Utilities.Mailing.SMTP
{
    public class SMTPEmail : ISMTPEmail
    {
        private readonly BodyBuilder bodyBuilder;
        public MimeMessage Message { get; set; }

        public SMTPEmail(string from, string to, string message, string subject)
        {
            this.bodyBuilder = new BodyBuilder();
            this.Message = new MimeMessage();
            this.Message.To.Add(MailboxAddress.Parse(to));
            this.Message.From.Add(MailboxAddress.Parse(from));
            this.Message.Subject = subject;

            this.bodyBuilder.HtmlBody = message;

            Message.Body = bodyBuilder.ToMessageBody();
        }

        public SMTPEmail(string from, string to, string message, string subject, List<string> Cc)
            : this(from, to, message, subject)
        {
            this.Message.Cc.AddRange(Cc.Select(x => MailboxAddress.Parse(x)));
        }

        public SMTPEmail(string from, string to, string message, string subject, Dictionary<string, byte[]> attachments)
            : this(from, to, message, subject)
        {
            foreach (var attachment in attachments)
            {
                this.bodyBuilder.Attachments.Add(attachment.Key, attachment.Value);
            }
        }

        public SMTPEmail(string from, string to, string message, string subject,
            List<string> Cc, Dictionary<string, byte[]> attachments)
            : this(from, to, message, subject, Cc)
        {
            foreach (var attachment in attachments)
            {
                this.bodyBuilder.Attachments.Add(attachment.Key, attachment.Value);
            }
        }

    }
}
