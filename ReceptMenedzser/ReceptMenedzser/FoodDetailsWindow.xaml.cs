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

            UpdateRecipeData(currentRecipeIndex);
            FillLabels();
        }

        private void UpdateRecipeData(int recipeIndex)
        {
            // RECEPTEKRE VONATKOZO ADATOK NYELVESITESE
            string recipeId = MainWindow.recipeIds[recipeIndex];
            DataSet dataSet = DBManager.QueryDataSet("SELECT * FROM recept WHERE rid=" + recipeId);
            DataRow receptDataRow = dataSet.Tables[0].Rows[0];

            textBlock_FoodFUllName.Text = receptDataRow["name"].ToString();
            textBox_Group.Text = LanguageManager.TranslateGroup(receptDataRow["group_id"].ToString());
            textBox_SubGroup.Text = LanguageManager.TranslateSubGroup(receptDataRow["subgroup_id"].ToString());
            textBox_MainIngredient.Text = LanguageManager.TranslateIngredient(receptDataRow["fo_osszetevo_id"].ToString());
            textBox_Ingredients.Text = receptDataRow["acc"].ToString();
            textBox_Completion.Text = receptDataRow["desc"].ToString();
            textBox_Remark.Text = receptDataRow["com"].ToString();
            textBox_Preparation.Text = receptDataRow["prep"].ToString();
            textBox_Picture.Text = receptDataRow["foodpic"].ToString();
            textBox_Language.Text = receptDataRow["lang"].ToString();
            image_FoodImageInDetailsWindow.Source = new BitmapImage(new Uri(System.AppDomain.CurrentDomain.BaseDirectory + "images\\Food_pictures\\" + receptDataRow["foodpic"].ToString()));
        }

        private void FillLabels()
        {
            btn_closeRecipeDetailWindow.Content = LanguageManager.TranslateFromDictionary("120");
            label_Group.Content = LanguageManager.TranslateFromDictionary("20");
            label_SubGroup.Content = LanguageManager.TranslateFromDictionary("21");
            label_MainIngredient.Content = LanguageManager.TranslateFromDictionary("22");
            label_Ingredients.Content = LanguageManager.TranslateFromDictionary("23");
            label_Completion.Content = LanguageManager.TranslateFromDictionary("11");
            label_Remark.Content = LanguageManager.TranslateFromDictionary("12");
            label_Preparation.Content = LanguageManager.TranslateFromDictionary("10");
            label_Picture.Content = LanguageManager.TranslateFromDictionary("56");
            label_Language.Content = LanguageManager.TranslateFromDictionary("57");

            btn_firstRecipe.Content = LanguageManager.TranslateFromDictionary("121");
            btn_previousRecipe.Content = LanguageManager.TranslateFromDictionary("122");
            btn_nextRecipe.Content = LanguageManager.TranslateFromDictionary("123");
            btn_lastRecipe.Content = LanguageManager.TranslateFromDictionary("124");
            btn_modifyRecipe.Content = LanguageManager.TranslateFromDictionary("125");

        }
         private void btn_Back_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btn_modifyRecipe_Click(object sender, RoutedEventArgs e)
        {
            underModification = !underModification;
           // if (underModification)

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
    }
}
