using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;

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

        #region ExecuteJavascript
        /// <summary>
        /// Runs javascript code on web page
        /// </summary>
        /// <param name="sJavascriptCode">javascript code as string</param>
        /// <returns></returns>
        public void ExecuteJavascript(string sJavascriptCode)
        {
            if (HttpContext.Current.CurrentHandler is Page)
            {
                Page p = (Page)HttpContext.Current.CurrentHandler;

                if (ScriptManager.GetCurrent(p) != null)
                {
                    ScriptManager.RegisterStartupScript(p, typeof(Page), "CustomScript", sJavascriptCode, true);
                }
                else
                {
                    p.ClientScript.RegisterStartupScript(typeof(Page), "CustomScript", sJavascriptCode, true);
                }
            }
        }

        #endregion 

        #region JsMessageBox
        /// <summary>
        /// Shows javascript alert box on web page
        /// </summary>
        /// <param name="sMessage">alert box message as string</param>
        /// <returns></returns>
        public void JsMessageBox(string sMessage)
        {
            sMessage = "alert('" + sMessage + "');";
            if (HttpContext.Current.CurrentHandler is Page)
            {
                Page p = (Page)HttpContext.Current.CurrentHandler;

                if (ScriptManager.GetCurrent(p) != null)
                {
                    ScriptManager.RegisterStartupScript(p, typeof(Page), "Message", sMessage, true);
                }
                else
                {
                    p.ClientScript.RegisterStartupScript(typeof(Page), "Message", sMessage, true);
                }
            }
        }

        #endregion

        //FIX, TEST AND SORT

        

        public string GetResponse(string url)
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

        public bool Exist(string url)
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

        public string XMLPost(string PostAddress, string xmlData)
        {
            try
            {
                WebClient wUpload = new WebClient();
                HttpWebRequest request = WebRequest.Create(PostAddress) as HttpWebRequest;
                request.Method = "POST";

                byte[] byteArray = Encoding.UTF8.GetBytes(xmlData);
                request.ContentType = "text/xml; charset=utf-8";
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();

                WebResponse response = request.GetResponse();
                Console.WriteLine(((HttpWebResponse)response).StatusDescription);
                dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream, Encoding.UTF8);
                string result = reader.ReadToEnd();

                reader.Close();
                dataStream.Close();
                response.Close();

                return result;
            }
            catch
            {
                return "-1";
            }
        }

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
                        Others o = new Others();
                        returnValue = (T)Convert.ChangeType(o.RemoveIllegalQueryCharacters(col[key].ToString()), typeof(T));
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

    }
}
