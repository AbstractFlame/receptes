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
    /// Interaction logic for LanguageSelectionWindow.xaml
    /// </summary>
    public partial class LanguageSelectionWindow : Window
    {
        public LanguageSelectionWindow()
        {
            InitializeComponent();
            
            comboBox.Items.Add("Magyar");
            comboBox.Items.Add("English");
            comboBox.Items.Add("Deutsch");
            comboBox.Items.Add("Slovensky");
            comboBox.SelectedIndex = 0;
            //label_Revision.Content = LanguageManager.TranslateFromDictionary("83");
        }

        private void btn_Quit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btn_Ok_Click(object sender, RoutedEventArgs e)
        {
            switch(comboBox.SelectedItem.ToString())
            {
                case "Magyar":
                    LanguageManager.currentLang = LanguageManager.Lang.HUNGARIAN;
                    break;
                case "English":
                    LanguageManager.currentLang = LanguageManager.Lang.ENGLISH;
                    break;
                case "Deutsch":
                    LanguageManager.currentLang = LanguageManager.Lang.GERMAN;
                    break;
                case "Slovensky":
                    LanguageManager.currentLang = LanguageManager.Lang.SLOVENSKY;
                    break;
            }
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
