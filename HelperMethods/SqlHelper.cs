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
        #region GetRowValue
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
        #endregion



        // SORT TEST AND FIX:

        #region ExecuteNonQuery

        public int ExecuteNonQuery(string conn, string query)
        {
            int oReturn = -1;
            try
            {
                using (SqlConnection connection = new SqlConnection(
                                 conn))
                {
                    SqlCommand sqlCommand = new SqlCommand(query, connection);
                    sqlCommand.Connection.Open();
                    oReturn = sqlCommand.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                oLog.Error(ex);
            }
            return oReturn;
        }

        public int ExecuteNonQuery(string conn, string query, CommandType commandType)
        {
            int oReturn = -1;
            try
            {
                using (SqlConnection connection = new SqlConnection(
                                conn))
                {
                    SqlCommand sqlCommand = new SqlCommand(query, connection);
                    sqlCommand.CommandType = commandType;
                    sqlCommand.Connection.Open();
                    oReturn = sqlCommand.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                oLog.Error(ex);
            }
            return oReturn;
        }

        public int ExecuteNonQuery(string conn, string query, CommandType commandType, params SqlParameter[] commandParameters)
        {
            int oReturn = -1;
            try
            {
                using (SqlConnection connection = new SqlConnection(
                                conn))
                {
                    SqlCommand sqlCommand = new SqlCommand(query, connection);
                    sqlCommand.CommandType = commandType;
                    foreach (SqlParameter commandParameter in commandParameters)
                    {
                        if (commandParameter.Value == null)
                        {
                            commandParameter.Value = DBNull.Value;
                        }
                        sqlCommand.Parameters.Add(commandParameter);
                    }
                    sqlCommand.Connection.Open();
                    oReturn = sqlCommand.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                oLog.Error(ex);
            }
            return oReturn;
        }

        public int ExecuteNonQuery(string conn, SqlCommand cmd)
        {
            int oReturn = -1;

            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    cmd.Connection = con;
                    using (cmd)
                    {
                        //oLog.Trace("Başladı");
                        con.Open();
                        cmd.CommandTimeout = int.MaxValue;
                        //oLog.Info("Parametreler Start");
                        string _par = "";
                        foreach (SqlParameter p in cmd.Parameters)
                        {
                            _par += p.ParameterName;
                            _par += p.Value == null ? "null" : "'" + p.Value.ToString() + "'"; ;
                        }
                        oLog.Info(_par);
                        //oLog.Info("Parametreler End");
                        oReturn = cmd.ExecuteNonQuery();
                        //oLog.Trace("Çalıştı");
                        oLog.Info(cmd.CommandText);

                        con.Close();
                        //oLog.Trace("Kapandı");
                    }
                }
            }
            catch (Exception ex)
            {
                oLog.Error(ex);
            }
            return oReturn;
        }

        #endregion ExecuteNonQuery

        #region ExecuteReader

        public DataTable ExecuteReader(string conn, string query)
        {
            DataTable oReturn = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(
                                 conn))
                {
                    SqlCommand sqlCommand = new SqlCommand(query, connection);
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.Connection.Open();
                    SqlDataReader sqlReader = sqlCommand.ExecuteReader();
                    oReturn.Load(sqlReader);
                    sqlCommand.Connection.Close();
                }
            }
            catch (Exception ex)
            {
                oLog.Error(ex);
            }
            return oReturn;
        }

        public DataTable ExecuteReader(string conn, string query, CommandType commandType)
        {
            DataTable oReturn = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(
                                 conn))
                {
                    SqlCommand sqlCommand = new SqlCommand(query, connection);
                    sqlCommand.CommandType = commandType;
                    sqlCommand.Connection.Open();
                    SqlDataReader sqlReader = sqlCommand.ExecuteReader();
                    oReturn.Load(sqlReader);
                    sqlCommand.Connection.Close();
                }
            }
            catch (Exception ex)
            {
                oLog.Error(ex);
            }
            return oReturn;
        }

        public DataTable ExecuteReader(string conn, SqlCommand cmd)
        {
            DataSet ds = new DataSet();

            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    cmd.Connection = con;
                    using (cmd)
                    {
                        con.Open();
                        cmd.CommandTimeout = int.MaxValue;
                        var adapter = new SqlDataAdapter(cmd);
                        adapter.Fill(ds);
                        con.Close();
                    }
                }

                if (ds.Tables[0] != null)
                {
                    return ds.Tables[0];
                }
            }
            catch (Exception ex)
            {
                oLog.Error(ex);
            }

            return null;
        }

        public DataTable ExecuteReader(string conn, string query, CommandType commandType, params SqlParameter[] commandParameters)
        {
            DataTable oReturn = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(
                 conn))
                {
                    SqlCommand sqlCommand = new SqlCommand(query, connection);
                    sqlCommand.CommandType = commandType;
                    foreach (var commandParameter in commandParameters)
                    {
                        sqlCommand.Parameters.Add(commandParameter);
                    }
                    sqlCommand.Connection.Open();
                    SqlDataReader sqlReader = sqlCommand.ExecuteReader();
                    oReturn.Load(sqlReader);
                    sqlCommand.Connection.Close();
                }
            }
            catch (Exception ex)
            {
                oLog.Error(ex);
            }
            return oReturn;
        }

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

        public T GetRowValue<T>(DataRow value, string rowName)
        {
            T returnValue = default(T);
            //object returnValue = null;
            try
            {
                if (value != null && value[rowName] != DBNull.Value)
                {
                    if (typeof(T) == typeof(Nullable<Int32>))
                    {
                        returnValue = ValueConverter<T>.Instance.ConvertValue(value[rowName]);
                    }
                    else if (typeof(T) == typeof(Nullable<Decimal>))
                    {
                        returnValue = ValueConverter<T>.Instance.ConvertValue(value[rowName]);
                    }
                    else
                    {
                        returnValue = (T)value[rowName];
                    }
                }
            }
            catch
            {
                try
                {
                    // Direct cast failed, but try a type conversion just in case
                    // we can still convert the raw value correctly (for example,
                    // if the raw value is the string "true" and T is a boolean,
                    // this allows us to correctly return a true boolean value).
                    returnValue = (T)Convert.ChangeType(value[rowName], typeof(T));
                    //              return returnValue == DBNull.Value ? default(T) : (T)Convert.ChangeType(value,
                    //Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T));
                }
                catch { }
            }

            //return (returnValue == null) ?
            //   default(T) : (T)Convert.ChangeType(returnValue, typeof(T)); ;
            //        return returnValue == DBNull.Value ? default(T) : (T)Convert.ChangeType(value,
            //Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T));
            return returnValue;
        }

        public T GetRowValue<T>(DataRow value, string rowName, T DefaultValue)
        {
            //T returnValue = default(T);
            object returnValue = DefaultValue;
            try
            {
                if (value != null && value[rowName] != DBNull.Value)
                {
                    if (typeof(T) == typeof(Nullable<Int32>))
                    {
                        returnValue = Int32.TryParse(value[rowName].ToString(), out var tempVal) ? tempVal : (int?)null;
                    }
                    else if (typeof(T) == typeof(Nullable<Decimal>))
                    {
                        returnValue = Decimal.TryParse(value[rowName].ToString(), out var tempVal) ? tempVal : (decimal?)null;
                    }
                    else
                    {
                        if (value.Table.Columns[rowName].DataType == typeof(TimeSpan) && typeof(T) == typeof(String))
                        {
                            returnValue = value[rowName].ToString();
                        }
                        else
                        {
                            returnValue = value[rowName];
                        }
                    }
                }
            }
            catch
            {
                try
                {
                    // Direct cast failed, but try a type conversion just in case
                    // we can still convert the raw value correctly (for example,
                    // if the raw value is the string "true" and T is a boolean,
                    // this allows us to correctly return a true boolean value).
                    returnValue = ValueConverter<T>.Instance.ConvertValue(returnValue);
                    //              return returnValue == DBNull.Value ? default(T) : (T)Convert.ChangeType(value,
                    //Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T));
                }
                catch { }
            }

            //return (returnValue == null) ?
            //   default(T) : (T)Convert.ChangeType(returnValue, typeof(T)); ;
            //        return returnValue == DBNull.Value ? default(T) : (T)Convert.ChangeType(value,
            //Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T));
            return ValueConverter<T>.Instance.ConvertValue(returnValue);
        }

        #endregion ExecuteReader

        #region converters

        public T CreateItemFromRow<T>(DataRow row, IList<PropertyInfo> properties) where T : new()
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
                //oLog.Error(ex);
                oLog.Error("Row objeye çevrilirken şu property'de hata oldu: " + p + ". Full Hata Mesajı: " + ex);
                return item;
            }
        }

        #endregion converters

        #region MySQL

        public DataTable ExecuteReaderMySql(string conn, MySqlCommand cmd)
        {
            DataSet ds = new DataSet();

            try
            {
                using (MySqlConnection con = new MySqlConnection(conn))
                {
                    cmd.Connection = con;
                    using (cmd)
                    {
                        //oLog.Trace("Başladı");
                        con.Open();
                        cmd.CommandTimeout = int.MaxValue;
                        //oLog.Info("Parametreler Start");
                        var adapter = new MySqlDataAdapter(cmd);
                        string _par = "";
                        foreach (MySqlParameter p in cmd.Parameters)
                        {
                            _par += p.ParameterName;
                            _par += p.Value == null ? "null" : "'" + p.Value.ToString() + "'"; ;
                        }
                        oLog.Info(_par);

                        //oLog.Info("Parametreler End");
                        oLog.Info(cmd.CommandText);
                        adapter.Fill(ds);
                        //oLog.Trace("Çalıştı");
                        con.Close();
                        //oLog.Trace("Kapandı");
                    }
                }

                if (ds.Tables[0] != null)
                {
                    return ds.Tables[0];
                }
            }
            catch (Exception ex)
            {
                oLog.Error(ex);
            }

            return null;
        }

        public T ExecuteReaderMySql<T>(string conn, string query, CommandType commandType, string rowname, T defaultvalue)
        {
            DataSet ds = new DataSet();

            try
            {
                using (MySqlConnection con = new MySqlConnection(conn))
                {
                    MySqlCommand cmd = new MySqlCommand(query);
                    cmd.CommandType = commandType;
                    cmd.Connection = con;
                    using (cmd)
                    {
                        //oLog.Trace("Başladı");
                        con.Open();
                        cmd.CommandTimeout = int.MaxValue;
                        //oLog.Info("Parametreler Start");
                        var adapter = new MySqlDataAdapter(cmd);
                        string _par = "";
                        foreach (MySqlParameter p in cmd.Parameters)
                        {
                            _par += p.ParameterName;
                            _par += p.Value == null ? "null" : "'" + p.Value.ToString() + "'"; ;
                        }
                        oLog.Info(_par);
                        //oLog.Info("Parametreler End");
                        adapter.Fill(ds);
                        //oLog.Trace("Çalıştı");
                        con.Close();
                        //oLog.Trace("Kapandı");
                    }
                }

                if (ds.Tables[0] != null)
                {
                    if (ds.Tables[0].Rows.Count != 0)
                    {
                        return GetRowValue<T>(ds.Tables[0].Rows[0], rowname);
                    }
                }
            }
            catch (Exception ex)
            {
                oLog.Error(ex);
            }

            return defaultvalue;
        }
        public List<Item> ExecuteReaderMySql(string conn, string query, CommandType commandType, string[] rownames)
        {
            DataSet ds = new DataSet();
            List<Item> oReturn = new List<Item>();
            try
            {
                using (MySqlConnection con = new MySqlConnection(conn))
                {
                    MySqlCommand cmd = new MySqlCommand(query);
                    cmd.CommandType = commandType;
                    cmd.Connection = con;
                    using (cmd)
                    {
                        //oLog.Trace("Başladı");
                        con.Open();
                        cmd.CommandTimeout = int.MaxValue;
                        //oLog.Info("Parametreler Start");
                        var adapter = new MySqlDataAdapter(cmd);
                        string _par = "";
                        foreach (MySqlParameter p in cmd.Parameters)
                        {
                            _par += p.ParameterName;
                            _par += p.Value == null ? "null" : "'" + p.Value.ToString() + "'"; ;
                        }
                        oLog.Info(_par);
                        //oLog.Info("Parametreler End");
                        adapter.Fill(ds);
                        //oLog.Trace("Çalıştı");
                        con.Close();
                        //oLog.Trace("Kapandı");
                    }
                }

                if (ds.Tables[0] != null)
                {
                    if (ds.Tables[0].Rows.Count != 0)
                    {
                        foreach (var rowname in rownames)
                        {
                            oReturn.Add(new Item { Name = rowname, Value = GetRowValue<string>(ds.Tables[0].Rows[0], rowname, "") });
                        }
                    }
                }
                return oReturn;
            }
            catch (Exception ex)
            {
                oLog.Error(ex);
            }

            return oReturn;
        }

        public List<Item> ExecuteReaderMySql(string conn, MySqlCommand cmd, string[] rownames)
        {
            DataSet ds = new DataSet();
            List<Item> oReturn = new List<Item>();
            try
            {
                using (MySqlConnection con = new MySqlConnection(conn))
                {
                    //MySqlCommand cmd = new MySqlCommand(query);
                    //cmd.CommandType = commandType;
                    cmd.Connection = con;
                    using (cmd)
                    {
                        //oLog.Trace("Başladı");
                        con.Open();
                        cmd.CommandTimeout = int.MaxValue;
                        //oLog.Info("Parametreler Start");
                        var adapter = new MySqlDataAdapter(cmd);
                        string _par = "";
                        foreach (MySqlParameter p in cmd.Parameters)
                        {
                            _par += p.ParameterName;
                            _par += p.Value == null ? "null" : "'" + p.Value.ToString() + "'"; ;
                        }
                        oLog.Info(_par);
                        //oLog.Info("Parametreler End");
                        adapter.Fill(ds);
                        //oLog.Trace("Çalıştı");
                        con.Close();
                        //oLog.Trace("Kapandı");
                    }
                }

                if (ds.Tables[0] != null)
                {
                    if (ds.Tables[0].Rows.Count != 0)
                    {
                        foreach (var rowname in rownames)
                        {
                            oReturn.Add(new Item { Name = rowname, Value = GetRowValue<string>(ds.Tables[0].Rows[0], rowname, "") });
                        }
                    }
                }
                return oReturn;
            }
            catch (Exception ex)
            {
                oLog.Error(ex);
            }

            return oReturn;
        }
        public T ExecuteReaderMySql<T>(string conn, MySqlCommand cmd, string rowname, T defaultvalue)
        {
            DataSet ds = new DataSet();

            try
            {
                using (MySqlConnection con = new MySqlConnection(conn))
                {
                    //MySqlCommand cmd = new MySqlCommand(query);
                    //cmd.CommandType = commandType;
                    cmd.Connection = con;
                    using (cmd)
                    {
                        //oLog.Trace("Başladı");
                        con.Open();
                        cmd.CommandTimeout = int.MaxValue;
                        //oLog.Info("Parametreler Start");
                        var adapter = new MySqlDataAdapter(cmd);
                        string _par = "";
                        foreach (MySqlParameter p in cmd.Parameters)
                        {
                            _par += p.ParameterName;
                            _par += p.Value == null ? "null" : "'" + p.Value.ToString() + "'"; ;
                        }
                        oLog.Info(_par);
                        //oLog.Info("Parametreler End");
                        adapter.Fill(ds);
                        //oLog.Trace("Çalıştı");
                        con.Close();
                        //oLog.Trace("Kapandı");
                    }
                }

                if (ds.Tables[0] != null)
                {
                    if (ds.Tables[0].Rows.Count != 0)
                    {
                        return GetRowValue<T>(ds.Tables[0].Rows[0], rowname);
                    }
                }
            }
            catch (Exception ex)
            {
                oLog.Error(ex);
            }

            return defaultvalue;
        }

        public int ExecuteNonQueryMySql(string conn, MySqlCommand cmd)
        {
            int oReturn = -1;

            try
            {
                using (MySqlConnection con = new MySqlConnection(conn))
                {
                    cmd.Connection = con;
                    using (cmd)
                    {
                        //oLog.Trace("Başladı");
                        con.Open();
                        cmd.CommandTimeout = int.MaxValue;
                        //oLog.Info("Parametreler Start");
                        string _par = "";
                        foreach (MySqlParameter p in cmd.Parameters)
                        {
                            _par += p.ParameterName;
                            _par += p.Value == null ? "null" : "'" + p.Value.ToString() + "'"; ;
                        }
                        oLog.Info(_par);
                        //oLog.Info("Parametreler End");
                        oReturn = cmd.ExecuteNonQuery();
                        //oLog.Trace("Çalıştı");
                        oLog.Info(cmd.CommandText);

                        con.Close();
                        //oLog.Trace("Kapandı");
                    }
                }
            }
            catch (Exception ex)
            {
                oLog.Error(ex);
            }
            return oReturn;
        }

        #endregion MySQL
    }
}
