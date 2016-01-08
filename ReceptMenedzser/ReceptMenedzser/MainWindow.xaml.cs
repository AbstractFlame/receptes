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
        public static string selectedRecipeId;

        public MainWindow()
        {
            InitializeComponent();

            DBManager.ConnectToSQLiteDB(@" Data Source=receptek.db; Version=3;");
            UpdateDataGrid("select * from recept");
            GroupManager.FillTreeView(treeView);
            FillFilterBar();
        }

        private void FormatDataGrid()
        {
            dataGrid.Columns[0].Width = 100; 
        }

        private void FillFilterBar()
        {
            // Group select
            DataSet dataSet = DBManager.QueryData("SELECT * FROM T_Group");
            comboB_GroupSelect.ItemsSource = dataSet.Tables[0].DefaultView;
            comboB_GroupSelect.DisplayMemberPath = dataSet.Tables[0].Columns["Desc"].ToString();
            comboB_GroupSelect.SelectedValuePath = dataSet.Tables[0].Columns["CS_ID"].ToString();

            // Subgroup select
            dataSet = DBManager.QueryData("SELECT * FROM T_Subgroup");
            comboB_SubGroupSelect.ItemsSource = dataSet.Tables[0].DefaultView;
            comboB_SubGroupSelect.DisplayMemberPath = dataSet.Tables[0].Columns["Desc"].ToString();
            comboB_SubGroupSelect.SelectedValuePath = dataSet.Tables[0].Columns["SCS_ID"].ToString();

            // Subgroup select
            dataSet = DBManager.QueryData("SELECT * FROM T_Ingredient");
            comboB_MainIngredientSelect.ItemsSource = dataSet.Tables[0].DefaultView;
            comboB_MainIngredientSelect.DisplayMemberPath = dataSet.Tables[0].Columns["Desc"].ToString();
            comboB_MainIngredientSelect.SelectedValuePath = dataSet.Tables[0].Columns["SCS_ID"].ToString();
        }

        private void UpdateDataGrid(string sql)
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

        private void btn_SearchInFoodName_Click(object sender, RoutedEventArgs e)
        {
            string keywords = textB_SearchInFoodName.Text;
            string groupId = comboB_GroupSelect.SelectedValue != null ? comboB_GroupSelect.SelectedValue.ToString() : "";
            string subGroupId = comboB_SubGroupSelect.SelectedValue != null ? comboB_SubGroupSelect.SelectedValue.ToString() : "";
            string ingredientId = comboB_MainIngredientSelect.SelectedValue != null ? comboB_MainIngredientSelect.SelectedValue.ToString() : "";
            string query = "SELECT * FROM recept WHERE";
            query += " name '%" + keywords + "'%";
            if (groupId != "")
                query += " AND group_id=" + groupId;
            if(subGroupId != "")
                query += " AND subgroup_id=" + subGroupId;
            if(ingredientId != "")
                query += " AND fo_osszetevo_id=" + ingredientId;

            UpdateDataGrid(query);
        }

        private void treeView_MouseUp(object sender, MouseButtonEventArgs e)
        {
            string query = GroupManager.GetSQLQuery((TreeViewItem)treeView.SelectedItem);
            UpdateDataGrid(query);
        }

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataRowView dataRow = (DataRowView)dataGrid.SelectedItem;
            selectedRecipeId = dataRow.Row.ItemArray[0].ToString();
            System.Windows.MessageBox.Show(selectedRecipeId);
            FoodDetailsWindow foodDetailsWindow = new FoodDetailsWindow();
            foodDetailsWindow.Show();
        }

        private void btn_Excel_Import_Click(object sender, RoutedEventArgs e)
        {
            ExcelImport.Import(System.AppDomain.CurrentDomain.BaseDirectory + "Receptek.xls");
            // TODO make it multilungal!
            System.Windows.MessageBox.Show("Az importálás sikeresen befejeződött!");
        }

        private void dataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            FormatDataGrid();
        }
    }
}
