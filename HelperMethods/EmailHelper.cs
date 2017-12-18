using System;
using System.Collections.Generic;
using System.Linq;
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
            string subject, string body, string userName, string password, bool enableSsl)
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
    }
}
