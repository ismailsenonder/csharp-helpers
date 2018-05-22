using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HelperMethods
{
    public static class StringHelper
    {
        #region RemoveExtraHtmlCode
        /// <summary>
        /// This is an extension method for string. 
        /// Usage: htmlString.RemoveExtraHtmlCode();
        /// Removes inline styles, classes, images and comments from an html code.
        /// This can be applied, for example, to an Outlook message prior to saving it into database
        /// </summary>
        /// <param name="htmlString">string</param>
        /// <returns>string</returns>
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
        /// <summary>
        /// This is an extension method for string. 
        /// Usage: strInput.ShortenToCharacterLimit(15);
        /// Shortens the string to the given character limit and adds "..." at the end.
        /// If the string count is smaller than the character limit, returns the original string.
        /// </summary>
        /// <param name="strInput">string</param>
        /// <param name="characterLimit">int</param>
        /// <returns>string</returns>
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
        /// <summary>
        /// Removes illegal query characters from a string.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>string</returns>
        public static string RemoveIllegalQueryCharacters(string value)
        {
            return value.Replace("\"", "")
                        .Replace("’", "")
                        .Replace("'", "")
                        .Replace(",", "");

        }
        #endregion

        // THESE FUNCTIONS NEED SORTING, SUMMARY AND TEST
        public static bool IsNumeric(this String str)
        {
            int n;
            bool isNumeric = int.TryParse(str, out n);
            return isNumeric;
        }

        public static bool IsNumeric(this object str)
        {
            bool isNumeric = false;
            if (str != null)
            {
                int n;
                isNumeric = int.TryParse(str.ToString(), out n);
            }

            return isNumeric;
        }

        public static bool IsDate(this String str)
        {
            DateTime n;
            bool isDate = DateTime.TryParse(str, out n);
            return isDate;
        }

        public static bool IsTime(this String str)
        {
            TimeSpan n;
            bool isTime = TimeSpan.TryParse(str, out n);
            return isTime;
        }

        //THERE CAN BE MORE THAN ONE DOT (.) FIX THIS METHOD!
        public static string GetFileExtension(this string str)
        {
            string oReturn = "";
            try
            {
                oReturn = str.Substring(str.IndexOf("."));
            }
            catch { }
            return oReturn;
        }

        public static bool checkNotNullOrEmpty(this DataTable table)
        {
            return (table != null && table.Rows.Count > 0 && table.Columns.Count > 0);
        }

        public static bool ContainsColumn(this DataTable table, string columnName)
        {
            DataColumnCollection columns = table.Columns;
            return columns.Contains(columnName);
        }

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
            catch (Exception ex)
            {
                try
                {
                    returnValue = (T)Convert.ChangeType(value, typeof(T));
                }
                catch
                {
                    //oLog.Error("EnsureNotNull fonksiyonunda hata oluştu ve convert edemedi: " + e);
                    return returnValue;
                }
            }

            return returnValue;
        }
    }
}
