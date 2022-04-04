using BookStore.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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

namespace BookStore.MyUserControl
{
    /// <summary>
    /// Interaction logic for MasterDataUserControl.xaml
    /// </summary>
    public partial class MasterDataUserControl : UserControl
    {
        public MasterDataUserControl()
        {
            InitializeComponent();
        }

        BindingList<Book> _list = new BindingList<Book>();
        List<Category> _categories = new List<Category>();


        class ViewModel : INotifyPropertyChanged
        {
            public List<Book> Books { get; set; }
            public List<Book> SelectedBooks { get; set; }

            public event PropertyChangedEventHandler? PropertyChanged;
        }
        ViewModel _vm = new ViewModel();
        int _totalItems = 0;
        int _totalPages = 0;
        int _currentPage = 1;
        int _itemsPerPage = 10;
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Business _bus = null;
            string? connectionString = AppConfig.ConnectionString();
            var dao = new SqlDataAccess(connectionString!);
            if (dao.CanConnect())
            {
                dao.Connect();
                // Thao tác với CSDL ở đây
                _bus = new Business(dao);
                _categories= _bus.ReadAllCategory();
                for(int i=0; i < _categories.Count; i++)
                {
                    _categories[i].Books = _bus.GetBooksByCategoryId(_categories[i].ID);
                    for(int j=0; j < _categories[i].Books.Count; j++)
                    {
                        Debug.WriteLine(_categories[i].Books[j].name);
                        _categories[i].Books[j].Category = _categories[i];
                    }
                }

            }
            categoriesComboBox.ItemsSource = _categories;
            booksListview.ItemsSource = _vm.SelectedBooks;
        }

        private void categoriesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int i = categoriesComboBox.SelectedIndex;
            if (i >= 0)
            {
                _currentPage = 1;

                _vm.Books = _categories[i].Books;
                _vm.SelectedBooks = _vm.Books.Skip((_currentPage - 1) * _itemsPerPage).Take(_itemsPerPage).ToList();
                _totalItems = _vm.Books.Count;
                _totalPages = _vm.Books.Count / _itemsPerPage + (_vm.Books.Count % _itemsPerPage == 0 ? 0 : 1);

                //currentPagingTextBlock.Text = $"{_currentPage}/{_totalPages}";

                booksListview.ItemsSource = _vm.SelectedBooks;

            }
        }

        private void pageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void nextButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage < _totalPages)
            {
                _currentPage++;
                _vm.SelectedBooks = _vm.Books.Skip((_currentPage - 1) * _itemsPerPage).Take(_itemsPerPage).ToList();

                booksListview.ItemsSource = _vm.SelectedBooks;
               // currentPagingTextBlock.Text = $"{_currentPage}/{_totalPages}";
            }
        }

        private void previousButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage > 1)
            {
                _currentPage--;
                _vm.SelectedBooks = _vm.Books.Skip((_currentPage - 1) * _itemsPerPage).Take(_itemsPerPage).ToList();

                booksListview.ItemsSource = _vm.SelectedBooks;
               // currentPagingTextBlock.Text = $"{_currentPage}/{_totalPages}";
            }
        }
    }
}
