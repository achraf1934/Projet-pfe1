using backen_dotnet.Data;
using backen_dotnet.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace backen_dotnet.Service
{
    public class MailService : IMailService
    {

        public MailService()
        {
        }

        public async Task<bool> SendMailAsync(MailData mailData)
        {

            var message = new MailMessage()
            {
                From = new MailAddress(mailData.FromEmail),
                Subject = mailData.Subject,
                IsBodyHtml = true,
                Body = mailData.Body
            };
            foreach(var toEmail in mailData.ToEmails.Split(";"))
            {
                message.To.Add(new MailAddress(toEmail));
            }
            var smtp = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(mailData.FromEmail, "ywvjnttknbuddlyb"),
                EnableSsl = true,

            };
            smtp.Send(message);
            return true;

        }
    }
}
