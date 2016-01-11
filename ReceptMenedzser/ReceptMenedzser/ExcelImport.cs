using System;
using System.Windows;
using System.Data;
using System.Data.OleDb;
//using Excel = Microsoft.Office.Interop.Excel;

namespace ReceptMenedzser
{
    public class ExcelImport
    {
        public static void Import(string fileName)
        {
            System.Data.OleDb.OleDbConnection myConnection = new System.Data.OleDb.OleDbConnection(
                        "Provider=Microsoft.ACE.OLEDB.12.0; " +
                         "data source='" + fileName + "';" +
                            "Extended Properties=\"Excel 12.0;HDR=YES;IMEX=1\" ");
            myConnection.Open();
            DataTable mySheets = myConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
            DataTable dt = makeDataTableFromSheetName(fileName);
            
            // If there is only one row (the column names)
            if (dt.Rows.Count < 2)
                return;

            String QueryString = "insert into recept (name, lang, acc, group_id, subgroup_id, fo_osszetevo_id, prep, desc, com, foodpic) VALUES";
            bool emptyRow = false;
            for (int rCnt = 1; rCnt < dt.Rows.Count && emptyRow == false; rCnt++)
            {
                emptyRow = true;
                string rowSQL = "(";
                for (int cCnt = 0; cCnt < dt.Columns.Count; cCnt++)
                {
                    string cellValue = dt.Rows[rCnt][cCnt].ToString();
                    if (cellValue != null)
                        emptyRow = false;
                    rowSQL += "'" + cellValue + "',";
                }
                if (emptyRow == false)
                {
                    rowSQL = rowSQL.Remove(rowSQL.Length - 1);
                    rowSQL += "),";
                    QueryString += rowSQL;
                }
            }
            QueryString = QueryString.Remove(QueryString.Length - 1);
            //MessageBox.Show(QueryString);
            DBManager.QueryCommand(QueryString);
        }

        private static DataTable makeDataTableFromSheetName(string filename)
        {
            System.Data.OleDb.OleDbConnection myConnection = new System.Data.OleDb.OleDbConnection(
            "Provider=Microsoft.ACE.OLEDB.12.0; " +
            "data source='" + filename + "';" +
            "Extended Properties=\"Excel 12.0;HDR=YES;IMEX=1\" ");

            DataTable dtImport = new DataTable();
            System.Data.OleDb.OleDbDataAdapter myImportCommand = new System.Data.OleDb.OleDbDataAdapter("select * from [Receptek$]", myConnection);
            myImportCommand.Fill(dtImport);
            return dtImport;
        }

        //    // Read the 0. sheet until the first ocurence of an empty row and save data into db
        //    // Expect that the first row contains the names of the columns
        //    public static void Import(string fileName)
        //    {
        //        Excel.Application xlApp = new Excel.Application();
        //        Excel.Workbook xlWorkBook;
        //        Excel.Worksheet xlWorkSheet;
        //        Excel.Range range;
        //        try
        //        {
        //            xlWorkBook = xlApp.Workbooks.Open(fileName);
        //            //Excel.Workbook xlWorkBook = xlApp.Workbooks.Open(fileName, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
        //            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

        //            range = xlWorkSheet.UsedRange;
        //            // If there is only one row (the column names)
        //            if (range.Rows.Count < 2)
        //                return;

        //            String QueryString = "insert into recept (name, lang, acc, group_id, subgroup_id, fo_osszetevo_id, prep, desc, com, foodpic) VALUES";
        //            bool emptyRow = false;
        //            for (int rCnt = 2; rCnt <= range.Rows.Count && emptyRow == false; rCnt++)
        //            {
        //                emptyRow = true;
        //                string rowSQL = "(";
        //                for (int cCnt = 1; cCnt <= range.Columns.Count; cCnt++)
        //                {
        //                    string cellValue = Convert.ToString((range.Cells[rCnt, cCnt] as Excel.Range).Value2);
        //                    if (cellValue != null)
        //                        emptyRow = false;
        //                    rowSQL += "'" + cellValue + "',";
        //                }
        //                if (emptyRow == false)
        //                {
        //                    rowSQL = rowSQL.Remove(rowSQL.Length - 1);
        //                    rowSQL += "),";
        //                    QueryString += rowSQL;
        //                }
        //            }
        //            QueryString = QueryString.Remove(QueryString.Length - 1);
        //            DBManager.QueryCommand(QueryString);
        //            xlWorkBook.Close(true, null, null);
        //            xlApp.Quit();

        //            releaseObject(xlWorkSheet);
        //            releaseObject(xlWorkBook);
        //            releaseObject(xlApp);
        //        }
        //        catch(Exception ex)
        //        {
        //            MessageBox.Show("At Excel import: " + ex.Message);
        //        }

        //    }


        //    private static void releaseObject(object obj)
        //    {
        //        try
        //        {
        //            System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
        //            obj = null;
        //        }
        //        catch (Exception ex)
        //        {
        //            obj = null;
        //            MessageBox.Show("Unable to release the Object " + ex.ToString());
        //        }
        //        finally
        //        {
        //            GC.Collect();
        //        }
        //    }
    }
}