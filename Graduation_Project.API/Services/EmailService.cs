using Graduation_Project.Shared.Models.Authentication;
using System.Net.Mail;
using System;
using System.Net;
using Microsoft.Extensions.Configuration;

namespace Graduation_Project.API.Services
{
    public class EmailService
    {
        private readonly IConfiguration Configuration;

        public EmailService(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void SendEmail(string email, string subject, string plainTextMessage)
        {
            try
            {
                string fromMail = Configuration["Smtp:From"];
                string fromPassword = Configuration["Smtp:Password"];

                MailMessage message = new MailMessage();
                message.From = new MailAddress(fromMail);
                message.Subject = subject;
                message.To.Add(new MailAddress(email));
                message.Body = plainTextMessage; 
                message.IsBodyHtml = false;

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
