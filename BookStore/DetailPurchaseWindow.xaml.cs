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
        public DetailPurchaseWindow(List<PurchaseDetail> list)
        {
            //detailComboBox.ItemsSource = list;
            InitializeComponent();
        }
    }
}
