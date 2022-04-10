using BookStore.Database;
using LiveCharts;
using LiveCharts.Wpf;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BookStore.MyUserControl
{
    /// <summary>
    /// Interaction logic for ReportUserControl.xaml
    /// </summary>
    public partial class ReportUserControl : UserControl
    {
        public SeriesCollection SeriesCollection { get; set; }
        public ReportUserControl()
        {
            InitializeComponent();
        }


        private void pickStartingDate_Click(object sender, RoutedEventArgs e)
        {
            var screen = new PickDateWindow();
            if (screen.ShowDialog() == true)
            {
                string date = screen.PickedDate;
                StartDate.Text = date;
            }
        }

        private void pickEndingDate_Click(object sender, RoutedEventArgs e)
        {
            var screen = new PickDateWindow();
            if (screen.ShowDialog() == true)
            {
                string date = screen.PickedDate;
                EndDate.Text = date;
            }
        }

        private void weekly_Click(object sender, RoutedEventArgs e)
        {
            Chart.Children.RemoveRange(0, 1);
            CartesianChart ch = new CartesianChart();
            ch.AxisX.Add(new Axis
            {
                Title = "Week",
                Labels = new[] {"Week 1", "Week 2", "Week 3", "Week 4"}
            });
            ch.Series = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Revenue:",
                    Values = new ChartValues<decimal>{5,6,7,8}
                    
                },
                new ColumnSeries
                {
                    Title = "Profit:",
                    Values = new ChartValues<decimal>{3,2,5,4}
                }
            };
            Chart.Children.Add(ch);
        }

        private void monthly_Click(object sender, RoutedEventArgs e)
        {
            string? connectionString = AppConfig.ConnectionString();
            var dao = new SqlDataAccess(connectionString!);
            if (dao.CanConnect())
            {
                dao.Connect();
                // Thao tác với CSDL ở đây
                var _bus = new Business(dao);

                var currentmonth = DateTime.Now.Month;
                var currentyear = DateTime.Now.Year;
                var month_1 = DateTime.Now.Month - 1;
                var year_1 = DateTime.Now.Year;
                var month_2 = DateTime.Now.Month - 2;
                var year_2 = DateTime.Now.Year;
                if (currentmonth == 1)
                {
                    month_1 = 12;
                    year_1 = currentyear - 1;
                    month_2 = 11;
                    year_2 = currentyear - 1;
                }
                if (currentmonth == 2)
                {
                    month_2 = 12;
                    year_2 = currentyear - 1;
                }


                Chart.Children.RemoveRange(0, 1); // xoa chart cu
                CartesianChart ch = new CartesianChart();
                ch.AxisX.Add(new Axis
                {
                    Title = "Month",
                    Labels = new[] { month_2.ToString() +"/" + year_2.ToString(), month_1.ToString() + "/" + year_1.ToString(), currentmonth.ToString() + "/" + currentyear.ToString() }
                });
                ch.Series = new SeriesCollection
                {
                new ColumnSeries
                {
                    Title = "Revenue:",
                    Values = new ChartValues<decimal>{ _bus.MonthlyRevenue(month_2.ToString(),year_2.ToString()), _bus.MonthlyRevenue(month_1.ToString(), year_1.ToString()), _bus.MonthlyRevenue(currentmonth.ToString(), currentyear.ToString()) }

                },
                new ColumnSeries
                {
                    Title = "Profit:",
                    Values = new ChartValues<decimal>{ _bus.MonthlyProfit(month_2.ToString(),year_2.ToString()), _bus.MonthlyProfit(month_1.ToString(), year_1.ToString()), _bus.MonthlyProfit(currentmonth.ToString(), currentyear.ToString()) }
                }
            };
                Chart.Children.Add(ch);
            }
        }

        private void annual_Click(object sender, RoutedEventArgs e)
        {
            string? connectionString = AppConfig.ConnectionString();
            var dao = new SqlDataAccess(connectionString!);
            if (dao.CanConnect())
            {
                dao.Connect();
                // Thao tác với CSDL ở đây
                var _bus = new Business(dao);

                Chart.Children.RemoveRange(0, 1);
                CartesianChart ch = new CartesianChart();
                ch.AxisX.Add(new Axis
                {
                    Title = "Year",
                    Labels = new[] { (DateTime.Now.Year - 3).ToString(), (DateTime.Now.Year - 2).ToString(), (DateTime.Now.Year - 1).ToString(), DateTime.Now.Year.ToString() }
                });
                ch.Series = new SeriesCollection
                {
                new ColumnSeries
                {
                    Title = "Revenue:",
                    Values = new ChartValues<decimal>{ _bus.AnnualRevenue((DateTime.Now.Year - 3).ToString()), _bus.AnnualRevenue((DateTime.Now.Year - 2).ToString()), _bus.AnnualRevenue((DateTime.Now.Year - 1).ToString()), _bus.AnnualRevenue(DateTime.Now.Year.ToString()) }

                },
                new ColumnSeries
                {
                    Title = "Profit:",
                    Values = new ChartValues<decimal>{ _bus.AnnualProfit((DateTime.Now.Year - 3).ToString()), _bus.AnnualProfit((DateTime.Now.Year - 2).ToString()), _bus.AnnualProfit((DateTime.Now.Year - 1).ToString()), _bus.AnnualProfit(DateTime.Now.Year.ToString()) }
                }
            };
                Chart.Children.Add(ch);
            }
        }

        private void check_Click(object sender, RoutedEventArgs e)
        {
            var startDate = StartDate.Text;
            var endDate = EndDate.Text;
            if (startDate != "N/A" && endDate != "N/A")
            {
                if(checkAfter(startDate, endDate) == true)
                {
                    MessageBox.Show("The Starting Date must be before the ending date");
                }
                else
                {
                    var revenue = 0;
                    var profit = 0;
                    // xu li tai day
                    string? connectionString = AppConfig.ConnectionString();
                    var dao = new SqlDataAccess(connectionString!);
                    if (dao.CanConnect())
                    {
                        dao.Connect();
                        // Thao tác với CSDL ở đây
                        var _bus = new Business(dao);
                        List<string> _dates = _bus.getAllPurchaseDay();
                        List<string> _validDates = new List<string>();
                        for(int i=0; i<_dates.Count; ++i)
                        {
                            if((checkAfter(_dates[i], startDate) || _dates[i] == startDate) &&(!checkAfter(_dates[i], endDate)))
                            {
                                _validDates.Add(_dates[i]);
                            }
                        }
                        for(int j=0; j<_validDates.Count; ++j)
                        {
                            profit += _bus.AnnualProfit(_validDates[j]);
                            revenue += _bus.AnnualRevenue(_validDates[j]); // use date as year to be param

                        }
                        MessageBox.Show(revenue.ToString());
                        MessageBox.Show(profit.ToString());
                        Chart.Children.RemoveRange(0, 1);
                        CartesianChart ch = new CartesianChart();
                        ch.Series = new SeriesCollection
                        {

                new ColumnSeries
                {
                    Title = "Revenue:",
                    Values = new ChartValues<decimal>{ revenue}

                },
                new ColumnSeries
                {
                    Title = "Profit:",
                    Values = new ChartValues<decimal>{ profit }
                }
                         };
                        Chart.Children.Add(ch);

                    }


                }
            }
            else
            {
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


        public bool checkAfter (string checkingYear, string MileStone)
        {
            if(getYear(checkingYear) > getYear(MileStone))
            {
                return true;
            }
            else if(getYear(checkingYear) == getYear(MileStone))
            {
                if(getMonth(checkingYear) > getMonth(MileStone))
                {
                    return true;
                }
                else if (getMonth(checkingYear) == getMonth(MileStone))
                {
                    if(getDay(checkingYear) > getDay(MileStone))
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
