using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperMethods
{
    public static class DataHelper
    {

        public static bool checkNotNullOrEmpty(this DataTable table)
        {
            return (table != null && table.Rows.Count > 0 && table.Columns.Count > 0);
        }

        public static bool ContainsColumn(this DataTable table, string columnName)
        {
            DataColumnCollection columns = table.Columns;
            return columns.Contains(columnName);
        }

    }
}
