using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net.Mail;
using System.Net;

namespace Graduation_Project_Dashboard.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration Configuration;

        public EmailSender(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            string fromMail = Configuration["Smtp:From"];
            string fromPassword = Configuration["Smtp:password"];

            MailMessage message = new MailMessage();
            message.From = new MailAddress(fromMail);
            message.Subject = subject;
            message.To.Add(new MailAddress(email));
            message.Body = "<html><body> " + htmlMessage + " </body></html>";
            message.IsBodyHtml = true;

            var smtpClient = new SmtpClient(Configuration["Smtp:Host"])
            {
                Port = Convert.ToInt32(Configuration["Smtp:Port"]),
                Credentials = new NetworkCredential(fromMail, fromPassword),
                EnableSsl = Convert.ToBoolean(Configuration["Smtp:EnableSsl"]),
            };
            smtpClient.Send(message);
        }


    }
}
