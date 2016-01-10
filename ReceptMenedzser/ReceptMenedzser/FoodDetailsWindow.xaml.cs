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
using System.Windows.Shapes;
using System.Data;

namespace ReceptMenedzser
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class FoodDetailsWindow : Window
    {
        private int currentRecipeIndex;
        private bool underModification;

        public FoodDetailsWindow()
        {
            InitializeComponent();

            currentRecipeIndex = MainWindow.selectedRecipeIndex;
            underModification = false;
            btn_cancelModificationRecipe.Visibility = Visibility.Hidden;

            UpdateRecipeData(currentRecipeIndex);
            RefreshLayout();
            FillLabels();
        }

        private void UpdateRecipeData(int recipeIndex)
        {
            if (recipeIndex >= MainWindow.recipesLength)
            {
                MessageBox.Show("Nincs ilyen recept.");
                return;
            }

            // RECEPTEKRE VONATKOZO ADATOK NYELVESITESE
            string recipeId = MainWindow.recipeIds[recipeIndex];
            DataSet dataSet = DBManager.QueryDataSet("SELECT * FROM recept WHERE rid=" + recipeId);
            DataRow receptDataRow = dataSet.Tables[0].Rows[0];

            textBlock_FoodFUllName.Text = receptDataRow["name"].ToString();
            textBox_Ingredients.Text = receptDataRow["acc"].ToString();
            textBox_Completion.Text = receptDataRow["desc"].ToString();
            textBox_Remark.Text = receptDataRow["com"].ToString();
            textBox_Preparation.Text = receptDataRow["prep"].ToString();
            textBox_Picture.Text = receptDataRow["foodpic"].ToString();
            textBox_Language.Text = receptDataRow["lang"].ToString();
            if (receptDataRow["foodpic"] != null)
            {
                try
                {
                    image_FoodImageInDetailsWindow.Source = new BitmapImage(new Uri(System.AppDomain.CurrentDomain.BaseDirectory + "images\\Food_pictures\\" + receptDataRow["foodpic"].ToString()));
                }
                catch(Exception ex)
                {
                    //MessageBox.Show("A képfájl nem található: " + ex.Message);
                    image_FoodImageInDetailsWindow.Source = new BitmapImage(new Uri(System.AppDomain.CurrentDomain.BaseDirectory + "images\\Pictures\\Cook.jpg"));
                }
            }

            string groupId = receptDataRow["group_id"].ToString();
            string subGroupId = receptDataRow["subgroup_id"].ToString();
            string mainIngredientId = receptDataRow["fo_osszetevo_id"].ToString();
            FillComboBoxes(groupId, subGroupId, mainIngredientId);
        }

        private void RefreshLayout()
        {
            textBlock_FoodFUllName.IsEnabled = underModification;
            comboB_GroupSelect.IsEnabled = underModification;
            comboB_SubGroupSelect.IsEnabled = underModification;
            comboB_MainIngredientSelect.IsEnabled = underModification;
            textBox_Ingredients.IsEnabled = underModification;
            textBox_Completion.IsEnabled = underModification;
            textBox_Remark.IsEnabled = underModification;
            textBox_Preparation.IsEnabled = underModification;
            textBox_Picture.IsEnabled = underModification;
            textBox_Language.IsEnabled = underModification;

            btn_cancelModificationRecipe.Visibility = underModification ? Visibility.Visible : Visibility.Hidden;
            btn_SaveModify.Visibility = underModification ? Visibility.Visible : Visibility.Hidden;
        }

        private void FillLabels()
        {
            label_Group.Content = LanguageManager.TranslateFromDictionary("20");
            label_SubGroup.Content = LanguageManager.TranslateFromDictionary("21");
            label_MainIngredient.Content = LanguageManager.TranslateFromDictionary("22");
            label_Ingredients.Content = LanguageManager.TranslateFromDictionary("23");
            label_Completion.Content = LanguageManager.TranslateFromDictionary("11");
            label_Remark.Content = LanguageManager.TranslateFromDictionary("12");
            label_Preparation.Content = LanguageManager.TranslateFromDictionary("10");
            label_Picture.Content = LanguageManager.TranslateFromDictionary("56");
            label_Language.Content = LanguageManager.TranslateFromDictionary("57");
            label_Revision.Content = LanguageManager.TranslateFromDictionary("83");

            btn_modifyRecipe.Content = LanguageManager.TranslateFromDictionary("125");
        }

        private void btn_Back_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btn_modifyRecipe_Click(object sender, RoutedEventArgs e)
        {
            underModification = true;
            RefreshLayout();
        }

        private int getComboBoxIndexById(ComboBox comboBox, string Id)
        {
            for(int i = 0; i < comboBox.Items.Count; ++i)
            {
                var v = comboBox.Items.GetItemAt(i);
                DataRowView dv = (DataRowView)v;
                string value = dv[0].ToString();
                if (value == Id)
                    return i;
            }

            return -1;
        }

        private void FillComboBoxes(string groupId, string subGroupId, string mainIngredientId)
        {
            string langColumnName = LanguageManager.GetGroupColumnName();

            // Group select
            DataSet dataSet = DBManager.QueryDataSet("SELECT * FROM T_Group");
            comboB_GroupSelect.ItemsSource = dataSet.Tables[0].DefaultView;
            comboB_GroupSelect.DisplayMemberPath = dataSet.Tables[0].Columns[langColumnName].ToString();
            comboB_GroupSelect.SelectedValuePath = dataSet.Tables[0].Columns["CS_ID"].ToString();
            comboB_GroupSelect.SelectedIndex = getComboBoxIndexById(comboB_GroupSelect, groupId);

            // Subgroup select
            dataSet = DBManager.QueryDataSet("SELECT * FROM T_Subgroup");
            comboB_SubGroupSelect.ItemsSource = dataSet.Tables[0].DefaultView;
            comboB_SubGroupSelect.DisplayMemberPath = dataSet.Tables[0].Columns[langColumnName].ToString();
            comboB_SubGroupSelect.SelectedValuePath = dataSet.Tables[0].Columns["SCS_ID"].ToString();
            comboB_SubGroupSelect.SelectedIndex = getComboBoxIndexById(comboB_SubGroupSelect, subGroupId);

            // Subgroup select
            dataSet = DBManager.QueryDataSet("SELECT * FROM T_Ingredient");
            comboB_MainIngredientSelect.ItemsSource = dataSet.Tables[0].DefaultView;
            comboB_MainIngredientSelect.DisplayMemberPath = dataSet.Tables[0].Columns[langColumnName].ToString();
            comboB_MainIngredientSelect.SelectedValuePath = dataSet.Tables[0].Columns["SCS_ID"].ToString();
            comboB_MainIngredientSelect.SelectedIndex = getComboBoxIndexById(comboB_MainIngredientSelect, mainIngredientId);
        }

        private void btn_previousRecipe_Click(object sender, RoutedEventArgs e)
        {
            if (underModification)
            {
                MessageBox.Show("Szerkesztés alatt nem lehet lépni.");
                return;
            }

            if (currentRecipeIndex < 1)
            {
                MessageBox.Show("Nincs megelőző elem.");
                return;
            }

            UpdateRecipeData(--currentRecipeIndex);
        }

        private void btn_nextRecipe_Click(object sender, RoutedEventArgs e)
        {
            if (underModification)
            {
                MessageBox.Show("Szerkesztés alatt nem lehet lépni.");
                return;
            }

            if (currentRecipeIndex > MainWindow.recipesLength - 2)
            {
                MessageBox.Show("Nincs következő elem.");
                return;
            }

            UpdateRecipeData(++currentRecipeIndex);
        }

        private void btn_firstRecipe_Click(object sender, RoutedEventArgs e)
        {
            if (underModification)
            {
                MessageBox.Show("Szerkesztés alatt nem lehet lépni.");
                return;
            }

            UpdateRecipeData(currentRecipeIndex = 0);
        }

        private void btn_lastRecipe_Click(object sender, RoutedEventArgs e)
        {
            if (underModification)
            {
                MessageBox.Show("Szerkesztés alatt nem lehet lépni.");
                return;
            }

            UpdateRecipeData(currentRecipeIndex = MainWindow.recipesLength - 1);
        }

        private void btn_cancelModificationRecipe_Click(object sender, RoutedEventArgs e)
        {
            underModification = false;
            btn_cancelModificationRecipe.Visibility = Visibility.Hidden;
            UpdateRecipeData(currentRecipeIndex);
            RefreshLayout();
        }

        private void btn_SaveModify_Click(object sender, RoutedEventArgs e)
        {
            underModification = false;
            RefreshLayout();

            string sql = "UPDATE recept SET ";
            sql += "name='" + textBlock_FoodFUllName.Text + "',";
            sql += "lang='" + textBox_Language.Text + "',";
            sql += "acc='" + textBox_Ingredients.Text + "',";
            sql += "group_id='" + comboB_GroupSelect.SelectedValue + "',";
            sql += "subgroup_id='" + comboB_SubGroupSelect.SelectedValue + "',";
            sql += "fo_osszetevo_id='" + comboB_MainIngredientSelect.SelectedValue + "',";
            sql += "prep='" + textBox_Preparation.Text + "',";
            sql += "desc='" + textBox_Completion.Text + "',";
            sql += "com='" + textBox_Remark.Text + "',";
            sql += "foodpic='" + textBox_Picture.Text + "'";
            sql += " WHERE rid = '" + MainWindow.recipeIds[currentRecipeIndex] + "'";
            DBManager.QueryCommand(sql);
        }
    }
}
