using Aspose.Cells;
using BookStore.Database;
using BookStore.MyUserControl;
using Fluent;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace BookStore
{



        /// <summary>
        /// Interaction logic for MainWindow.xaml
        /// </summary>
        public partial class MainWindow : RibbonWindow
    {

        ObservableCollection<TabItem> screens = new ObservableCollection<TabItem>()
             {
            new TabItem() { Content = new MasterDataUserControl()},
            new TabItem() { Content = new SaleUserControl()},
            new TabItem() { Content = new ReportUserControl()}
            };

        List<Category> _categories = null;
        Business _bus = null;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void exitMenu_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void excelImportButton_Clicked(object sender, RoutedEventArgs e)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            _categories = new List<Category>();

            var screen = new OpenFileDialog();

            if (screen.ShowDialog() == true)
            {
                string filename = screen.FileName;
                Debug.WriteLine(filename);

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
                MessageBox.Show(_categories[i].Name);

                for (int j = 0; j < _categories[i].Books.Count(); j++)
                {

                    _bus.insertBook(_categories[i].Books[j].name, _categories[i].Books[j].author, _categories[i].Books[j].publicYear, _categories[i].Books[j].bookCover, _categories[i].Books[j].purchasePrice, _categories[i].Books[j].sellingPrice, _categories[i].Books[j].stockNumer, _categories[i].Books[j].sellingNumber, id);
                }

            }
        }

        private void deleteCategorytButton_Clicked(object sender, RoutedEventArgs e)
        {

        }

        private void addCategorytButton_Clicked(object sender, RoutedEventArgs e)
        {
            var userControl = tabs.Items[0] as MasterDataUserControl;
            userControl.HandleParentEvent( MasterDataUserControl.MasterDataAction.AddNewCategory);
        }

        private void addBookButton_Clicked(object sender, RoutedEventArgs e)
        {

        }

        private void editBookButton_Clicked(object sender, RoutedEventArgs e)
        {

        }

        private void deleteBookButton_Clicked(object sender, RoutedEventArgs e)
        {

        }
        private void RibbonWindow_Loaded(object sender, RoutedEventArgs e)
        {
            tabs.ItemsSource = screens;

            string? connectionString = AppConfig.ConnectionString();
            var dao = new SqlDataAccess(connectionString!);
            if (dao.CanConnect())
            {
                dao.Connect();
                // Thao tác với CSDL ở đây
                _bus = new Business(dao);


                MessageBox.Show("Connect successfully");

            }
            else
            {
                MessageBox.Show("Cannot connect to db");
            }
        }
    }
}
