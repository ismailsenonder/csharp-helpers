using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperMethods
{
    public static class DataHelper
    {
        #region checkNotNullOrEmpty
        public static bool checkNotNullOrEmpty(this DataTable table)
        {
            return (table != null && table.Rows.Count > 0 && table.Columns.Count > 0);
        }
        #endregion

        #region ContainsColumn
        public static bool ContainsColumn(this DataTable table, string columnName)
        {
            DataColumnCollection columns = table.Columns;
            return columns.Contains(columnName);
        }
        #endregion

        #region GetCultureCodes

        public static Dictionary<string,string> GetCultureCodes()
        {
            CultureInfo[] cinfo = CultureInfo.GetCultures(CultureTypes.AllCultures);
            Dictionary<string, string> cultureCodes = new Dictionary<string, string>();
            foreach (CultureInfo cul in cinfo)
            {
                cultureCodes.Add(cul.DisplayName, cul.Name);
            }
            return cultureCodes;
        }
        
        #endregion
    }
}
