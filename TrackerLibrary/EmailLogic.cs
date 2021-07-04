using System.Collections.Generic;
using System.Net.Mail;

namespace TrackerLibrary
{
    public static class EmailLogic
    {
        public static void SendEmail(string to, string subject, string body)
        {
           SendEmail(new List<string>(){to}, new List<string>(), subject, body);
        }
        
        public static void SendEmail(List<string> to, List<string> bcc, string subject, string body)
        {
            MailAddress fromMailAddress = new MailAddress(GlobalConfig.AppKeyLookup("senderEmail"), 
                GlobalConfig.AppKeyLookup("senderDisplayName"));
            MailMessage mailMessage = new MailMessage();
            foreach (string mailid in to)
            {
                mailMessage.To.Add(mailid);
            }
            foreach (string mailid in bcc)
            {
                mailMessage.Bcc.Add(mailid);
            }
            
            mailMessage.From = fromMailAddress;
            mailMessage.Subject = subject;
            mailMessage.Body = body;
            mailMessage.IsBodyHtml = true;

            SmtpClient client = new SmtpClient();
            client.Send(mailMessage);
        }
    }
}