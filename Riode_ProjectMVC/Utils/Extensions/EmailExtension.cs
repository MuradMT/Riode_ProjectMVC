﻿using System.Net.Mail;
using System.Net;

namespace Riode_ProjectMVC.Utils.Extensions
{
    public class EmailExtension
    {
        public bool SendEmail(string userMail, string emailcontent, string emailSubject)
        {


            string myMail = "mammadzade03@gmail.com";
            string pass = "wdsjqvtxvedctffp";

            MailMessage mailMessage = new();
            mailMessage.From = new MailAddress(myMail, "Riode");
            mailMessage.To.Add(new MailAddress(userMail));
            mailMessage.Subject = emailSubject;
            mailMessage.Body = emailcontent;
            mailMessage.IsBodyHtml = true;

            SmtpClient client = new()
            {
                Credentials = new NetworkCredential(myMail, pass),
                EnableSsl = true,
                Host = "smtp.gmail.com",
                Port = 587,
            };
            client.Send(mailMessage);

            return true;
        }
    }
}
