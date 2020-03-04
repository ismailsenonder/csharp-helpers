using System;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace HelperMethods
{
    public class EmailHelpers
    {
        #region SendMail
        public Tuple<bool, string> SendMail(string smtpServer, string to, string from, string fromName, int portNumber,
            string subject, string body, string userName, string password, bool enableSsl, bool isBodyHtml)
        {
            try
            {
                SmtpClient server = new SmtpClient(smtpServer); //"mail.example.com"
                server.EnableSsl = enableSsl;
                server.Port = portNumber;
                server.Credentials = new System.Net.NetworkCredential(userName, password);
                MailMessage email = new MailMessage();
                email.IsBodyHtml = true;
                email.From = new MailAddress(from);
                email.Subject = subject;
                email.Body = body;
                email.IsBodyHtml = isBodyHtml;
                //adress: person1@example.com;person2@example.com;person3@example.com
                foreach (var address in to.Split(';'))
                {
                    email.To.Add(new MailAddress(address.Trim(), ""));
                }
                server.Send(email);

                return Tuple.Create(true, "Success");

            }
            catch (Exception ex)
            {
                return Tuple.Create(false, "Fail: " + ex.ToString());
            }
        }
        #endregion

        #region SendMail2

        public Tuple<bool, string> SendMail2(string smtpServer, string to, string toName, string from, string fromName, int portNumber,
            string mailsubject, string mailbody, string password, bool enableSsl, bool isBodyHtml)
        {
            try
            {
                var fromAddress = new MailAddress(from, fromName);
                var toAddress = new MailAddress(to, toName);
                string fromPassword = password;
                string subject = mailsubject;
                string body = mailbody;

                var smtp = new SmtpClient
                {
                    Host = smtpServer,
                    Port = portNumber,
                    EnableSsl = enableSsl,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                };
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = isBodyHtml,
                })
                {
                    smtp.Send(message);
                }

                return Tuple.Create(true, "Success");

            }
            catch (Exception ex)
            {
                return Tuple.Create(false, "Fail: " + ex.ToString());
            }
        }

        #endregion

        #region SendMail3
        public Tuple<bool, string> SendMail3(int port, string host, string fromEmail, string toEmail, string password,
            string subject, string body, bool ssl = true, int timeout = 10000)
        {
            try
            {
                // Command line argument must the the SMTP host.
                SmtpClient client = new SmtpClient();
                client.Port = port; //587
                client.Host = host; //"smtp.gmail.com"
                client.EnableSsl = ssl; //default true
                client.Timeout = timeout; //default 10000
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential(fromEmail, password);

                MailMessage mm = new MailMessage(fromEmail, toEmail, subject, body);
                mm.BodyEncoding = UTF8Encoding.UTF8;
                mm.IsBodyHtml = true;
                mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

                client.Send(mm);
                return Tuple.Create(true, "Success");
            }
            catch (Exception ex)
            {
                return Tuple.Create(false, "Fail: " + ex.ToString());
            }
        }
        #endregion

    }
}
