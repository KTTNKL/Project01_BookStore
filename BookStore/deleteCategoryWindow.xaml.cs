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
    /// Interaction logic for deleteCategoryWindow.xaml
    /// </summary>
    public partial class deleteCategoryWindow : Window
    {
        public List<Category> category;
        public int index = -1;
        public deleteCategoryWindow(List<Category> _categories)
        {
            category = _categories;
            InitializeComponent();
            
        }

        private void okBtn_Click(object sender, RoutedEventArgs e)
        {
            index = deleteCategoriesComboBox.SelectedIndex;
            DialogResult = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            deleteCategoriesComboBox.ItemsSource = category;
        }
    }
}
