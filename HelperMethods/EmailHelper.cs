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
        /// <param name="smtpServer">smtp server ip or address as string: "mail.senonder.com"</param>
        /// <param name="to">to e-mail address as string</param>
        /// <param name="from">from e-mail address as string</param>
        /// <param name="fromName">e-mail from name as string</param>
        /// <param name="portNumber">port number as int. E.g. 25 or 587</param>
        /// <param name="subject">e-mail subject as string</param>
        /// <param name="body">e-mail HTML body (or plain text) as string</param>
        /// <param name="userName">smtp username as string: "info@senonder.com"</param>
        /// <param name="password">smtp password as string</param>
        /// <returns>Tuple containing a bool and a string</returns>
        /// bool: true if success, false if fail
        /// string: "Success" if success, "Fail: " + exception message if fail
        public Tuple<bool, string> SendMail(string smtpServer, string to, string from, string fromName, int portNumber,
            string subject, string body, string userName, string password)
        {
            try
            {
                SmtpClient server = new SmtpClient(smtpServer);
                server.EnableSsl = false;
                server.Port = portNumber;
                server.Credentials = new System.Net.NetworkCredential(userName, password);
                MailMessage email = new MailMessage();
                email.IsBodyHtml = true;
                email.From = new MailAddress(from);
                email.Subject = subject;
                email.Body = body;
                email.To.Add(to);
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
