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
        /// Removes inline styles, classes, images and comments from an html code.
        /// This can be applied, for example, to an Outlook message prior to saving it into database
        /// </summary>
        /// <param name="htmlString">the html string to be converted</param>
        /// <returns>string</returns>
        /// Not tested
        public static string RemoveExtraHtmlCode(string htmlString)
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
        /// Shortens the string to the given character limit and adds "..." at the end.
        /// If the string count is smaller than the character limit, returns the original string.
        /// </summary>
        /// <param name="strInput">the string to be shortened</param>
        /// <param name="characterLimit">character limit (integer)</param>
        /// <returns>string</returns>
        /// Not tested
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
    }
}
