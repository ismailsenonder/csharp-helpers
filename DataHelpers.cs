using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace HelperMethods
{
    public static class DataHelpers
    {
        #region GetCultureCodes

        public static List<string> GetCultureCodes()
        {
            CultureInfo[] cinfo = CultureInfo.GetCultures(CultureTypes.AllCultures);
            List<string> cultureCodes = new List<string>();
            foreach (CultureInfo cul in cinfo)
            {
                cultureCodes.Add(cul.DisplayName + " : " + cul.Name);
            }
            return cultureCodes;
        }

        #endregion

        #region CheckNullOrEmpty
        //returns 1 if datatable is populated, 0 if not.
        public static bool CheckNullOrEmpty(this DataTable table)
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

        #region ValueConverter
        internal class ValueConverter<T>
        {
            internal static readonly ValueConverter<T> Instance = Initialize();

            private static ValueConverter<T> Initialize()
            {
                Type type = typeof(T);
                Type underlyingType = Nullable.GetUnderlyingType(type);

                if (underlyingType != null)
                {
                    // if T is nullable, use reflection to create an instance of NullableValueConverter
                    Type converterType = typeof(NullableValueConverter<>);
                    converterType = converterType.MakeGenericType(underlyingType);
                    return (ValueConverter<T>)Activator.CreateInstance(converterType);
                }
                else
                {
                    // otherwise use a normal converter
                    return new ValueConverter<T>();
                }
            }

            internal ValueConverter()
            {
            }

            internal virtual T ConvertValue(object value)
            {
                return (T)Convert.ChangeType(value, typeof(T));
            }
        }

        internal class NullableValueConverter<T> : ValueConverter<T?> where T : struct
        {
            public NullableValueConverter()
                : base()
            {
            }

            internal override T? ConvertValue(object value)
            {
                if (value == null)
                    return null;
                return (T)Convert.ChangeType(value, typeof(T));
            }
        }

        #endregion

        #region GetRowValue
        /// <summary>
        /// Returns column value of a given row by converting it to given type T. 
        /// If the value is null, returns given default value of type T.
        /// </summary>
        /// <param name="value">DataRow</param>
        /// <param name="columnName">Column Name</param>
        /// <param name="DefaultValue">Default Value if value is null</param>
        /// <returns>T</returns>
        public static T GetRowValue<T>(this DataRow value, string columnName, T DefaultValue)
        {
            //T returnValue = default(T);
            object returnValue = DefaultValue;
            try
            {
                if (value != null && value[columnName] != DBNull.Value)
                {
                    if (typeof(T) == typeof(Nullable<Int32>))
                    {
                        int a;
                        returnValue = Int32.TryParse(value[columnName].ToString(), out a) ? a : (int?)null;
                    }
                    else if (typeof(T) == typeof(Nullable<Decimal>))
                    {
                        decimal b;
                        returnValue = Decimal.TryParse(value[columnName].ToString(), out b) ? b : (decimal?)null;
                    }
                    else
                    {
                        if (value.Table.Columns[columnName].DataType == typeof(TimeSpan) && typeof(T) == typeof(String))
                        {
                            returnValue = value[columnName].ToString();
                        }
                        else
                        {
                            returnValue = value[columnName];
                        }
                    }
                }
            }
            catch
            {
                try
                {
                    returnValue = ValueConverter<T>.Instance.ConvertValue(returnValue);
                }
                catch
                {
                    throw new Exception("Unable to cast...");
                }
            }

            return ValueConverter<T>.Instance.ConvertValue(returnValue);
        }
        #endregion

        #region CreateGenericObjectFromDataRow
        /// <summary>
        /// Converts datarow to an object 
        /// Object property names and datarow column names must be identical.
        /// </summary>
        /// <param name="row">DataRow</param>
        /// <param name="properties">PropertyInfo list</param>
        /// <returns>Object</returns>
        /// USAGE EXAMPLE:
        /// MyObject myobject = new MyObject();
        /// DataRow r = table.Rows[0];
        /// IList<PropertyInfo> properties = typeof(MyObject).GetProperties().ToList();
        /// myobject = CreateItemFromRow<MyObject>(r, properties);
        public static T CreateItemFromRow<T>(DataRow row, IList<PropertyInfo> properties) where T : new()
        {
            string p = "";
            T item = new T();
            try
            {
                foreach (var property in properties)
                {
                    p = property.Name;
                    if (row.Table.Columns.Contains(p))
                    {
                        Type t = null;
                        if (property.PropertyType == typeof(string))
                            t = typeof(string);
                        else
                            t = Nullable.GetUnderlyingType(property.PropertyType);
                        if (row[p] != DBNull.Value && property.CanWrite)
                            property.SetValue(item, Convert.ChangeType(row[p], t));
                    }
                }
                return item;
            }
            catch (Exception ex)
            {
                return item;
            }
        }
        #endregion

        #region getQueryFromMySqlCommand
        /// <summary>
        /// Returns the full command text with parameters from an SqlCommand
        /// Good for debugging where using SQL profiler is not an option.
        /// You can change the command type for different kind of connection. i.e MySqlCommand
        /// </summary>
        /// <param name="cmd">SqlCommand</param>
        /// <returns>string</returns>
        public static string getQueryFromMySqlCommand(this SqlCommand cmd)
        {
            string CommandTxt = cmd.CommandText;
            foreach (SqlParameter parms in cmd.Parameters)
            {
                string val = String.Empty;
                if (parms.DbType.Equals(DbType.String) || parms.DbType.Equals(DbType.DateTime))
                    val = "'" + Convert.ToString(parms.Value).Replace(@"\", @"\\").Replace("'", @"\'") + "'";
                if (parms.DbType.Equals(DbType.Int16) || parms.DbType.Equals(DbType.Int32) || parms.DbType.Equals(DbType.Int64) || parms.DbType.Equals(DbType.Decimal) || parms.DbType.Equals(DbType.Double))
                    val = Convert.ToString(parms.Value);
                //string paramname = "@" + parms.ParameterName;
                string paramname = parms.ParameterName;
                CommandTxt = CommandTxt.Replace(paramname, val);
            }
            return (CommandTxt);
        }
        #endregion

        #region CreatePassword
        public static string CreatePassword(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }
        #endregion
    }
}
