using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
