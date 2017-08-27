using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HelperMethods
{
    public class StringHelper
    {
        #region RemoveExtraHtmlCode
        /// <summary>
        /// Removes inline styles, classes, images and comments from an html code.
        /// This can be applied, for example, to an Outlook message prior to saving it into database
        /// </summary>
        /// <param name="htmlString">the html string to be converted</param>
        /// <returns>string</returns>
        /// Not tested
        public string RemoveExtraHtmlCode(string htmlString)
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
    }
}
