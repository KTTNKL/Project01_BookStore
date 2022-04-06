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
    public partial class MainWindow : Window
    {
        Business _bus = null;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void RibbonWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var screens = new ObservableCollection<TabItem>()
             {
            new TabItem() { Header="Master Data", Content = new MasterDataUserControl()},
            new TabItem() { Header="Sale",Content = new SaleUserControl()},
            new TabItem() { Header="Report",Content = new ReportUserControl()}
            };
            tabs.ItemsSource = screens;
            tabs.SelectedIndex = 0;

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
