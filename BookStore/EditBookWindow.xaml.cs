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
    /// Interaction logic for EditBookWindow.xaml
    /// </summary>
    public partial class EditBookWindow : Window
    {
        public Book EditedBook { get; set; }
        public EditBookWindow(Book bk)
        {
            InitializeComponent();
            EditedBook = (Book)bk.Clone();
            this.DataContext = EditedBook;

            Business _bus = null;
            string? connectionString = AppConfig.ConnectionString();
            var dao = new SqlDataAccess(connectionString!);
            dao.Connect();
            // Thao tác với CSDL ở đây
            _bus = new Business(dao);

            List<Category> _categories = _bus.ReadAllCategory();
            categoriesComboBox.ItemsSource = _categories;

            
            
        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void categoriesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cat = (Category)categoriesComboBox.SelectedItem;

            MessageBox.Show(cat.ID.ToString());
            EditedBook.Category.ID = cat.ID;
            EditedBook.Category.Name = cat.Name;
            EditedBook.category_id = cat.ID;
        }
    }
}
