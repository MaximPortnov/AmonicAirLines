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

namespace AmonicAirLines
{
    /// <summary>
    /// Логика взаимодействия для BillingConfirmation.xaml
    /// </summary>
    public partial class BillingConfirmation : Window
    {
        public decimal totalAmount;
        public bool paymentCompleted = false;
        public BillingConfirmation()
        {
            InitializeComponent();
        }

        public void init()
        {
            tb_TotalAmount.Text = totalAmount.ToString();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            if (rb_Voucher.IsChecked == true)
            {
                MessageBox.Show("оплата ваучером");
            }
            if (rb_Cash.IsChecked == true)
            {
                MessageBox.Show("оплата наличкой");
            }
            if (rb_CreditCard.IsChecked == true)
            {
                MessageBox.Show("оплата кредиткой");
            }
            paymentCompleted = true;
            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            paymentCompleted = false;
            Close();
        }
    }
}
