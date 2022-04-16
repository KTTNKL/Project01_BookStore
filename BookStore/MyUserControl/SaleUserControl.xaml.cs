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
    /// Interaction logic for SaleUserControl.xaml
    /// </summary>
    public partial class SaleUserControl : UserControl
    {
        BindingList<Purchase> _list = new BindingList<Purchase>();
        int _totalItems = 0;
        int _currentPage = 0;
        int _totalPages = 0;
        int _rowsPerPage = 10;

        class Page: INotifyPropertyChanged{
            public int currentPage { get; set; }
            public int totalPage { get; set; }

            public event PropertyChangedEventHandler? PropertyChanged;
        }

        BindingList<Page> page = new BindingList<Page>();
        public SaleUserControl()
        {
            InitializeComponent();

        }


        private void addNewOrder_Click(object sender, RoutedEventArgs e)
        {
            Business _bus = null;
            string? connectionString = AppConfig.ConnectionString();
            var dao = new SqlDataAccess(connectionString!);
            if (dao.CanConnect())
            {
                dao.Connect();
                // Thao tác với CSDL ở đây
                _bus = new Business(dao);
                var init = _bus.ReadAllBook();
                var screen = new AddSaleData(init);
                if (screen.ShowDialog() == true) {
                    _list = _bus.ReadAllPurchase();
                    orderComboBox.ItemsSource = _list;
                    calcPage();
                    updatePage();
                    _currentPage = 1;
                    updateData();
                    numberOfOrderTextBlock.Text = "Số lượng đơn hàng: " + _bus.NumberOfOrder();
                    updateMonth();
                    updateWeek();
                }
            }
           
        }

        private void deleteOrder_Click(object sender, RoutedEventArgs e)
        {
            int index = orderComboBox.SelectedIndex;
            if (index >= 0)
            {
                string? connectionString = AppConfig.ConnectionString();
                var dao = new SqlDataAccess(connectionString!);
                if (dao.CanConnect())
                {
                    dao.Connect();
                    // Thao tác với CSDL ở đây
                    var _bus = new Business(dao);

                    _bus.DeleteOrderByID(_list[index].id);

                    MessageBox.Show("Xóa đơn hàng thành công" , " ", MessageBoxButton.OK, MessageBoxImage.Information);

                    // cap nhat lai danh sach order
                    _list = _bus.ReadAllPurchase();
                    orderComboBox.ItemsSource = _list;
                    calcPage();
                    updatePage();
                    _currentPage = 1;
                    updateData();
                    numberOfOrderTextBlock.Text = "Số lượng đơn hàng: " + _bus.NumberOfOrder();
                    updateMonth();
                    updateWeek();
                }
                else
                {
                    MessageBox.Show("Cannot connect to db");
                }
            }
            else
            {
                MessageBox.Show("Please choose an order");
            }
        }

        private void nextButton_Click(object sender, RoutedEventArgs e)
        {
            _currentPage++;
            if (_currentPage <= _totalPages) updateData();
            else _currentPage--;
        }

        private void previousButton_Click(object sender, RoutedEventArgs e)
        {
            _currentPage--;
            if (_currentPage > 0) updateData();
            else _currentPage++;
        }

  
        private void Sale_Loaded(object sender, RoutedEventArgs e)
        {
            try { 

            string? connectionString = AppConfig.ConnectionString();
            var dao = new SqlDataAccess(connectionString!);
            if (dao.CanConnect())
            {
                dao.Connect();
                // Thao tác với CSDL ở đây
                var _bus = new Business(dao);

                // hiển thị số lượng order
                numberOfOrderTextBlock.Text = "Tổng số lượng đơn hàng: " + _bus.NumberOfOrder();

                _list = _bus.ReadAllPurchase();
                    updateMonth();
                    updateWeek();
                calcPage();
                updatePage();
                _currentPage = 1;
                updateData();
            }
            else
            {
                MessageBox.Show("Cannot connect to db");
            }
        }catch (Exception ex)
            {
                MessageBox.Show("Cannot connect to database");
            }
            Window window = Window.GetWindow(this);
            window.Closing += window_Closing;
        }
        public string GetCurentDayOfTheWeek()
        {
            return DateTime.Now.ToString("dddd");
        }
        void updateWeek()
        {
            var DayoftheWeek = new Dictionary<string, int>();
            DayoftheWeek.Add("Monday", 1);
            DayoftheWeek.Add("Tuesday", 2);
            DayoftheWeek.Add("Wednesday", 3);
            DayoftheWeek.Add("Thursday", 4);
            DayoftheWeek.Add("Friday", 5);
            DayoftheWeek.Add("Saturday", 6);
            DayoftheWeek.Add("Sunday", 7);
            var Week03_end = DateTime.Now.AddDays(-DayoftheWeek[GetCurentDayOfTheWeek()]);
            var Week04_start = Week03_end.AddDays(1);
            var Week04_end = DateTime.Now;

            string? connectionString = AppConfig.ConnectionString();
            var dao = new SqlDataAccess(connectionString!);
            if (dao.CanConnect())
            {
                dao.Connect();
                var _bus = new Business(dao);
                List<string> _dates = _bus.getAllPurchaseDay();
                var sum = 0;
                for (int i = 0; i < _dates.Count; ++i)
                {
                    if ((checkAfter(_dates[i], Week04_start.ToString("d/M/yyyy")) || _dates[i] == Week04_start.ToString("d/M/yyyy")) && (!checkAfter(_dates[i], Week04_end.ToString("d/M/yyyy"))))
                    {
                        sum+=1;
                    }
                }
                numberOfOrderWeekTextBlock.Text = "Tuần này:" + sum;

            }
        }
        void updateMonth()
        {
            string? connectionString = AppConfig.ConnectionString();
            var dao = new SqlDataAccess(connectionString!);
            if (dao.CanConnect())
            {
                dao.Connect();
                // Thao tác với CSDL ở đây
                var _bus = new Business(dao);
                _list = _bus.ReadAllPurchase();
                var month = 0;
                foreach (var purchase in _list)
                {
                    if (getMonth(purchase.date) == getMonth(DateTime.Now.ToString("d/M/yyyy")))
                    {
                        month += 1;
                    }
                }
                numberOfOrderMonthTextBlock.Text = "Tháng này: " + month;
            }
        }

        void window_Closing(object sender, global::System.ComponentModel.CancelEventArgs e)
        {
            AppConfig.SetValue(AppConfig.Page, "1");
        }
        private void calcPage()
        {
            _totalItems = _list.Count();
            _totalPages = _list.Count() / _rowsPerPage + (_list.Count() % _rowsPerPage == 0 ? 0 : 1);

        }
        private void updatePage()
        {
            page.Clear();
            for (int i = 1; i <= _totalPages; i++)
            {
                page.Add(new Page()
                {
                    totalPage = _totalPages,
                    currentPage = i
                });
            }
            pageComboBox.SelectedIndex = 0;

        }
        private void updateData()
        {
            calcPage();
            pageComboBox.ItemsSource = page;
            orderComboBox.ItemsSource = _list.Skip((_currentPage - 1) * _rowsPerPage).Take(_rowsPerPage).ToList();
        }


        private void pageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int i = pageComboBox.SelectedIndex;
            _currentPage = i + 1;
            updateData();
        }

        private void viewOrder_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int index = (_currentPage - 1) * 10 + orderComboBox.SelectedIndex;
                if (index >= 0)
                {
                    string? connectionString = AppConfig.ConnectionString();
                    var dao = new SqlDataAccess(connectionString!);
                    if (dao.CanConnect())
                    {
                        dao.Connect();
                        // Thao tác với CSDL ở đây
                        var _bus = new Business(dao);
                        var detailOrderList = _bus.getAllDetailOrder(_list[index].id);

                        for (int i = 0; i < detailOrderList.Count(); i++)
                        {
                            detailOrderList[i].name = _bus.GetBookNameById(detailOrderList[i].book_id);
                        }

                        var screen = new DetailPurchaseWindow(detailOrderList, _list[index].customerName, _list[index].tel, _list[index].address, _list[index].total, _list[index].status);
                        if (screen.ShowDialog() == true)
                        {
                            if (screen.statusOrder == 0)
                            {
                                _bus.updateStatusOrder(_list[index].id, "shipping");
                                _list[index].status = "shipping";
                            }
                            else if (screen.statusOrder == 1)
                            {
                                _bus.updateStatusOrder(_list[index].id, "shipped");
                                _list[index].status = "shipped";
                            }
                            else
                            {
                                _bus.updateStatusOrder(_list[index].id, "cancel");
                                _list[index].status = "cancel";
                            }
                            orderComboBox.ItemsSource = _list;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Cannot connect to db");
                    }
                }
                else
                {
                    MessageBox.Show("Please choose an order");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Book detail has been deleted (This is caused by importing new books or deleting books). Cannot view order detail");
            }


        }

        private void pickStartingDate_Click(object sender, RoutedEventArgs e)
        {
            var screen = new PickDateWindow();
            if (screen.ShowDialog() == true)
            {
                string date = screen.PickedDate;
                StartDate.Content = date;
            }
        }

        private void pickEndingDate_Click(object sender, RoutedEventArgs e)
        {
            var screen = new PickDateWindow();
            if (screen.ShowDialog() == true)
            {
                string date = screen.PickedDate;
                EndDate.Content = date;
            }
        }

        private void searchDateClick(object sender, RoutedEventArgs e)
        {
            var startDate = StartDate.Content.ToString();
            var endDate = EndDate.Content.ToString();
            if (startDate != "N/A" && endDate != "N/A")
            {
                if (checkAfter(startDate, endDate) == true)
                {
                    MessageBox.Show("The starting date must be before the ending date");
                }
                else {
                    string? connectionString = AppConfig.ConnectionString();
                    var dao = new SqlDataAccess(connectionString!);
                    if (dao.CanConnect())
                    {
                        dao.Connect();
                        // Thao tác với CSDL ở đây
                        var _bus = new Business(dao);
                        BindingList<Purchase> _dates = _bus.ReadAllPurchase();
                        BindingList<Purchase> _validDates = new BindingList<Purchase>();
                        for (int i = 0; i < _dates.Count; ++i)
                        {
                            if ((checkAfter(_dates[i].date, startDate) || _dates[i].date == startDate) && (!checkAfter(_dates[i].date, endDate)))
                            {
                                _validDates.Add(_dates[i]);
                            }
                        }
                        _list = _validDates;
                        orderComboBox.ItemsSource = _list;
                        calcPage();
                        updatePage();
                        _currentPage = 1;
                        updateData();
                    }
                 }
            }
            else {
                MessageBox.Show("Please Pick Dates");
            }
        }
        public int getDay(string year)
        {
            string[] list = year.Split('/');
            return int.Parse(list[0]);
        }

        public int getMonth(string year)
        {
            string[] list = year.Split('/');
            return int.Parse(list[1]);
        }

        public int getYear(string year)
        {
            string[] list = year.Split('/');
            return int.Parse(list[2]);
        }


        public bool checkAfter(string checkingYear, string MileStone)
        {
            if (getYear(checkingYear) > getYear(MileStone))
            {
                return true;
            }
            else if (getYear(checkingYear) == getYear(MileStone))
            {
                if (getMonth(checkingYear) > getMonth(MileStone))
                {
                    return true;
                }
                else if (getMonth(checkingYear) == getMonth(MileStone))
                {
                    if (getDay(checkingYear) > getDay(MileStone))
                    {
                        return true;
                    }
                    else
                    {
                        return false; // equal is still false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

        }

       
    }
}
