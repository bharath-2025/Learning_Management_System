using System.Net;
using System.Net.Mail;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;

namespace LearnApp.Models;

public class EmailSender : IEmailSender
{
    public void SendEmailAsync(string receiverEmail, string subject, string message)
    {
        string senderEmail = "bharathmokara2025@gmail.com";
        string password = "qmaxkhprooduzxhq";

        MailMessage mailMessage = new MailMessage(senderEmail,receiverEmail);
        mailMessage.Subject = subject;
        mailMessage.Body = message;
        mailMessage.IsBodyHtml = false;
        SmtpClient smtp = new SmtpClient("smtp.gmail.com",587);
        smtp.EnableSsl = true;
        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
        NetworkCredential networkCredential = new NetworkCredential(senderEmail,password);
        smtp.UseDefaultCredentials = false;
        smtp.Credentials = networkCredential;

        smtp.Send(mailMessage);
    }
}