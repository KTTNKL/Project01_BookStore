using BookStore.Database;
using BookStore.MyUserControl;
using Fluent;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public MainWindow()
        {
            InitializeComponent();
        }

        private void exitMenu_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void excelImportButton_Clicked(object sender, RoutedEventArgs e)
        {

        }

        private void deleteCategorytButton_Clicked(object sender, RoutedEventArgs e)
        {

        }

        private void addCategorytButton_Clicked(object sender, RoutedEventArgs e)
        {

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
            var screens = new ObservableCollection<TabItem>()
             {
            new TabItem() { Content = new MasterDataUserControl()},
            new TabItem() { Content = new SaleUserControl()},
            new TabItem() { Content = new ReportUserControl()}
            };
            tabs.ItemsSource = screens;

            string? connectionString = AppConfig.ConnectionString();
            var dao = new SqlDataAccess(connectionString!);
            if (dao.CanConnect())
            {
                dao.Connect();
                // Thao tác với CSDL ở đây

                MessageBox.Show("Connect to db");

            }
            else
            {
                MessageBox.Show("Cannot connect to db");
            }
        }
    }
}
