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
using System.Collections.Generic;
using System.ComponentModel;
using BookStore.Database;
using System.Diagnostics;

namespace BookStore
{
    /// <summary>
    /// Interaction logic for AddSaleData.xaml
    /// </summary>
    public partial class AddSaleData : Window
    {
        BindingList<Book> _list = new BindingList<Book>();
        BindingList<Book> _saleList = new BindingList<Book>();
        int sum = 0;

        public AddSaleData(List<Book> list)
        {
            InitializeComponent();
            _list = new BindingList<Book>(list);
            bookListView.ItemsSource = _list;
        }
      
        private void Search_Click(object sender, RoutedEventArgs e)
        {
            List<Book> init = new List<Book>();
            var name = SearchTextBox.Text.Trim();
            Business _bus = null;
            string? connectionString = AppConfig.ConnectionString();
            var dao = new SqlDataAccess(connectionString!);
            if (dao.CanConnect())
            {
                dao.Connect();
               
                _bus = new Business(dao);
                init = _bus.ReadAllBookLikeName(name);
                _list = new BindingList<Book>(init);
                bookListView.ItemsSource = _list;

            }
        }

        private void AddItem_Click(object sender, RoutedEventArgs e)
        {
            var index = bookListView.SelectedIndex;
            var book = _list[index];
            var isBought = false;
            foreach( var saleBook in _saleList)
            {
                if (saleBook.name == book.name)
                {
                    saleBook.buyNumber += 1;
                    saleBook.saleTotalPrice += book.sellingPrice;
                    sum += book.sellingPrice;
                    Total.Content = "Total: " + sum;

                    isBought = true;
                    break;
                }
            }
            if (!isBought)
            {
                book.buyNumber = 1;
                book.saleTotalPrice = book.sellingPrice;
                sum += book.sellingPrice;
                Total.Content = "Total: " + sum;

                _saleList.Add(book);

            }
            foreach (var i in _saleList)
            {
                Debug.WriteLine(i.name);
                Debug.WriteLine(i.stockNumer);
                Debug.WriteLine(i.sellingNumber);

            }
            bookSaleListView.ItemsSource = _saleList;  
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Total.Content = "Total: "+sum;
        }

        private void EditQuantity_Click(object sender, RoutedEventArgs e)
        {
            var book = (Book)bookSaleListView.SelectedItem;
            var index= bookSaleListView.SelectedIndex;
            var screen = new EditQuantityScreen(book);
            if (screen.ShowDialog() == true)
            {
                var info = screen.editBook;
                book.buyNumber = info.buyNumber;
                sum-=info.saleTotalPrice;
                book.saleTotalPrice= book.sellingPrice* book.buyNumber;
                sum += book.saleTotalPrice;
                Total.Content = "Total: " + sum;

            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            var book = (Book)bookSaleListView.SelectedItem;
            sum-=book.saleTotalPrice;
            Total.Content = "Total: " + sum;

            _saleList.Remove(book);

        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            var temp = 0;
            foreach(var book in _saleList)
            {
                book.stockNumer -= book.buyNumber;
                book.sellingNumber += book.buyNumber;
                temp += book.buyNumber * book.purchasePrice;
            }

            var profit = sum - temp;

            Business _bus = null;
            string? connectionString = AppConfig.ConnectionString();
            var dao = new SqlDataAccess(connectionString!);
            if (dao.CanConnect())
            {
                dao.Connect();

                _bus = new Business(dao);

                if (_saleList.Count == 0)
                {
                    MessageBox.Show("Please buy some thing before hit enter :D !");
                }
                else
                {
                    foreach (var book in _saleList)
                    {
                        _bus.UpdateBook(book.id, book.name, book.author, book.publicYear, book.bookCover, book.purchasePrice, book.sellingPrice, book.stockNumer, book.sellingNumber, book.category_id);

                    }

                    _bus.insertPurchase(NameCustomer.Text, TelephoneCustomer.Text, AddressCustomer.Text, sum, profit, DateTime.Now.ToString("M/d/yyyy"), "đang giao");
                    int id= _bus.LastestPurchaseID();

                    Debug.WriteLine(id);
                    
                    DialogResult = true;

                }

            }

        }
    }
}
