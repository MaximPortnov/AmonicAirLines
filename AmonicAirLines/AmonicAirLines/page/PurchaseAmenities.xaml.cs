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

namespace AmonicAirLines.page
{
    /// <summary>
    /// Логика взаимодействия для PurchaseAmenities.xaml
    /// </summary>
    public partial class PurchaseAmenities : Page
    {
        public PurchaseAmenities()
        {
            InitializeComponent();
            //SetupResponseDistribution(new int[] { 583, 504, 1087, 123, 19, 140, 60 });
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // Пример данных
            int[] responses = new int[] { 583, 504, 1087, 123, 19, 140, 60 };
        }
    }
}
