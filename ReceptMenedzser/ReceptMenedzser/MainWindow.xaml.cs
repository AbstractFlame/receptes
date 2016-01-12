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
    public partial class MainWindow : Window
    {
        public static int selectedRecipeIndex;
        public static string[] recipeIds;
        public static int recipesLength;

        private DataView dataView;

        public MainWindow()
        {
            InitializeComponent();

            dataGrid.CanUserAddRows = false;
            RefreshLayout();
            StatikusLabelekButtonokNyelvesitese();
        }

        private void RefreshLayout()
        {
            UpdateDataGrid("select * from recept");
            UpdateFoodPicture(System.AppDomain.CurrentDomain.BaseDirectory + "images\\Pictures\\Cook.jpg");
            GroupManager.FillTreeView(treeView);

            DataSet dataSet = DBManager.QueryDataSet("SELECT count(*) FROM recept WHERE lang='" + LanguageManager.GetLangShortName() + "'");
            string Darabszam = dataSet.Tables[0].Rows[0][0].ToString();
            label_Receptjeim_14_count_28.Content = LanguageManager.TranslateFromDictionary("14") + ": "  + Darabszam + " " + LanguageManager.TranslateFromDictionary("28");
        }

        private void StatikusLabelekButtonokNyelvesitese()
        {
            btn_Search.Content = LanguageManager.TranslateFromDictionary("6");
            btn_History.Content = LanguageManager.TranslateFromDictionary("16");

            label_Receptjeim_14_count_28.Content = LanguageManager.TranslateFromDictionary("14");
            label_FelsoSzoveg1_110.Content = LanguageManager.TranslateFromDictionary("110");
            label1_FelsoSZoveg2_112.Content = LanguageManager.TranslateFromDictionary("112");
            label_Revision.Content = LanguageManager.TranslateFromDictionary("83");
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
            string shortLangName = LanguageManager.GetLangShortName();
  
            string finalSql  = "SELECT ";
            finalSql += "rid, name, acc, prep, desc, com ";
            finalSql += "FROM (" + sql + ") WHERE lang='" + shortLangName + "' ORDER BY name ASC";

            return finalSql;
        }

        private void treeView_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (treeView.SelectedItem == null)
                return;

            string query = GroupManager.GetSQLQuery((TreeViewItem)treeView.SelectedItem);
            UpdateDataGrid(query);
            string foodPicPath = GroupManager.getPicturePath((TreeViewItem)treeView.SelectedItem);
            UpdateFoodPicture(foodPicPath);
        }

        private void btn_Excel_Import_Click(object sender, RoutedEventArgs e)
        {
            ExcelImport.Import(System.AppDomain.CurrentDomain.BaseDirectory + "Receptek.xls");
            RefreshLayout();
            System.Windows.MessageBox.Show(LanguageManager.TranslateFromDictionary("128"));
        }

        private void dataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            FormatDataGrid();
        }

        private void btn_History_Click(object sender, RoutedEventArgs e)
        {
            HistoryWindow w = new HistoryWindow();
            w.Show();
        }

        private void btn_Help_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show("Help: Ez a funkció jelenleg nem elérhető.");
        }

        private void btn_SendMail_Click(object sender, RoutedEventArgs e)
        {
            if (System.Windows.MessageBox.Show("Megnyissam a levelezőprogramot, hogy levelet írhass kzoli62@gmail.com-nak?", "",
               MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                var url = "mailto:kzoli62@gmail.com";
                Process.Start(url);
            }
        }

        private void KZK_logo_png_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (System.Windows.MessageBox.Show("Biztosan szeretné törölni a duplikátum recepteket az adatbázisból?", "",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                string sql = "DELETE FROM recept ";
                sql += " WHERE rid NOT IN ( ";
                sql += " SELECT MIN(rid) as rid";
                sql += " FROM recept";
                sql += " GROUP BY name";
                sql += " )";
                int affectedRows = DBManager.QueryCommand(sql);

                // todo
                // System.Windows.MessageBox.Show(LanguageManager.TranslateFromDictionary(""));
                // temporary solution:
                System.Windows.MessageBox.Show("Duplikátumok törölve!");
            }
        }

        private void dataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dataGrid.SelectedIndex == -1)
                return;

            FoodDetailsWindow foodDetailsWindow = new FoodDetailsWindow(recipeIds, dataGrid.SelectedIndex);
            foodDetailsWindow.Show();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            FormatDataGrid();
        }

        private void btn_Search_Click(object sender, RoutedEventArgs e)
        {
            SearchWindow searchWindow = new SearchWindow();
            searchWindow.Show();
        }

        private void btn_Magnifying_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedIndex == -1)
            {
                System.Windows.MessageBox.Show("Nagyító: Nincs recept kiválasztva.");
                return;
            }

            string recipeId = recipeIds[dataGrid.SelectedIndex];
            FullScreenFoodDetails w = new FullScreenFoodDetails(recipeId);
            w.Show();
        }

        private void btn_Back_Click(object sender, RoutedEventArgs e)
        {
            LanguageSelectionWindow languageSelectionWindow = new LanguageSelectionWindow();
            languageSelectionWindow.Show();
            this.Close();
        }

        private void btn_Exit_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
    }
}
