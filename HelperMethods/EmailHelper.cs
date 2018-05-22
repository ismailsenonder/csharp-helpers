using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace HelperMethods
{
    public class EmailHelper
    {
        #region SendMail
        /// <summary>
        /// Sends Email
        /// </summary>
        /// <param name="smtpServer">mail.example.com</param>
        /// <param name="to">person1@example.com;person2@example.com;person3@example.com</param>
        /// <param name="from">fromemail@example.com</param>
        /// <param name="fromName">Example Sender</param>
        /// <param name="portNumber">587</param>
        /// <param name="subject">Mail Subject</param>
        /// <param name="body">e-mail HTML body (or plain text)</param>
        /// <param name="userName">info@example.com</param>
        /// <param name="password">smtppassword</param>
        /// <returns>Tuple containing a bool and a string</returns>
        /// bool: true if success, false if fail
        /// string: "Success" if success, "Fail: " + exception message if fail
        public Tuple<bool, string> SendMail(string smtpServer, string to, string from, string fromName, int portNumber,
            string subject, string body, string userName, string password, bool enableSsl, bool isBodyHtml)
        {
            try
            {
                SmtpClient server = new SmtpClient(smtpServer);
                server.EnableSsl = enableSsl;
                server.Port = portNumber;
                server.Credentials = new System.Net.NetworkCredential(userName, password);
                MailMessage email = new MailMessage();
                email.IsBodyHtml = true;
                email.From = new MailAddress(from);
                email.Subject = subject;
                email.Body = body;
                email.IsBodyHtml = isBodyHtml;
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

        #region SendMailWithGmailAddress
        /// <summary>
        /// Sends Email - First method caused error when trying to send through Gmail. So this is the second one :)
        /// </summary>
        /// <param name="smtpServer">smtp server ip or address as string: "mail.senonder.com"</param>
        /// <param name="to">to e-mail address as string.</param>
        /// <param name="toName">to e-mail name as string.</param>
        /// <param name="from">from e-mail address as string (also counts as smtp user name)</param>
        /// <param name="fromName">e-mail from name as string</param>
        /// <param name="portNumber">port number as int. E.g. 25 or 587</param>
        /// <param name="mailsubject">e-mail subject as string</param>
        /// <param name="mailbody">e-mail HTML body (or plain text) as string</param>
        /// <param name="password">smtp password as string</param>
        /// <param name="enableSsl">bool</param>
        /// <returns>Tuple containing a bool and a string</returns>
        /// bool: true if success, false if fail
        /// string: "Success" if success, "Fail: " + exception message if fail
        public Tuple<bool, string> SendMailWithGmailAddress(string smtpServer, string to, string toName, string from, string fromName, int portNumber,
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


    }
}
