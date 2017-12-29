using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperMethods
{
    public static class SqlHelper
    {
        /// <summary>
        /// Returns column value of a given row by converting it to given type T. 
        /// If the value is null, returns given default value of type T.
        /// </summary>
        /// <param name="value">DataRow</param>
        /// <param name="columnName">Column Name</param>
        /// <param name="DefaultValue">Default Value if value is null</param>
        /// <returns>T</returns>
        public static T GetRowValue<T>(DataRow value, string columnName, T DefaultValue)
        {
            T returnValue = default(T);
            try
            {
                if (value != null && value[columnName] != DBNull.Value)
                {
                    returnValue = (T)value[columnName];
                }
            }
            catch
            {
                try
                {
                    returnValue = (T)Convert.ChangeType(value[columnName], typeof(T));
                }
                catch { }
            }

            return returnValue;
        }
    }
}
