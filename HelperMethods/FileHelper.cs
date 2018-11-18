using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
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
    }
}
