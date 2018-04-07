using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace HelperMethods
{
    public class WebHelper
    {
        #region GetWebUserIPAddress
        /// <summary>
        /// Returns Web Visitor IP Address as string
        /// </summary>
        /// <returns>string</returns>
        public string GetWebUserIPAddress()
        {
            string VisitorsIPAddr = string.Empty;
            if (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
            {
                VisitorsIPAddr = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
            }
            else if (HttpContext.Current.Request.UserHostAddress.Length != 0)
            {
                VisitorsIPAddr = HttpContext.Current.Request.UserHostAddress;
            }

            return VisitorsIPAddr;
        }
        #endregion

        #region GetQueryStringValue
        /// <summary>
        /// Gets the requested querystring value. If null, returns the specified default value.
        /// Usage: string myVal = GetQueryStringValue(Request.QueryString, "myQueryVal", "Default Value");
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="col"></param>
        /// <param name="key"></param>
        /// <param name="nullValue"></param>
        /// <returns>T</returns>
        //gets QueryString and Key name, returns value. if it is null, returns (T)nullValue.
        public T GetQueryStringValue<T>(NameValueCollection col, string key, T nullValue)
        {
            T returnValue = default(T);

            try
            {
                if (col[key] == null)
                    returnValue = nullValue;
                else
                {
                    if (typeof(T) == typeof(string))
                    {
                        returnValue = (T)Convert.ChangeType(StringHelper.RemoveIllegalQueryCharacters(col[key].ToString()), typeof(T));
                    }
                    else
                        returnValue = (T)Convert.ChangeType(col[key], typeof(T));
                }
            }
            catch
            {
                returnValue = nullValue;
            }

            return returnValue;
        }
        #endregion

        #region GetWebResponse
        /// <summary>
        /// Gets web response from given url.
        /// </summary>
        /// <param name="url"></param>
        /// <returns>string</returns>
        public string GetWebResponse(string url)
        {
            WebRequest request = WebRequest.Create(
               url);
            // If required by the server, set the credentials.
            request.Credentials = CredentialCache.DefaultCredentials;
            // Get the response.
            WebResponse response = request.GetResponse();
            // Display the status.
            //Console.WriteLine(((HttpWebResponse)response).StatusDescription);
            // Get the stream containing content returned by the server.
            Stream dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.
            string responseFromServer = reader.ReadToEnd();
            // Display the content.
            //Console.WriteLine(responseFromServer);
            // Clean up the streams and the response.
            reader.Close();
            response.Close();
            return responseFromServer;
        }
        #endregion

        #region WebSiteAlive
        /// <summary>
        /// Checks if given website is alive or not
        /// </summary>
        /// <param name="url"></param>
        /// <returns>bool</returns>
        public bool WebSiteAlive(string url)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)System.Net.WebRequest.Create(url);
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    return response.StatusCode == HttpStatusCode.OK;
                }
            }
            catch
            {
                return false;
            }
        }
        #endregion

        

    }
}
