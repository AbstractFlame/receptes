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
using System.Windows.Forms;
using System.Data;

namespace ReceptMenedzser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string language;

        public MainWindow()
        {
            InitializeComponent();

            DBManager.ConnectToSQLiteDB(@" Data Source=receptek.db; Version=3;");
            UpdateDataGrid("select * from recept");
            GroupManager.FillTreeView(treeView);
        }

        public void UpdateDataGrid(string sql)
        {
            try
            {
                DataSet recipesDataSet = DBManager.QueryData(sql);
                DataView dataView = new DataView(recipesDataSet.Tables[0]);
                dataGrid.ItemsSource = dataView;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("at UpdateDataGrid: " + ex.Message);
            }
        }


        private void Tortenet_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
