using BookStore.Database;
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

namespace BookStore
{
    /// <summary>
    /// Interaction logic for DetailPurchaseWindow.xaml
    /// </summary>
    public partial class DetailPurchaseWindow : Window
    {
        public int statusOrder = -1;
        public DetailPurchaseWindow(List<PurchaseDetail> list, string name, string phone, string addr, int total, string status)
        {
            
            InitializeComponent();
            detailComboBox.ItemsSource = list;
            CustomerName.Content = name;
            CustomerPhone.Content = phone;
            CustomerAddress.Content = addr;
            Total.Content = total.ToString();

            if (status == "shipping")
            {
                statusComboBox.SelectedIndex = 0;
                statusOrder = 0;
            }
            else if (status == "shipped")
            {
                statusComboBox.SelectedIndex = 1;
                statusOrder = 1;
            }
            else
            {
                statusComboBox.SelectedIndex = 2;
                statusOrder = 2;
            }



        }

        private void statusComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            statusOrder = statusComboBox.SelectedIndex;
        }

        private void okBtn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
