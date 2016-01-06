using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data;

namespace ReceptMenedzser
{
    public class DBManager
    {
        private static SQLiteConnection sqLite_Con;
        private static bool isConnected = false;

        public static void ConnectToSQLiteDB(string DBPath)
        {
            try
            {
                sqLite_Con = new SQLiteConnection(DBPath);
                isConnected = true;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        public static DataSet QueryData(string sql)
        {
            if(!isConnected)
                throw new Exception("Not connected");

            try
            {
                sqLite_Con.Open();
                DataSet dataSet = new DataSet();
                SQLiteCommand sql_cmd = sqLite_Con.CreateCommand();
                SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(sql, sqLite_Con);
                dataAdapter.Fill(dataSet);
                return dataSet;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("at QueryData: " + ex.Message);
                return null;
            }
            finally
            {
                sqLite_Con.Close();
            }
        }
    }
}
