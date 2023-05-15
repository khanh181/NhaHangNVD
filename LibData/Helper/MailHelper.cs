using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LibData.Helper
{
    public class MailHelper
    {
        
        private MailAddress sender;
        private string senderPassword = "";
        public MailHelper(string mail, string name, string pass)
        {
            //GlobalConfig.LoadConfig();
            sender = new MailAddress(mail, name);
            senderPassword = pass;
        }
        public bool SendMail(string MailTo, string Subject, string Body)
        {
            try
            {
                string[] emails = MailTo.Split(new char[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries);
                //string[] emailsCC = MailCC.Split(new char[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries);
                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(sender.Address, senderPassword),
                    //Timeout = 20000
                };
                var send_mail = new MailMessage();
                send_mail.IsBodyHtml = true;
                send_mail.From = sender;
                foreach (string email in emails)
                {
                    send_mail.To.Add(email);
                }

                //foreach (string emailCC in emailsCC)
                //{
                //    send_mail.CC.Add(emailCC);
                //}
                //subject of the mail.
                send_mail.Subject = Subject;

                send_mail.Body = Body;
                smtp.Send(send_mail);

                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public bool SendMail(string Subject, string Body, string attachmentFileName, MailAddress toAddress)
        {
            try
            {

                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(sender.Address, senderPassword)
                };
                using (var message = new MailMessage(sender, toAddress)
                {
                    Subject = Subject,
                    Body = Body,
                })
                {
                    message.Attachments.Add(new Attachment(attachmentFileName));
                    smtp.Send(message);
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
