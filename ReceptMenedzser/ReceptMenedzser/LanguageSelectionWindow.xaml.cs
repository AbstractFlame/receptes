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
        public static string lang;

        public LanguageSelectionWindow()
        {
            InitializeComponent();

            comboBox.Items.Add("Magyar");
            comboBox.Items.Add("English");
            comboBox.Items.Add("Deutsch");
            comboBox.Items.Add("Slovensky");
            comboBox.SelectedIndex = 0;
        }

        private void btn_Quit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btn_Ok_Click(object sender, RoutedEventArgs e)
        {
            lang = comboBox.SelectedItem.ToString();
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
