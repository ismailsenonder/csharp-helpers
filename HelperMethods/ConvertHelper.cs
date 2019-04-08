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
        //convert an object (usually a string) to decimal number. You can use comma (,) or dot (.) for decimal seperator.
        //returns null if given object is not convertible to decimal.
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

        #region ToTurkishMoneyString
        public static string ToTurkishMoneyString(this double amount)
        {
            string strAmount = amount.ToString("F2").Replace('.', ',');           
            string lira = strAmount.Substring(0, strAmount.IndexOf(','));
            string kurus = strAmount.Substring(strAmount.IndexOf(',') + 1, 2);
            string retVal = "";

            string[] units = { "", "BİR", "İKİ", "ÜÇ", "DÖRT", "BEŞ", "ALTI", "YEDİ", "SEKİZ", "DOKUZ" };
            string[] tens = { "", "ON", "YİRMİ", "OTUZ", "KIRK", "ELLİ", "ALTMIŞ", "YETMİŞ", "SEKSEN", "DOKSAN" };
            string[] thousands = { "MİLYAR", "MİLYON", "BİN", "" }; //Add more at the beginning of this array if needed.


            //This number indicates the count of every 3 digit groups in the amount
            //We have up to Billions (Milyar in Turkish) in thousands array so it will be; 1,000,000,000.00 = 4
            //if you add more to thousands array, you have to raise this count.
            int threeDigitsGroupCount = 4;

            lira = lira.PadLeft(threeDigitsGroupCount * 3, '0');           

            string groupValue;

            for (int i = 0; i < threeDigitsGroupCount * 3; i += 3)
            {
                groupValue = "";

                if (lira.Substring(i, 1) != "0")
                    groupValue += units[Convert.ToInt32(lira.Substring(i, 1))] + "YÜZ";                

                if (groupValue == "BİRYÜZ")
                    groupValue = "YÜZ";

                groupValue += tens[Convert.ToInt32(lira.Substring(i + 1, 1))];

                groupValue += units[Convert.ToInt32(lira.Substring(i + 2, 1))];                

                if (groupValue != "")
                    groupValue += thousands[i / 3];

                if (groupValue == "BİRBİN")
                    groupValue = "BİN";

                retVal += groupValue;
            }

            if (retVal != "")
                retVal += " LİRA ";

            int stringLength = retVal.Length;

            if (kurus.Substring(0, 1) != "0")
                retVal += tens[Convert.ToInt32(kurus.Substring(0, 1))];

            if (kurus.Substring(1, 1) != "0")
                retVal += units[Convert.ToInt32(kurus.Substring(1, 1))];

            if (retVal.Length > stringLength)
                retVal += " Kuruş";
            //else
            //    retVal += "SIFIR Kuruş";

            return retVal;
        }
        #endregion

        #region Factorial
        public static int factorial(int n)
        {
            if (n <= 1)
                return 1;
            else
                return n * factorial(n - 1);
        }
        #endregion


    }
}
