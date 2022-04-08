﻿using BookStore.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        private void viewDetail_Click(object sender, RoutedEventArgs e)
        {
            int index = orderComboBox.SelectedIndex;
            string? connectionString = AppConfig.ConnectionString();
            var dao = new SqlDataAccess(connectionString!);
            if (dao.CanConnect())
            {
                dao.Connect();
                // Thao tác với CSDL ở đây
                var _bus = new Business(dao);
                //var detailOrderList = _bus.getAllDetailOrder(_list[index].id);
                //new DetailPurchaseWindow(detailOrderList).Show();
                //var a = screen.a;
                //MessageBox.Show(a.ToString());
            }
            else
            {
                MessageBox.Show("Cannot connect to db");
            }
        }

        private void addNewOrder_Click(object sender, RoutedEventArgs e)
        {

        }

        private void deleteOrder_Click(object sender, RoutedEventArgs e)
        {

        }

        private void editOrder_Click(object sender, RoutedEventArgs e)
        {

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

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Sale_Loaded(object sender, RoutedEventArgs e)
        {
            string? connectionString = AppConfig.ConnectionString();
            var dao = new SqlDataAccess(connectionString!);
            if (dao.CanConnect())
            {
                dao.Connect();
                // Thao tác với CSDL ở đây
                var _bus = new Business(dao);

                _list = _bus.ReadAllPurchase();
                orderComboBox.ItemsSource = _list;
                calcPage();
                updatePage();
                _currentPage = 1;
                updateData();
            }
            else
            {
                MessageBox.Show("Cannot connect to db");
            }
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
    }
}
