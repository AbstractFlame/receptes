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

            label_FoodFullName.Content = receptDataRow["name"].ToString();
            textBox_Group.Text = LanguageManager.TranslateGroup(receptDataRow["group_id"].ToString());
            textBox_SubGroup.Text = LanguageManager.TranslateSubGroup(receptDataRow["subgroup_id"].ToString());
            textBox_MainIngredient.Text = LanguageManager.TranslateIngredient(receptDataRow["fo_osszetevo_id"].ToString());
            textBox_Ingredients.Text = receptDataRow["acc"].ToString();
            textBox_Completion.Text = receptDataRow["desc"].ToString();
            textBox_Remark.Text = receptDataRow["com"].ToString();
            textBox_Preparation.Text = receptDataRow["prep"].ToString();
            textBox_Picture.Text = receptDataRow["foodpic"].ToString();
            textBox_Language.Text = LanguageManager.TranslateGroup(receptDataRow["lang"].ToString());

            // STATIKUS LABELEK NYELVESITESE

            //btn_Back.Content = LanguageManager.TranslateFromDictionary("20");
            label_Group.Content = LanguageManager.TranslateGroup("20");
            label_SubGroup.Content = LanguageManager.TranslateGroup("21");
            label_MainIngredient.Content = LanguageManager.TranslateGroup("22");
            label_Ingredients.Content = LanguageManager.TranslateGroup("23");
            label_Completion.Content = LanguageManager.TranslateGroup("11");
            label_Remark.Content = LanguageManager.TranslateGroup("12");
            label_Preparation.Content = LanguageManager.TranslateGroup("10");
            label_Picture.Content = LanguageManager.TranslateGroup("56");
            label_Language.Content = LanguageManager.TranslateGroup("57");

        }

        private void btn_Back_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
