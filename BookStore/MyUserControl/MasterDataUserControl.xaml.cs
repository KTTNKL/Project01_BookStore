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
        public enum MasterDataAction
        {
            AddNewCategory,               // Thêm mới một Loại sản phẩm
            DeleteSelectedCategory,   // Xóa Loại sản phẩm đang được chọn
            AddNewProduct,		  // Thêm mới một Sản phẩm
            UpdateSelectedProduct,   // Cập nhật Sản phẩm đang được chọn
            DeleteSelectedProduct     // Xóa Sản phẩm đang được chọn
        };
        public void HandleParentEvent(MasterDataAction action)
        {
            switch (action)
            {
                case MasterDataAction.AddNewCategory:
                    addNewCategory();
                    break;
            }
        }

        private void addNewCategory()
        {
            MessageBox.Show("Helo");
        }

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
            public List<Page> Pages { get; set; }
        }
        class Page
        {
            public int currentPage { get; set; }
            public int totalPage { get; set; }
        }
        ViewModel _vm = new ViewModel();
        int _totalItems = 0;
        int _totalPages = 0;
        int _currentPage = 1;
        int _itemsPerPage = 10;
        int currentCat = -1;
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            categoriesComboBox.SelectedIndex = 0;

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
                        _categories[i].Books[j].Category = _categories[i];
                    }
                }

            }

            categoriesComboBox.ItemsSource = _categories;
            booksListview.ItemsSource = _vm.SelectedBooks;
            pageNumberComboBox.SelectedIndex = 1;

            numberOfBookTextBlock.Text= "Số lượng sách :"+_bus.countBook().ToString();
        }

        private void categoriesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int i = categoriesComboBox.SelectedIndex;
            if (i >= 0)
            {
                currentCat = i;
                _currentPage = 1;

                _vm.Books = _categories[i].Books;
                _vm.SelectedBooks = _vm.Books.Skip((_currentPage - 1) * _itemsPerPage).Take(_itemsPerPage).ToList();
                _totalItems = _vm.Books.Count;
                _totalPages = _vm.Books.Count / _itemsPerPage + (_vm.Books.Count % _itemsPerPage == 0 ? 0 : 1);


                booksListview.ItemsSource = _vm.SelectedBooks;
                _vm.Pages = new List<Page>();
                for (int j = 0; j < _totalPages; j++)
                {
                    _vm.Pages.Add(new Page(){ currentPage = j+1, totalPage=_totalPages });
                }
                pagingComboBox.ItemsSource= _vm.Pages;
            }
        }

        private void pageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int i = pagingComboBox.SelectedIndex;
            _currentPage=i+1;
            _vm.SelectedBooks = _vm.Books.Skip((_currentPage - 1) * _itemsPerPage).Take(_itemsPerPage).ToList();

            booksListview.ItemsSource = _vm.SelectedBooks;
        }

        private void nextButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage < _totalPages)
            {
                _currentPage++;

                pagingComboBox.SelectedIndex = _currentPage-1;

                _vm.SelectedBooks = _vm.Books.Skip((_currentPage - 1) * _itemsPerPage).Take(_itemsPerPage).ToList();

                booksListview.ItemsSource = _vm.SelectedBooks;
            }
        }

        private void previousButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage > 1)
            {
                _currentPage--;
                pagingComboBox.SelectedIndex = _currentPage-1;

                _vm.SelectedBooks = _vm.Books.Skip((_currentPage - 1) * _itemsPerPage).Take(_itemsPerPage).ToList();

                booksListview.ItemsSource = _vm.SelectedBooks;
            }
        }

        private void pageNumber_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int i = pageNumberComboBox.SelectedIndex;
            if (i == 0)
            {
                _itemsPerPage = 5;
            }
            else {
                _itemsPerPage = 10;
            }
            _currentPage = 1;
            if (categoriesComboBox.SelectedIndex < 0)
            {
                categoriesComboBox.SelectedIndex = 0;
            }
            _vm.Books = _categories[categoriesComboBox.SelectedIndex].Books;
            _vm.SelectedBooks = _vm.Books.Skip((_currentPage - 1) * _itemsPerPage).Take(_itemsPerPage).ToList();
            _totalItems = _vm.Books.Count;
            _totalPages = _vm.Books.Count / _itemsPerPage + (_vm.Books.Count % _itemsPerPage == 0 ? 0 : 1);

            booksListview.ItemsSource = _vm.SelectedBooks;
            _vm.Pages = new List<Page>();
            for (int j = 0; j < _totalPages; j++)
            {
                _vm.Pages.Add(new Page() { currentPage = j + 1, totalPage = _totalPages });
            }
            pagingComboBox.ItemsSource = _vm.Pages;

        }


        private void Deleted_Click(object sender, RoutedEventArgs e)
        {
            var book = booksListview.SelectedItem as Book;
            MessageBox.Show(book.name);
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Detail_Click(object sender, RoutedEventArgs e)
        {

        }
        
        private void Search_Click(object sender, RoutedEventArgs e)
        {
            var name= SearchTextBox.Text.Trim();
            Business _bus = null;
            string? connectionString = AppConfig.ConnectionString();
            var dao = new SqlDataAccess(connectionString!);
            var sum = 0;
            booksListview.ItemsSource = null;
            if (dao.CanConnect())
            {
                dao.Connect();
                // Thao tác với CSDL ở đây
                _bus = new Business(dao);
                _categories = _bus.ReadAllCategory();
                for (int i = 0; i < _categories.Count; i++)
                {
                    _categories[i].Books = _bus.ReadAllBookLikeName(name,_categories[i].ID);
                    sum += _categories[i].Books.Count;
                    for (int j = 0; j < _categories[i].Books.Count; j++)
                    {
                        _categories[i].Books[j].Category = _categories[i];
                    }
                }

            }
            categoriesComboBox.SelectedIndex =  currentCat;
            int cat = currentCat;
            if (cat >= 0)
            {
                _currentPage = 1;
                _vm.Books = _categories[cat].Books;
                _vm.SelectedBooks = _vm.Books.Skip((_currentPage - 1) * _itemsPerPage).Take(_itemsPerPage).ToList();
                _totalItems = _vm.Books.Count;
                _totalPages = _vm.Books.Count / _itemsPerPage + (_vm.Books.Count % _itemsPerPage == 0 ? 0 : 1);
                booksListview.ItemsSource = _vm.SelectedBooks;
                _vm.Pages = new List<Page>();
                for (int j = 0; j < _totalPages; j++)
                {
                    _vm.Pages.Add(new Page() { currentPage = j + 1, totalPage = _totalPages });
                }
                pagingComboBox.ItemsSource = _vm.Pages;
            }
            numberOfBookTextBlock.Text = "Số lượng sách :" +sum.ToString();

        }

        private void SearchPrice_Click(object sender, RoutedEventArgs e)
        {
            var low = Int32.Parse(lowPrice.Text);
            var high= Int32.Parse(highPrice.Text);
            Business _bus = null;
            string? connectionString = AppConfig.ConnectionString();
            var dao = new SqlDataAccess(connectionString!);
            var sum = 0;
            booksListview.ItemsSource = null;
            if (dao.CanConnect())
            {
                dao.Connect();
                // Thao tác với CSDL ở đây
                _bus = new Business(dao);
                _categories = _bus.ReadAllCategory();
                for (int i = 0; i < _categories.Count; i++)
                {
                    _categories[i].Books = _bus.ReadAllBookPrice(low,high, _categories[i].ID);
                    sum += _categories[i].Books.Count;
                    for (int j = 0; j < _categories[i].Books.Count; j++)
                    {
                        _categories[i].Books[j].Category = _categories[i];
                    }
                }

            }
            categoriesComboBox.SelectedIndex = currentCat;
            int cat = currentCat;
            if (cat >= 0)
            {
                _currentPage = 1;
                _vm.Books = _categories[cat].Books;
                _vm.SelectedBooks = _vm.Books.Skip((_currentPage - 1) * _itemsPerPage).Take(_itemsPerPage).ToList();
                _totalItems = _vm.Books.Count;
                _totalPages = _vm.Books.Count / _itemsPerPage + (_vm.Books.Count % _itemsPerPage == 0 ? 0 : 1);
                booksListview.ItemsSource = _vm.SelectedBooks;
                _vm.Pages = new List<Page>();
                for (int j = 0; j < _totalPages; j++)
                {
                    _vm.Pages.Add(new Page() { currentPage = j + 1, totalPage = _totalPages });
                }
                pagingComboBox.ItemsSource = _vm.Pages;
            }
            numberOfBookTextBlock.Text = "Số lượng sách :" + sum.ToString();

        }
    }
}
