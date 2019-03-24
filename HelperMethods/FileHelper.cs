using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;

namespace HelperMethods
{
    public static class FileHelper
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

        #region ExcelImportToDataTable
        //NOT TESTED
        public static DataTable ExcelImportToDataTable(string filePath)
        {
            string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties=\"Excel 12.0;HDR=No;IMEX=1\"";
            OleDbConnection conn = new OleDbConnection(connectionString);
            OleDbCommand cmd = new OleDbCommand();
            OleDbDataAdapter dataAdapter = new OleDbDataAdapter();
            DataTable dt = new DataTable();
            cmd.Connection = conn;
            conn.Open();
            DataTable dtSchema;
            dtSchema = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            string ExcelSheetName = dtSchema.Rows[0]["TABLE_NAME"].ToString();
            conn.Close();
            conn.Open();
            cmd.CommandText = "SELECT * From [" + ExcelSheetName + "]";
            dataAdapter.SelectCommand = cmd;
            dataAdapter.Fill(dt);
            conn.Close();
            return dt;
        }
        #endregion

        #region ClientSaveAs
        public static void ClientSaveAs(MemoryStream a, string file_name)
        {
            if (HttpContext.Current.CurrentHandler is Page)
            {
                Page p = (Page)HttpContext.Current.CurrentHandler;
                p.Response.Clear();
                p.Response.ClearContent();
                p.Response.ClearHeaders();
                p.Response.Buffer = true;
                p.Response.ContentType = "application/vnd.ms-excel"; //file extension (.xls, xlsx, pdf etc..)

                //p.Response.ContentType = "application/" + file_name.SplitStr(".")[1]; //file extension (.xls, xlsx, pdf etc..)
                p.Response.AppendHeader("Content-Disposition", "attachment; filename=" + file_name);
                p.Response.BinaryWrite(a.ToArray());

                try
                {
                    p.Response.Flush();
                    //p.Response.End();
                }
                catch
                {
                    //nothing
                }
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


        public void DeleteFilesBeforeDate(int dateinterval, string literalpath)
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
    }
}
