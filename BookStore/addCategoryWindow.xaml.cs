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
using BookStore.Database;

namespace BookStore
{
    /// <summary>
    /// Interaction logic for addCategoryWindow.xaml
    /// </summary>
    public partial class addCategoryWindow : Window
    {

        public Category newCat { get; set; }
        public addCategoryWindow()
        {
            InitializeComponent();
            newCat = new Category();
            this.DataContext = newCat;
        }

        private void okBtn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
