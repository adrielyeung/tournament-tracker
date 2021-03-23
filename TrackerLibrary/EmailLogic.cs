using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Mail;

namespace TrackerLibrary
{
    /// <summary>
    /// Utility class for sending out different kinds of notifications to users.
    /// </summary>
    // Static class since not storing data
    public static class EmailLogic
    {
        public static void SendEmail(List<string> to, List<string> bcc, string subject, string body)
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

            SmtpClient client = new SmtpClient()
            {
                Host = GlobalConfig.AppKeyLookup("host"),
                Port = Convert.ToInt32(GlobalConfig.AppKeyLookup("port")),
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(GlobalConfig.AppKeyLookup("userName"), GlobalConfig.AppKeyLookup("password"))
            };
            client.Send(mail);
        }

        public static void SendEmail(List<string> to, string subject, string body)
        {
            SendEmail(to, new List<string>(), subject, body);
        }
    }
}
