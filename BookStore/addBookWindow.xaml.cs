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
    /// Interaction logic for addBookWindow.xaml
    /// </summary>
    public partial class addBookWindow : Window
    {
        public Book AddedBook { get; set; }
        public addBookWindow()
        {
            AddedBook = new Book();
            AddedBook.Category = new Category();
            InitializeComponent();
            this.DataContext = AddedBook;

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
            AddedBook.Category.ID = cat.ID;
            AddedBook.Category.Name = cat.Name;
            AddedBook.category_id = cat.ID;
        }
    }
}
