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
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class HistoryWindow : Window
    {
        public HistoryWindow()
        {



            InitializeComponent();

            // NYELVESITES

            //btn_BackFromHistoryWindow.Content = LanguageManager.TranslateFromDictionary();
            label_71.Content = LanguageManager.TranslateFromDictionary("71");
            label_72.Content = LanguageManager.TranslateFromDictionary("72");
            label_73.Content = LanguageManager.TranslateFromDictionary("73");
            label_74.Content = LanguageManager.TranslateFromDictionary("74");
            label_75.Content = LanguageManager.TranslateFromDictionary("75");
            label_76.Content = LanguageManager.TranslateFromDictionary("76");
            label_77.Content = LanguageManager.TranslateFromDictionary("77");
            label_78.Content = LanguageManager.TranslateFromDictionary("78");
            label_79.Content = LanguageManager.TranslateFromDictionary("79");
            label_80.Content = LanguageManager.TranslateFromDictionary("80");
            label_81.Content = LanguageManager.TranslateFromDictionary("81");
            label_82.Content = LanguageManager.TranslateFromDictionary("82");
            label_108.Content = LanguageManager.TranslateFromDictionary("108");
            label_109.Content = LanguageManager.TranslateFromDictionary("109");
            label_115.Content = LanguageManager.TranslateFromDictionary("115");
            label_116.Content = LanguageManager.TranslateFromDictionary("116");

        }

        private void btw_BackFromHistoryWindow_Click(object sender, RoutedEventArgs e)
        {
            this.Close();

        }
    }
}
