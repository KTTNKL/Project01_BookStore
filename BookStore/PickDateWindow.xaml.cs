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
    /// Interaction logic for PickDateWindow.xaml
    /// </summary>
    public partial class PickDateWindow : Window
    {
        public string PickedDate{get; set;}
        public PickDateWindow()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {

            if(Calendar.SelectedDate.HasValue)
            {
                PickedDate = Calendar.SelectedDate.Value.ToString("dd/MM/yyyy");
            }
            else
            {
                PickedDate = "N/A";
            }
            DialogResult = true;
        }
    }
}
