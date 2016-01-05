using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SQLite;
using System.Data;

namespace ReceptMenedzser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SQLiteConnection sqLite_Con;
        private string default_sql = "select * from recept";

        public MainWindow()
        {

            InitializeComponent();

            //dataGrid.AutoGenerateColumns = true;

            connectToSQLiteDB(@" Data Source=receptek.db; Version=3;");
            UpdateDataGrid(default_sql);
            
        }

        private void connectToSQLiteDB(string DBPath)
        {
            try
            {
                sqLite_Con = new SQLiteConnection(DBPath);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void UpdateDataGrid(string sql)
        {
            try
            {
                sqLite_Con.Open();
                DataSet dataSet = new DataSet();
                SQLiteCommand sql_cmd = sqLite_Con.CreateCommand();
                SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(sql, sqLite_Con);
                dataAdapter.Fill(dataSet);
                DataView dataView = new DataView(dataSet.Tables[0]);
                dataGrid.ItemsSource = dataView;
                sqLite_Con.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Tortenet_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
