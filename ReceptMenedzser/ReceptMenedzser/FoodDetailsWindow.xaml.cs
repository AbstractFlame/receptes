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
        public FoodDetailsWindow()
        {InitializeComponent();


            // RECEPTEKRE VONATKOZO ADATOK NYELVESITESE
            DataSet dataSet = DBManager.QueryDataSet("SELECT * FROM recept WHERE rid=" + MainWindow.selectedRecipeId);
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
            //image_FoodImageInDetailsWindow.

            // STATIKUS LABELEK NYELVESITESE

            //btn_Back.Content = LanguageManager.TranslateFromDictionary("20");
            label_Group.Content = LanguageManager.TranslateFromDictionary("20");
            label_SubGroup.Content = LanguageManager.TranslateFromDictionary("21");
            label_MainIngredient.Content = LanguageManager.TranslateFromDictionary("22");
            label_Ingredients.Content = LanguageManager.TranslateFromDictionary("23");
            label_Completion.Content = LanguageManager.TranslateFromDictionary("11");
            label_Remark.Content = LanguageManager.TranslateFromDictionary("12");
            label_Preparation.Content = LanguageManager.TranslateFromDictionary("10");
            label_Picture.Content = LanguageManager.TranslateFromDictionary("56");
            label_Language.Content = LanguageManager.TranslateFromDictionary("57");

        }

        private void btn_Back_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
