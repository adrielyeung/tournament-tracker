using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace TrackerLibrary
{
    public class EmailSender : IEmailSender
    {
        public void SendEmail(List<string> to, List<string> bcc, string subject, string body, SmtpClient smtpClient = null)
        {
            MailAddress fromMailAddress = new MailAddress(GlobalConfig.AppKeyLookup("senderEmail"), GlobalConfig.AppKeyLookup("senderDisplayName"));
            
            // Set up mail message
            MailMessage mail = new MailMessage();

            foreach (string toAddress in to)
            {
                mail.To.Add(toAddress);
            }
            foreach (string bccAddress in bcc)
            {
                mail.Bcc.Add(bccAddress);
            }
            mail.From = fromMailAddress;
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;

            if (smtpClient == null)
            {
                smtpClient = new SmtpClient()
                {
                    Host = GlobalConfig.AppKeyLookup("host"),
                    Port = Convert.ToInt32(GlobalConfig.AppKeyLookup("port")),
                    EnableSsl = true,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(GlobalConfig.AppKeyLookup("userName"), GlobalConfig.AppKeyLookup("password"))
                };
            }
            
            smtpClient.Send(mail);
        }

        public void SendEmail(string to, string subject, string body, SmtpClient smtpClient = null)
        {
            SendEmail(new List<string> { to }, new List<string>(), subject, body, smtpClient);
        }
    }
}
