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
using System.Windows.Forms;

namespace ReceptMenedzser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SQLiteConnection sqLite_Con;
        DataSet recipesDataSet;
        private string default_sql = "select * from recept";

        public MainWindow()
        {

            InitializeComponent();

            //dataGrid.AutoGenerateColumns = true;
            recipesDataSet = new DataSet();

            ConnectToSQLiteDB(@" Data Source=receptek.db; Version=3;");
            UpdateDataGrid(default_sql);
            FillTreeView();

        }

        private void ConnectToSQLiteDB(string DBPath)
        {
            try
            {
                sqLite_Con = new SQLiteConnection(DBPath);
            }
            catch(Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        private void FillTreeView()
        {
            //treeView.Items.Clear(); // TODO: temporarly clear...

            //DataSet cartonDataSet = QueryData("SELECT * FROM T_Karton");
            //foreach (DataRow dr in cartonDataSet.Tables[0].Rows)
            //{
            //    string kartonName = dr[2].ToString();
            //    string SQLSubGroupQuery = dr[6].ToString();
            //    //TreeItem treeItem = new TreeItem(kartonName, SQLSubGroupQuery);
            //    TreeNode level1Node = new TreeNode
            //    treeView;
            //}
        }

        private DataSet QueryData(string sql)
        {
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
        }

        private void UpdateDataGrid(string sql)
        {
            try
            {
                recipesDataSet = QueryData(sql);
                DataView dataView = new DataView(recipesDataSet.Tables[0]);
                dataGrid.ItemsSource = dataView;
            }
            catch(Exception ex)
            {
                System.Windows.MessageBox.Show("at UpdateDataGrid: " + ex.Message);
            }
        }

        //public class TreeItem
        //{
        //    public string Name;
        //    public string SQLSubGroupQuery;
        //    public TreeItem(string Name, string SQLSubGroupQuery)
        //    {
        //        this.Name = Name;
        //        this.SQLSubGroupQuery = SQLSubGroupQuery;
        //    }
        //}

        private void Tortenet_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
