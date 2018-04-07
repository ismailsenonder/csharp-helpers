using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace HelperMethods
{
    public class XmlHelper
    {
        #region ReturnXmlAsDataSet
        /// <summary>
        /// Returns XML file as DataSet
        /// XmlHelper a = new XmlHelper();
        /// DataTable d = a.ReturnXmlAsDataSet("~/App_Data/UserData.xml").Tables[0];
        /// </summary>
        /// <param name="xmlPath"></param>
        /// <returns>DataSet</returns>
        public DataSet ReturnXmlAsDataSet(string xmlPath)
        {
            string xmlData = HttpContext.Current.Server.MapPath(xmlPath);
            DataSet xmlDataSet = new DataSet();
            xmlDataSet.ReadXml(xmlData);
            return xmlDataSet;
        }
        #endregion

        #region XMLPost
        /// <summary>
        /// Post given xml data to given address
        /// </summary>
        /// <param name="PostAddress"></param>
        /// <param name="xmlData"></param>
        /// <returns>string</returns>
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
    }
}
