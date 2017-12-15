using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace SharedCommunity.Helpers
{
    // This class is used by the application to send email for account confirmation and password reset.
    // For more details see https://go.microsoft.com/fwlink/?LinkID=532713
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            return Task.CompletedTask;
        }

        public Task SmtpEmailAsync(string subject, string message, string receiver)
        {
            SmtpClient client = new SmtpClient("smtp.office365.com") {
                Port = 587,
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("v-tazho@microsoft.com","Edward11520")                
            };
            var mail = new MailMessage ();
            mail.From = new MailAddress("v-tazho@microsoft.com");
            mail.To.Add(new MailAddress(receiver));
            mail.Subject = subject;
            mail.Body = message;
            client.Send(mail);
            return Task.FromResult(0);
        }
    }
}
