using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace HelperMethods
{
    public static class ConvertHelpers
    {
        #region CelsiusToFahrenheit
        public static double CelsiusToFahrenheit(string temperatureCelsius)
        {
            return (Double.Parse(temperatureCelsius) * 9 / 5) + 32;
        }
        #endregion

        #region FahrenheitToCelsius
        public static double FahrenheitToCelsius(string temperatureFahrenheit)
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
            if (decimal.TryParse(value.ToString()
            .Replace(",", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator)
            .Replace(".", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator),
            NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out retVal))
                return retVal;
            else
                return null;
        }
        #endregion

        #region ListToDataTable
        public static DataTable ToDataTable<T>(this List<T> data)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
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
        public static int Factorial(int n)
        {
            if (n <= 1)
                return 1;
            else
                return n * Factorial(n - 1);
        }
        #endregion

        #region ObjectOrListToXML
        //converts a generic object or a list of objects to xml formatted string
        public static string ToXML(this object instanceToConvert)
        {
            using (var stringwriter = new System.IO.StringWriter())
            {
                var serializer = new XmlSerializer(instanceToConvert.GetType());
                serializer.Serialize(stringwriter, instanceToConvert);
                return stringwriter.ToString();
            }
        }
        #endregion

        #region XmlStringToObject
        //this method converts an xml string to a generic object (the exact opposite of ToXML method, but only for objects, not for lists).
        public static T XmlStringToObject<T>(string xmlText)
        {
            using (var stringReader = new System.IO.StringReader(xmlText))
            {
                var serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(stringReader);
            }
        }
        #endregion
        
        #region EnsureNotNull
        //Converts a variable to another type
        //ensuring that if the value is null or not convertible, returns the default value
        public static T EnsureNotNull<T>(this object value, T DefaultValue)
        {
            T returnValue = default(T);
            try
            {
                if (value != null)
                {
                    //TimeSpan is not IConvertible, so this has to be done:
                    if (value.GetType() == typeof(TimeSpan) && typeof(T) == typeof(String))
                    {
                        TimeSpan ts = TimeSpan.Parse(value.ToString());
                        DateTime d = new DateTime(ts.Ticks);
                        returnValue = (T)Convert.ChangeType(d.ToString("HH:mm:ss"), typeof(T));
                    }
                    else
                    {
                        returnValue = (T)value;
                    }
                }
                else
                {
                    returnValue = DefaultValue;
                }

            }
            catch
            {
                try
                {
                    returnValue = (T)Convert.ChangeType(value, typeof(T));
                }
                catch
                {
                    return returnValue;
                }
            }

            return returnValue;
        }
        #endregion

        #region RemoveExtraHtmlCode
        public static string RemoveExtraHtmlCode(this string htmlString)
        {
            htmlString = Regex.Replace(htmlString, @"(<style.+?</style>)|(<script.+?</script>)", "", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            htmlString = Regex.Replace(htmlString, @"(<img.+?>)", "", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            htmlString = Regex.Replace(htmlString, @"(<o:.+?</o:.+?>)", "", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            htmlString = Regex.Replace(htmlString, @"<!--.+?-->", "", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            htmlString = Regex.Replace(htmlString, @"class=.+?>", ">", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            htmlString = Regex.Replace(htmlString, @"class=.+?\s", " ", RegexOptions.IgnoreCase | RegexOptions.Singleline);

            return htmlString;
        }
        #endregion

        #region ShortenToCharacterLimit
        public static string ShortenToCharacterLimit(this string strInput, int characterLimit)
        {
            if (strInput.Length > characterLimit)
            {
                return strInput.Substring(0, characterLimit) + "...";
            }
            else
            {
                return strInput;
            }
        }
        #endregion

        #region RemoveIllegalQueryCharacters
        public static string RemoveIllegalQueryCharacters(this string value)
        {
            return value.Replace("\"", "")
                        .Replace("’", "")
                        .Replace("'", "")
                        .Replace(",", "");

        }
        #endregion
    }
}
