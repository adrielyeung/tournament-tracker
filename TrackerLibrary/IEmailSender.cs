using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace TrackerLibrary
{
    public interface IEmailSender
    {
        void SendEmail(List<string> to, List<string> bcc, string subject, string body, SmtpClient client);
        void SendEmail(string to, string subject, string body, SmtpClient client);
    }
}
