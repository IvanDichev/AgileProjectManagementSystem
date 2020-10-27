using MimeKit;
using System;
using System.Collections.Generic;
using System.Text;

namespace Helper.Mailing
{
    class Email : IEmail
    {
        public string Subject { get; set; }
        public string To { get; set; }
        public TextPart Body { get; set; }
    }
}
