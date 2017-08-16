using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
        /// <param name="xmlPath">the path of the xml file as string</param>
        /// <returns>DataSet</returns>
        /// TESTED +
        public DataSet ReturnXmlAsDataSet(string xmlPath)
        {
            string xmlData = HttpContext.Current.Server.MapPath(xmlPath);
            DataSet xmlDataSet = new DataSet();
            xmlDataSet.ReadXml(xmlData);
            return xmlDataSet;
        }
        #endregion
    }
}
