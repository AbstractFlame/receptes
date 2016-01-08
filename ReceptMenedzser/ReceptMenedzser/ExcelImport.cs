using System;
using System.Windows;
using Excel = Microsoft.Office.Interop.Excel;

namespace ReceptMenedzser
{
    public class ExcelImport
    {
        // Read the 0. sheet until the first ocurence of an empty row and save data into db
        // Expect that the first row contains the names of the columns
        public static void Import(string fileName)
        {
            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlWorkBook = xlApp.Workbooks.Open(fileName, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
            Excel.Worksheet xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

            Excel.Range range = xlWorkSheet.UsedRange;

            // If there is only one row (the column names)
            if (range.Rows.Count < 2)
                return;

            String QueryString = "insert into recept (name, lang, acc, group_id, subgroup_id, fo_osszetevo_id, prep, desc, com, foodpic) VALUES";
            bool emptyRow = false;
            for (int rCnt = 1; rCnt <= range.Rows.Count && emptyRow == false; rCnt++)
            {
                emptyRow = true;
                string rowSQL = "(";
                for (int cCnt = 1; cCnt <= range.Columns.Count; cCnt++)
                {
                    string cellValue = Convert.ToString((range.Cells[rCnt, cCnt] as Excel.Range).Value2);
                    if (cellValue != null)
                        emptyRow = false;
                    rowSQL += "\"" + cellValue + "\",";
                }
                if (emptyRow == false)
                {
                    rowSQL = rowSQL.Remove(rowSQL.Length - 1);
                    rowSQL += "),";
                    QueryString += rowSQL;
                }
            }
            QueryString = QueryString.Remove(QueryString.Length - 1);
            MessageBox.Show(QueryString);
            xlWorkBook.Close(true, null, null);
            xlApp.Quit();
        }
    }
}