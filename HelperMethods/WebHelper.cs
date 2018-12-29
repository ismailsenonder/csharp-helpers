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
using Newtonsoft.Json;
using static HelperMethods.Objects.Instagram;

namespace HelperMethods
{
    public class WebHelper
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
        // Usage: string myVal = GetQueryStringValue(Request.QueryString, "myQueryVal", "Default Value");
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

        #region GetInstagramPosts
        public string GetInstagramPosts(string accesstoken, int count)
        {
            //you have to do required tasks and get an accesstoken from instagram before being able to use this method.
            using (WebClient wc = new WebClient())
            {
                var json = wc.DownloadString("https://api.instagram.com/v1/users/self/media/recent/?access_token=" + accesstoken + "&count=" + count.ToString());
                var root = JsonConvert.DeserializeObject<RootObject>(json);

                //this is my custom string, you can generate another string for your own use
                string instStr = @"<div class=""row"">";

                foreach (Datum dt in root.data)
                {
                    instStr += @"<div class=""col-md-3 col-xs-3"">
                  <a href=""" + dt.link + @""" target=""_blank""><img src=""" + dt.images.thumbnail.url + @""" /></a></div>";
                }

                instStr += @"</div>";

                return instStr;
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
            //or you can change this method and get all values and filter it in your code.
        }
        #endregion

    }
}
