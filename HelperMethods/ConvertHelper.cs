using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperMethods
{
    public static class ConvertHelper
    {
        #region CelsiusToFahrenheit
        public static double CelsiusToFahrenheit(this string temperatureCelsius)
        {
            return (Double.Parse(temperatureCelsius) * 9 / 5) + 32;
        }
        #endregion

        #region FahrenheitToCelsius
        public static double FahrenheitToCelsius(this string temperatureFahrenheit)
        {
            return (Double.Parse(temperatureFahrenheit) - 32) * 5 / 9;
        }
        #endregion

        #region ToDecimal
        public static decimal? ToDecimal(this object value)
        {
            decimal retVal = 0;
            bool converted = decimal.TryParse(value.ToString().Replace(",", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator).Replace(".", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator),
                NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out retVal);
            if (converted)
            {
                return retVal;
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region ListToDataTable
        public static DataTable ToDataTable<T>(this List<T> data)
        {
            PropertyDescriptorCollection properties =
               TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;
        }
        #endregion

    }
}
