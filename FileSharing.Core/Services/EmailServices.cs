using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace FileSharing.Core.Services
{
    public class EmailServices
    {
        private void SendEmail(string subject, string body, string toEmail)
        {
            var fromAddress = new MailAddress("mmihail.damian@gmail.com");
            var toAddress = new MailAddress(toEmail);
            const string fromPassword = "Mf23dg5s?";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword),
                Timeout = 20000
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml=true
            })
            {
                smtp.Send(message);
            }
        }

        public void SendLink(string email, string url)
        {
            SendEmail("File sharing", $"Test text. <a href='{ url}'> Click here for download your file! </a> ", email);
        }
    }
}
