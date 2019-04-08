using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HelperMethods
{
    public static class StringHelper
    {
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

        #region TypeCheckers
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
        #endregion

        #region EnsureNotNull
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

        #region Base64Encode
        public static string Base64Encode(this string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
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

        //WORK ON THESE
        


        //test
        //THERE CAN BE MORE THAN ONE DOT (.) FIX THIS METHOD!
        //public static string GetFileExtension(this string str)
        //{
        //    string oReturn = "";
        //    try
        //    {
        //        oReturn = str.Substring(str.IndexOf("."));
        //    }
        //    catch { }
        //    return oReturn;
        //}
    }
}
