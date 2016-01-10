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
using System.Diagnostics;
using System.Windows.Controls.Primitives;

namespace ReceptMenedzser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class SearchWindow : Window
    {
        public static int selectedRecipeIndex;
        public static string[] recipeIds;
        public static int recipesLength;

        private DataView dataView;

        public SearchWindow()
        {
            InitializeComponent();

            DBManager.ConnectToSQLiteDB(@" Data Source=receptek.db; Version=3;");
            UpdateDataGrid("select * from recept");
            UpdateFoodPicture(System.AppDomain.CurrentDomain.BaseDirectory + "images\\Pictures\\Cook.jpg");
            FillFilterBar();
            StatikusLabelekButtonokNyelvesitese();
        }
        private void StatikusLabelekButtonokNyelvesitese()
        {
            btn_SearchInFoodName.Content = LanguageManager.TranslateFromDictionary("6");
        }

        private void FormatDataGrid()
        {
            dataGrid.Columns[0].Visibility = Visibility.Hidden;
            dataGrid.Columns[3].Visibility = Visibility.Hidden;
            dataGrid.Columns[5].Visibility = Visibility.Hidden;

            dataGrid.Columns[1].Width = dataGrid.ActualWidth / 3;
            dataGrid.Columns[2].Width = dataGrid.ActualWidth / 3;
            dataGrid.Columns[4].Width = dataGrid.ActualWidth / 3;

            dataGrid.Columns[1].Header = LanguageManager.TranslateFromDictionary("55");
            dataGrid.Columns[2].Header = LanguageManager.TranslateFromDictionary("23");
            dataGrid.Columns[4].Header = LanguageManager.TranslateFromDictionary("11");
        }

        private void FillFilterBar()
        {
            string langColumnName = LanguageManager.GetGroupColumnName();

            // Group select
            DataSet dataSet = DBManager.QueryDataSet("SELECT * FROM T_Group");
            comboB_GroupSelect.IsEditable = true;
            comboB_GroupSelect.IsReadOnly = true;
            comboB_GroupSelect.Text = LanguageManager.TranslateFromDictionary("20");
            comboB_GroupSelect.ItemsSource = dataSet.Tables[0].DefaultView;
            comboB_GroupSelect.DisplayMemberPath = dataSet.Tables[0].Columns[langColumnName].ToString();
            comboB_GroupSelect.SelectedValuePath = dataSet.Tables[0].Columns["CS_ID"].ToString();

            // Subgroup select
            dataSet = DBManager.QueryDataSet("SELECT * FROM T_Subgroup");
            comboB_SubGroupSelect.IsEditable = true;
            comboB_SubGroupSelect.IsReadOnly = true;
            comboB_SubGroupSelect.Text = LanguageManager.TranslateFromDictionary("21");
            comboB_SubGroupSelect.ItemsSource = dataSet.Tables[0].DefaultView;
            comboB_SubGroupSelect.DisplayMemberPath = dataSet.Tables[0].Columns[langColumnName].ToString();
            comboB_SubGroupSelect.SelectedValuePath = dataSet.Tables[0].Columns["SCS_ID"].ToString();

            // Main Ingredient select
            dataSet = DBManager.QueryDataSet("SELECT * FROM T_Ingredient");
            comboB_MainIngredientSelect.IsEditable = true;
            comboB_MainIngredientSelect.IsReadOnly = true;
            comboB_MainIngredientSelect.Text = LanguageManager.TranslateFromDictionary("22");
            comboB_MainIngredientSelect.ItemsSource = dataSet.Tables[0].DefaultView;
            comboB_MainIngredientSelect.DisplayMemberPath = dataSet.Tables[0].Columns[langColumnName].ToString();
            comboB_MainIngredientSelect.SelectedValuePath = dataSet.Tables[0].Columns["SCS_ID"].ToString();
        }

        private void UpdateDataGrid(string sql)
        {
            sql = FinalizeSQLForGrid(sql);
            try
            {
                DataSet recipesDataSet = DBManager.QueryDataSet(sql);
                dataView = new DataView(recipesDataSet.Tables[0]);
                BindingOperations.ClearAllBindings(dataGrid);
                dataGrid.ItemsSource = dataView;
                dataGrid.Items.Refresh();
                FillRecipeIdList(recipesDataSet);
                FormatDataGrid();
            }
            catch (Exception ex)
            {
                // can't resolve this problem: schose a recipe to go to detailWindow, then back, then schose a category
                //System.Windows.MessageBox.Show("at UpdateDataGrid: " + ex.Message);
            }
        }

        private void FillRecipeIdList(DataSet recipesDataSet)
        {
            recipeIds = new string[recipesDataSet.Tables[0].Rows.Count];
            recipesLength = recipesDataSet.Tables[0].Rows.Count;
            int i = 0;
            foreach (DataRow dataRow in recipesDataSet.Tables[0].Rows)
            {
                string id = dataRow[0].ToString();
                recipeIds[i++] = id;
            }
        }

        private void UpdateFoodPicture(string path)
        {
            img_Food.Source = new BitmapImage(new Uri(path));
        }

        private string FinalizeSQLForGrid(string sql)
        {
            string languageFilteredSQL = LanguageManager.FilterSQL(sql);

            string finalSql  = "SELECT ";
            finalSql += "rid, name, acc, prep, desc, com ";
            finalSql += "FROM (" + languageFilteredSQL + ") ORDER BY name ASC";

            return finalSql;
        }

        private void btn_SearchInFoodName_Click(object sender, RoutedEventArgs e)
        {
            string keywords = textB_SearchInFoodName.Text;
            string groupId = comboB_GroupSelect.SelectedValue != null ? comboB_GroupSelect.SelectedValue.ToString() : "";
            string subGroupId = comboB_SubGroupSelect.SelectedValue != null ? comboB_SubGroupSelect.SelectedValue.ToString() : "";
            string ingredientId = comboB_MainIngredientSelect.SelectedValue != null ? comboB_MainIngredientSelect.SelectedValue.ToString() : "";
            string query = "SELECT * FROM recept WHERE";
            query += " name LIKE '%" + keywords + "%'";
            if (groupId != "")
                query += " AND group_id=" + groupId;
            if(subGroupId != "")
                query += " AND subgroup_id=" + subGroupId;
            if(ingredientId != "")
                query += " AND fo_osszetevo_id=" + ingredientId;

            UpdateDataGrid(query);
            UpdateFoodPicture(System.AppDomain.CurrentDomain.BaseDirectory + "images\\Pictures\\Cook.jpg");
        }

        private void dataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            FormatDataGrid();
        }

        private void dataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dataGrid.SelectedIndex == -1)
                return;

            selectedRecipeIndex = dataGrid.SelectedIndex;
            FoodDetailsWindow foodDetailsWindow = new FoodDetailsWindow();
            foodDetailsWindow.Show();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            FormatDataGrid();
        }
    }
}
