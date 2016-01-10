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
        }
    }
}
