using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;

namespace HelperMethods
{
    public class WebHelpers
    {
        #region GetWebUserIPAddress
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
        //gets QueryString and Key name, returns value. if it is null, returns (T)nullValue.
        //Usage: string myVal = GetQueryStringValue(Request.QueryString, "myQueryVal", "Default Value");
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
                        string nonIllegalStr = RemoveIllegalQueryCharacters(col[key].ToString());
                        returnValue = (T)Convert.ChangeType(nonIllegalStr, typeof(T));
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
        public string GetWebResponse(string url)
        {
            WebRequest request = WebRequest.Create(url);
            // Credentials if required
            request.Credentials = CredentialCache.DefaultCredentials;
            //method if needed
            //request.Method = "HEAD";
            WebResponse response = request.GetResponse();
            // If you want to get status description from the response:
            //string sd = ((HttpWebResponse)response).StatusDescription;
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();
            // IMPORTANT: Clean up the streams and the response.
            reader.Close();
            response.Close();
            return responseFromServer;
        }
        #endregion

        #region IsWebSiteAlive
        public bool IsWebSiteAlive(string url)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
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
        
        #region ExecuteJavascriptOnCurrentPage
        public void ExecuteJavascriptOnCurrentPage(string sJavascriptCode)
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

        #region ShowJavaScriptMessageBoxOnPage
        public void ShowJavaScriptMessageBoxOnPage(string sMessage)
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

        #region GetRequestHeaders
        public string GetRequestHeaders(string requestAddressWithParemeters, string headername)
        {
            var request = (HttpWebRequest)WebRequest.Create(requestAddressWithParemeters);
            //Example: (HttpWebRequest)WebRequest.Create("http://requestexample.net/requestexample.ashx?id=123456&g=m");
            var response = (HttpWebResponse)request.GetResponse();
            WebHeaderCollection headers = response.Headers;
            return headers.GetValues(headername)[0];
            //to get all the headers; loop in headers and return a list or array including values
        }
        #endregion

        #region DownloadFile
        private void DownloadFile(string url, string filePath)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            // Get the stream associated with the response.
            Stream receiveStream = response.GetResponseStream();
            byte[] buffer = new byte[32768];
            using (FileStream fileStream = File.Create(filePath))
            {
                while (true)
                {
                    int read = receiveStream.Read(buffer, 0, buffer.Length);
                    if (read <= 0)
                        break;
                    fileStream.Write(buffer, 0, read);
                }
            }

            return;
        }
        #endregion

        #region XMLPost
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
        #endregion

        #region RemoveIllegalQueryCharacters
        public string RemoveIllegalQueryCharacters(string value)
        {
            return value.Replace("\"", "")
                        .Replace("’", "")
                        .Replace("'", "")
                        .Replace(",", "");

        }
        #endregion
    }
}


    
