using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

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
        public static int Factorial(int n)
        {
            if (n <= 1)
                return 1;
            else
                return n * Factorial(n - 1);
        }
        #endregion

        //converts a class instance to xml formatted string
        //NOT TESTED!
        public static string ToXML(this object instanceToConvert)
        {
            using (var stringwriter = new System.IO.StringWriter())
            {
                var serializer = new XmlSerializer(instanceToConvert.GetType());
                serializer.Serialize(stringwriter, instanceToConvert);
                return stringwriter.ToString();
            }
        }

        //converts an xml format string to the specified class instance
        //NOT TESTED
        public static T LoadFromXMLString<T>(string xmlText)
        {
            using (var stringReader = new System.IO.StringReader(xmlText))
            {
                var serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(stringReader);
            }
        }


        //BUNLARA BAK BUNLARA
        //public T XmlToClass<T>(string text)
        //{
        //    T root = default(T);

        //    XmlSerializer serializer = new XmlSerializer(typeof(T));

        //    byte[] byteArray = Encoding.UTF8.GetBytes(text);
        //    MemoryStream stream = new MemoryStream(byteArray);
        //    StreamReader reader = new StreamReader(stream);

        //    root = (T)serializer.Deserialize(reader);
        //    reader.Close();
        //    return root;
        //}


        //public String ClassToXml<T>(T classee)
        //{
        //    try
        //    {
        //        String XmlizedString = null;
        //        MemoryStream memoryStream = new MemoryStream();
        //        XmlSerializer xs = new XmlSerializer(typeof(T));
        //        XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
        //        xs.Serialize(xmlTextWriter, classee);
        //        memoryStream = (MemoryStream)xmlTextWriter.BaseStream;
        //        XmlizedString = UTF8ByteArrayToString(memoryStream.ToArray());

        //        return HttpUtility.HtmlDecode(XmlizedString);
        //    }
        //    catch (Exception e) { System.Console.WriteLine(e); return null; }
        //}

        //private String UTF8ByteArrayToString(Byte[] characters)
        //{

        //    UTF8Encoding encoding = new UTF8Encoding(true);
        //    String constructedString = encoding.GetString(characters);
        //    constructedString = constructedString.Remove(0, 1);

        //    return (constructedString);
        //}


        //public static String EvaluateAsHtml(string inputstring)
        //{
        //    if (String.IsNullOrEmpty(inputstring))
        //        return null;

        //    string pattern = @"<html.*?>(.*?)</html>";
        //    MatchCollection matches = Regex.Matches(inputstring, pattern);
        //    if (matches.Count > 0)
        //        return inputstring;
        //    else
        //    {
        //        return String.Format("<html><head></head><body>{0}</body></html>", inputstring);
        //    }

        //}


        /* write to file web
         * System.IO.File.AppendAllText(System.Web.Hosting.HostingEnvironment.MapPath("~/") + DateTime.Now.ToString("yyyy-MM-dd") + "_CreateEmailCampaign",
                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "   " + m.ClassToXml(emailInfo));
         * */

    }
}
