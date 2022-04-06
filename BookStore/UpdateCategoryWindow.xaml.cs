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
    /// Interaction logic for UpdateCategoryWindow.xaml
    /// </summary>
    public partial class UpdateCategoryWindow : Window
    {
        public Category newCat { get; set; }
        public UpdateCategoryWindow(List<Category> _categories)
        {
            InitializeComponent();

            updateCategoriesComboBox.ItemsSource = _categories;
        }

        private void okBtn_Click(object sender, RoutedEventArgs e)
        {
            newCat = (Category)updateCategoriesComboBox.SelectedItem;
            newCat.Name = NewName.Text;            
            DialogResult = true;
        }
    }
}
