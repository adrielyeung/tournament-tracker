using System;
using System.Collections.Generic;

namespace TrackerLibrary
{
    /// <summary>
    /// Utility class for sending out different kinds of notifications to users.
    /// </summary>
    // Static class since not storing data
    // Use an interface so that it can be mocked for testing
    public static class EmailLogic
    {
        public static IEmailSender emailSender = new EmailSender();
        public static void SendEmail(List<string> to, List<string> bcc, string subject, string body)
        {
            emailSender.SendEmail(to, bcc, subject, body, null);
        }

        public static void SendEmail(string to, string subject, string body)
        {
            emailSender.SendEmail(to, subject, body, null);
        }
    }
}
