using System;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Web;

namespace HelperMethods
{
    public class FileHelpers
    {
        #region ReturnXmlAsDataSet
        public static DataSet ReturnXmlAsDataSet(string xmlPath)
        {
            string xmlData = HttpContext.Current.Server.MapPath(xmlPath);
            DataSet xmlDataSet = new DataSet();
            xmlDataSet.ReadXml(xmlData);
            return xmlDataSet;
        }
        #endregion

        #region DeleteFilesInFolderByCreateDate
        public static void DeleteFilesOlderThanDays(int dateinterval, string literalpath)
        {
            try
            {
                string[] files = Directory.GetFiles(literalpath);

                foreach (string file in files)
                {
                    FileInfo fi = new FileInfo(file);
                    //If you want to log which files are deleted, do it in this line:
                    //Console.WriteLine(fi.FullName + fi.CreationTime.ToString("yyyy-MM-dd"));
                    if (fi.CreationTime < DateTime.Now.AddDays(dateinterval))
                        fi.Delete();
                }
            }
            catch (Exception ex)
            {
                //if you want to throw an exception or make a log, do it in this line:
                //Console.WriteLine(ex.ToString());
            }
        }
        #endregion

        #region ZipCompressFile
        public static byte[] ZipCompressFile(byte[] file, object fileName, string extension)
        {
            MemoryStream zipStream = new MemoryStream();

            using (ZipArchive zip = new ZipArchive(zipStream, ZipArchiveMode.Create, true))
            {
                ZipArchiveEntry zipElement = zip.CreateEntry(fileName + "." + extension);
                Stream entryStream = zipElement.Open();
                entryStream.Write(file, 0, file.Length);
                entryStream.Flush();
                entryStream.Close();
            }
            zipStream.Position = 0;
            return zipStream.ToArray();

        }
        #endregion

        #region UncompressZipFile
        public static byte[] UncompressZipFile(byte[] docData)
        {
            byte[] unzippedData = { };
            MemoryStream zippedStream = new MemoryStream(docData);
            using (ZipArchive archive = new ZipArchive(zippedStream))
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    MemoryStream ms = new MemoryStream();
                    Stream zipStream = entry.Open();
                    zipStream.CopyTo(ms);
                    unzippedData = ms.ToArray();
                }
            }
            return unzippedData;
        }
        #endregion
    }
}
