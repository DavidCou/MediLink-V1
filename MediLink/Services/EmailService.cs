using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


using MediLink.Models;
using MailKit.Security;
using MimeKit.Text;
using MimeKit;
using MailKit.Net.Smtp;

namespace MediLink.Services
{
    public static class EmailService
    {
        private static string _Host = "smtp.gmail.com";

        private static int _Port = 587;

        //pyvt hhuj eaci fhuq
        private static string _WhoSent = "MediLink";
        private static string _Email = "2jddynamos@gmail.com";
        private static string _Password = "xeranpqhcjgmbfou";

        public static bool SendEmail(Email oEmail)
        {
            try
            {
                var email = new MimeMessage();

                email.From.Add(new MailboxAddress(_WhoSent, _Email));
                email.To.Add(MailboxAddress.Parse(oEmail.recipient));
                email.Subject = oEmail.subject;
                email.Body = new TextPart(TextFormat.Html)
                {
                    Text = oEmail.body
                };

                var smtp = new SmtpClient();
                smtp.Connect(_Host, _Port, SecureSocketOptions.StartTls);

                smtp.Authenticate(_Email, _Password);
                smtp.Send(email);
                smtp.Disconnect(true);

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}

