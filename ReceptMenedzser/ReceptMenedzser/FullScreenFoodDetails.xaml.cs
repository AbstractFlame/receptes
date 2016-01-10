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

namespace ReceptMenedzser
{
    /// <summary>
    /// Interaction logic for FullScreenFoodDetails.xaml
    /// </summary>
    public partial class FullScreenFoodDetails : Window
    {
        public FullScreenFoodDetails()
        {
            InitializeComponent();
            label_Revision.Content = LanguageManager.TranslateFromDictionary("83");

            btn_QuitFromFUllscreenFoodDetailsWIndow.Content = LanguageManager.TranslateFromDictionary("120");
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

        }

        private void btn_QuitFromFUllscreenFoodDetailsWIndow_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
