using Aspose.Cells;
using BookStore.Database;
using Microsoft.Win32;
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

        BindingList<Book> _list = new BindingList<Book>();
        List<Category> _categories = new List<Category>();
   

        public MasterDataUserControl()
        {
            InitializeComponent();
        }

        

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
            try
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
                    _categories = _bus.ReadAllCategory();
                    for (int i = 0; i < _categories.Count; i++)
                    {
                        _categories[i].Books = _bus.GetBooksByCategoryId(_categories[i].ID);
                        for (int j = 0; j < _categories[i].Books.Count; j++)
                        {
                            _categories[i].Books[j].Category = _categories[i];
                        }
                    }
                }

                this.categoriesComboBox.ItemsSource = _categories;

                booksListview.ItemsSource = _vm.SelectedBooks;
                pageNumberComboBox.SelectedIndex = 1;
                if (_bus != null) { 
                numberOfBookTextBlock.Text = "Số lượng sách :" + _bus.countBook().ToString();
                }
            }catch (Exception ex)
            {
                MessageBox.Show("Cannot connect to database");
            }
        }

        private void categoriesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Debug.WriteLine("InitializeCate");

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
            try
            {
                int i = pageNumberComboBox.SelectedIndex;
                if (i == 0)
                {
                    _itemsPerPage = 5;
                }
                else
                {
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
            } catch (Exception ex)
            {
                MessageBox.Show("Cannot connect to database");
            }
        }


        private void Deleted_Click(object sender, RoutedEventArgs e)
        {
            var book = booksListview.SelectedItem as Book;

            var result = MessageBox.Show($"Bạn thật sự muốn xóa sách {book.name} - {book.author} - {book.publicYear}",
                "Xác nhận xóa", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (MessageBoxResult.Yes == result)
            {
                if (categoriesComboBox.SelectedIndex < 0)
                {
                    categoriesComboBox.SelectedIndex = 0;
                }
                // Xóa sách ở các list
                for (int i = 0; i < _categories[categoriesComboBox.SelectedIndex].Books.Count; ++i)
                {
                    if(_categories[categoriesComboBox.SelectedIndex].Books[i].id == book.id)
                    {
                        _categories[categoriesComboBox.SelectedIndex].Books.RemoveAt(i);
                    }
                }
                for (int i = 0; i < _vm.Books.Count; ++i)
                {
                    if (_vm.Books[i].id == book.id)
                    {
                        _vm.Books.RemoveAt(i);
                    }
                }

                Business _bus = null;
                string? connectionString = AppConfig.ConnectionString();
                var dao = new SqlDataAccess(connectionString!);
                dao.Connect();
                // Thao tác với CSDL ở đây
                _bus = new Business(dao);
                _bus.DeleteBookById(book.id);

                MessageBox.Show("Xóa sách thành công", " ", MessageBoxButton.OK, MessageBoxImage.Information);
                numberOfBookTextBlock.Text = "Số lượng sách :" + _bus.countBook().ToString();

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

            }
            else
            {
                // Do nothing
            }

            
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            var book = booksListview.SelectedItem as Book;

            var old_cat = book.category_id;
            var screen = new EditBookWindow(book);
            Business _bus = null;
            string? connectionString = AppConfig.ConnectionString();
            var dao = new SqlDataAccess(connectionString!);
            dao.Connect();
            // Thao tác với CSDL ở đây
            _bus = new Business(dao);
            Book info = null;
            if (screen.ShowDialog() == true)
            {
                info = screen.EditedBook;
                book = info;
                
                _bus.UpdateBook(book.id, book.name, book.author, book.publicYear, book.bookCover, book.purchasePrice, book.sellingPrice, book.stockNumer, book.sellingNumber, book.category_id);
                MessageBox.Show("Cập nhật sách sách thành công", " ", MessageBoxButton.OK, MessageBoxImage.Information);

                Console.WriteLine("Trước khi xóa");
                for (int i = 0; i < _categories.Count; ++i)
                {
                    if (_categories[i].ID == old_cat)
                    {
                        for (int j = 0; j < _categories[i].Books.Count; ++j)
                        {
                            Console.WriteLine(_categories[i].Books[j].name);
                        }
                    }
                }

                if (old_cat != info.category_id)
                {


                    MessageBox.Show(old_cat.ToString());
                    MessageBox.Show(info.category_id.ToString());
                    for (int i =0; i <_categories.Count; ++i)
                    {
                        if (_categories[i].ID == old_cat)
                        {
                            for (int j = 0; j < _categories[i].Books.Count; ++j)
                            {
                                if (_categories[i].Books[j].id == info.id)
                                {
                                    MessageBox.Show(_categories[i].Books[j].name);
                                    _categories[i].Books.RemoveAt(j);
                                    break;
                                }
                            }
                        }
                    }





                    for (int i = 0; i < _categories.Count; ++i)
                    {
                        if (_categories[i].ID == book.category_id)
                        {
                            _categories[i].Books.Add(info);
                        }
                    }




                }
            }




            // window loaded

            if (dao.CanConnect())
            {
                dao.Connect();
                // Thao tác với CSDL ở đây
                _bus = new Business(dao);
                _categories = _bus.ReadAllCategory();
                for (int i = 0; i < _categories.Count; i++)
                {
                    _categories[i].Books = _bus.GetBooksByCategoryId(_categories[i].ID);
                    for (int j = 0; j < _categories[i].Books.Count; j++)
                    {
                        _categories[i].Books[j].Category = _categories[i];
                    }
                }

            }
            categoriesComboBox.ItemsSource = _categories;
            booksListview.ItemsSource = _vm.SelectedBooks;
            pageNumberComboBox.SelectedIndex = 1;
            categoriesComboBox.SelectedIndex = currentCat;


            numberOfBookTextBlock.Text = "Số lượng sách :" + _bus.countBook().ToString();



        }

        private void Detail_Click(object sender, RoutedEventArgs e)
        {
            var book = booksListview.SelectedItem as Book;
            var screen = new DetailBookWindow(book);
            if (screen.ShowDialog() == true)
            {
                // Do nothing
            }
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

        private void AddCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            var screen = new addCategoryWindow();
            if (screen.ShowDialog() == true)
            {
                var newCategory = screen.newCat;

                string? connectionString = AppConfig.ConnectionString();
                var dao = new SqlDataAccess(connectionString!);
                if (dao.CanConnect())
                {
                    dao.Connect();
                    // Thao tác với CSDL ở đây
                    var _bus = new Business(dao);

                    _bus.insertCategory(newCategory.Name);
                    MessageBox.Show("Insert new category successfully!");
                    _categories = _bus.ReadAllCategory();

                    this.categoriesComboBox.ItemsSource = _categories;
                }
                else
                {
                    MessageBox.Show("Cannot connect to db");
                }
            }
        }

        private void DeleteCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            string? connectionString = AppConfig.ConnectionString();
            var dao = new SqlDataAccess(connectionString!);
            if (dao.CanConnect())
            {
                dao.Connect();
                // Thao tác với CSDL ở đây
                var _bus = new Business(dao);

                _categories = _bus.ReadAllCategory();
                var screen = new deleteCategoryWindow(_categories);

                if (screen.ShowDialog() == true)
                {
                    var index = screen.index;
                    _bus.DeleteCategoryById(_categories[index].ID);
                    _categories.RemoveAt(index);

                    MessageBox.Show("Delete category successfully!");
                    categoriesComboBox.ItemsSource = _categories;
                }
            }
            else
            {
                MessageBox.Show("Cannot connect to db");
            }
        }

        private void Import_Click(object sender, RoutedEventArgs e)
        {
            Business _bus = null;
            string? connectionString = AppConfig.ConnectionString();
            var dao = new SqlDataAccess(connectionString!);
            if (dao.CanConnect())
            {
                dao.Connect();
                _bus = new Business(dao);
            }
          
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            _categories = new List<Category>();

            var screen = new OpenFileDialog();

            if (screen.ShowDialog() == true)
            {
                string filename = screen.FileName;

                var workbook = new Workbook(filename);

                var tabs = workbook.Worksheets;
                // In ra các tab để debug
                foreach (var tab in tabs)
                {
                    Category cat = new Category()
                    {
                        Name = tab.Name,
                        Books = new List<Book>()
                    };

                    // Bắt đầu từ ô B3
                    var column = 'C';
                    var row = 4;

                    var cell = tab.Cells[$"{column}{row}"];

                    while (cell.Value != null)
                    {
                        string name = cell.StringValue;
                        string author = tab.Cells[$"D{row}"].StringValue;
                        int publicYear = tab.Cells[$"E{row}"].IntValue;
                        string bookCover = tab.Cells[$"F{row}"].StringValue;
                        int purchasePrice = tab.Cells[$"G{row}"].IntValue;
                        int sellingPrice = tab.Cells[$"H{row}"].IntValue;
                        int stockNumer = tab.Cells[$"I{row}"].IntValue;
                        int sellingNumber = tab.Cells[$"J{row}"].IntValue;

                        Console.WriteLine(bookCover);

                        var p = new Book(name, author, publicYear, bookCover, purchasePrice, sellingPrice, stockNumer, sellingNumber);
                        cat.Books.Add(p);

                        row++;
                        cell = tab.Cells[$"{column}{row}"];
                    }

                    _categories.Add(cat); // Model
                }


            }
            _bus.deleteTable("Book");
            _bus.deleteTable("Category");

            for (int i = 0; i < _categories.Count(); i++)
            {
                _bus.insertCategory(_categories[i].Name);
                int id = _bus.getCategoryID(_categories[i].Name);

                for (int j = 0; j < _categories[i].Books.Count(); j++)
                {

                    _bus.insertBook(_categories[i].Books[j].name, _categories[i].Books[j].author, _categories[i].Books[j].publicYear, _categories[i].Books[j].bookCover, _categories[i].Books[j].purchasePrice, _categories[i].Books[j].sellingPrice, _categories[i].Books[j].stockNumer, _categories[i].Books[j].sellingNumber, id);
                }

            }
        }

        private void UpdateCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            string? connectionString = AppConfig.ConnectionString();
            var dao = new SqlDataAccess(connectionString!);
            if (dao.CanConnect())
            {
                dao.Connect();
                // Thao tác với CSDL ở đây
                var _bus = new Business(dao);

                _categories = _bus.ReadAllCategory();
                var screen = new UpdateCategoryWindow(_categories);

                if (screen.ShowDialog() == true)
                {
                    var temp = screen.newCat;
                    _bus.updateNameCategoryByID(temp.ID, temp.Name);

                    categoriesComboBox.ItemsSource = _categories;
                }
            }
            else
            {
                MessageBox.Show("Cannot connect to db");
            }
        }
    }
}
