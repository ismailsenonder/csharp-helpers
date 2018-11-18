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
            catch (Exception ex)
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
        //public string convertNumberToText(double tutar)
        //{
        //    string sTutar = tutar.ToString("F2").Replace('.', ','); // Replace('.',',') ondalık ayracının . olma durumu için            
        //    string lira = sTutar.Substring(0, sTutar.IndexOf(',')); //tutarın tam kısmı
        //    string kurus = sTutar.Substring(sTutar.IndexOf(',') + 1, 2);
        //    string yazi = "";

        //    string[] birler = { "", "bir", "iki", "üç", "dört", "beş", "alti", "yedi", "sekiz", "dokuz" };
        //    string[] onlar = { "", "on", "yirmi", "otuz", "kırk", "elli", "altmış", "yetmiş", "seksen", "doksan" };
        //    string[] binler = { "Katrilyon", "Trilyon", "Milyar", "Milyon", "Bin", "" }; //KATRİLYON'un önüne ekleme yapılarak artırabilir.

        //    int grupSayisi = 6; //sayıdaki 3'lü grup sayısı. katrilyon içi 6. (1.234,00 daki grup sayısı 2'dir.)
        //                        //KATRİLYON'un başına ekleyeceğiniz her değer için grup sayısını artırınız.

        //    lira = lira.PadLeft(grupSayisi * 3, '0'); //sayının soluna '0' eklenerek sayı 'grup sayısı x 3' basakmaklı yapılıyor.            

        //    string grupDegeri;

        //    for (int i = 0; i < grupSayisi * 3; i += 3) //sayı 3'erli gruplar halinde ele alınıyor.
        //    {
        //        grupDegeri = "";

        //        if (lira.Substring(i, 1) != "0")
        //            grupDegeri += birler[Convert.ToInt32(lira.Substring(i, 1))] + "Yüz"; //yüzler                

        //        if (grupDegeri == "birYüz") //biryüz düzeltiliyor.
        //            grupDegeri = "Yüz";

        //        grupDegeri += onlar[Convert.ToInt32(lira.Substring(i + 1, 1))]; //onlar

        //        grupDegeri += birler[Convert.ToInt32(lira.Substring(i + 2, 1))]; //birler                

        //        if (grupDegeri != "") //binler
        //            grupDegeri += binler[i / 3];

        //        if (grupDegeri == "birBin") //birbin düzeltiliyor.
        //            grupDegeri = "bin";

        //        yazi += grupDegeri;
        //    }

        //    if (yazi != "")
        //        yazi += "TL";

        //    int yaziUzunlugu = yazi.Length;

        //    if (kurus.Substring(0, 1) != "0") //kuruş onlar
        //        yazi += onlar[Convert.ToInt32(kurus.Substring(0, 1))];

        //    if (kurus.Substring(1, 1) != "0") //kuruş birler
        //        yazi += birler[Convert.ToInt32(kurus.Substring(1, 1))];

        //    if (yazi.Length > yaziUzunlugu)
        //        yazi += "kuruştur.";
        //    else
        //        yazi += "sıfırkuruştur.";

        //    return yazi;
        //}


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
