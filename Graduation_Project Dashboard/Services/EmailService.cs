using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net.Mail;
using System.Net;

namespace Graduation_Project_Dashboard.Services
{
    public class EmailService
    {
        private readonly IConfiguration Configuration;

        public EmailService(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        public void SendEmail(string email, string subject, string htmlMessage)
        {
            try
            {
                string fromMail = Configuration["Smtp:From"];
                string fromPassword = Configuration["Smtp:Password"];

                MailMessage message = new MailMessage();
                message.From = new MailAddress(fromMail);
                message.Subject = subject;
                message.To.Add(new MailAddress(email));
                message.Body = htmlMessage; 
                message.IsBodyHtml = true;   

                var smtpClient = new SmtpClient(Configuration["Smtp:Host"])
                {
                    Port = Convert.ToInt32(Configuration["Smtp:Port"]),
                    Credentials = new NetworkCredential(fromMail, fromPassword),
                    EnableSsl = Convert.ToBoolean(Configuration["Smtp:EnableSsl"]),
                };
                smtpClient.Send(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
                throw;
            }
        }


    }
}
