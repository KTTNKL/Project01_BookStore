using BookStore.Database;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
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

        private void weekly_Click(object sender, RoutedEventArgs e)
        {
            var DayoftheWeek = new Dictionary<string, int>();
            DayoftheWeek.Add("Monday", 1);
            DayoftheWeek.Add("Tuesday", 2);
            DayoftheWeek.Add("Wednesday", 3);
            DayoftheWeek.Add("Thursday", 4);
            DayoftheWeek.Add("Friday", 5);
            DayoftheWeek.Add("Saturday", 6);
            DayoftheWeek.Add("Sunday", 7);

            string? connectionString = AppConfig.ConnectionString();
            var dao = new SqlDataAccess(connectionString!);
            if (dao.CanConnect())
            {
                dao.Connect();
                // Thao tác với CSDL ở đây
                var _bus = new Business(dao);

                // xu li cac ngay



                var Week03_end = DateTime.Now.AddDays(-DayoftheWeek[GetCurentDayOfTheWeek()]);
                var Week03_start = Week03_end.AddDays(-6);
                var Week02_end = Week03_start.AddDays(-1);
                var Week02_start = Week02_end.AddDays(-6);
                var Week01_end = Week02_start.AddDays(-1);
                var Week01_start = Week01_end.AddDays(-6);
                var Week04_start = Week03_end.AddDays(1);
                var Week04_end = DateTime.Now;
                // xu li doanh thu cac khoan thoi gian

                List<string> _dates = _bus.getAllPurchaseDay();

                // tuan 1
                var revenue_week01 = 0;
                var profit_week01 = 0;
                List<string> _week01_dates = new List<string>();
                for (int i = 0; i < _dates.Count; ++i)
                {
                    if ((checkAfter(_dates[i], Week01_start.ToString("d/M/yyyy")) || _dates[i] == Week01_start.ToString("d/M/yyyy")) && (!checkAfter(_dates[i], Week01_end.ToString("d/M/yyyy"))))
                    {
                        _week01_dates.Add(_dates[i]);
                    }
                }
                for (int j = 0; j < _week01_dates.Count; ++j)
                {
                    profit_week01 += _bus.AnnualProfit(_week01_dates[j]);
                    revenue_week01 += _bus.AnnualRevenue(_week01_dates[j]); // use date as year to be param
                }

                // tuan 2
                var revenue_week02 = 0;
                var profit_week02 = 0;
                List<string> _week02_dates = new List<string>();
                for (int i = 0; i < _dates.Count; ++i)
                {
                    if ((checkAfter(_dates[i], Week02_start.ToString("d/M/yyyy")) || _dates[i] == Week02_start.ToString("d/M/yyyy")) && (!checkAfter(_dates[i], Week02_end.ToString("d/M/yyyy"))))
                    {
                        _week02_dates.Add(_dates[i]);
                    }
                }
                for (int j = 0; j < _week02_dates.Count; ++j)
                {
                    profit_week02 += _bus.AnnualProfit(_week02_dates[j]);
                    revenue_week02 += _bus.AnnualRevenue(_week02_dates[j]); // use date as year to be param
                }

                // tuan 3
                var revenue_week03 = 0;
                var profit_week03 = 0;
                List<string> _week03_dates = new List<string>();
                for (int i = 0; i < _dates.Count; ++i)
                {
                    if ((checkAfter(_dates[i], Week03_start.ToString("d/M/yyyy")) || _dates[i] == Week03_start.ToString("d/M/yyyy")) && (!checkAfter(_dates[i], Week03_end.ToString("d/M/yyyy"))))
                    {
                        _week03_dates.Add(_dates[i]);
                    }
                }
                for (int j = 0; j < _week03_dates.Count; ++j)
                {
                    profit_week03 += _bus.AnnualProfit(_week03_dates[j]);
                    revenue_week03 += _bus.AnnualRevenue(_week03_dates[j]); // use date as year to be param
                }

                // tuan 4
                var revenue_week04 = 0;
                var profit_week04 = 0;
                List<string> _week04_dates = new List<string>();
                for (int i = 0; i < _dates.Count; ++i)
                {
                    if ((checkAfter(_dates[i], Week04_start.ToString("d/M/yyyy")) || _dates[i] == Week04_start.ToString("d/M/yyyy")) && (!checkAfter(_dates[i], Week04_end.ToString("d/M/yyyy"))))
                    {
                        _week04_dates.Add(_dates[i]);
                    }
                }
                for (int j = 0; j < _week04_dates.Count; ++j)
                {
                    profit_week04 += _bus.AnnualProfit(_week04_dates[j]);
                    revenue_week04 += _bus.AnnualRevenue(_week04_dates[j]); // use date as year to be param
                }

                Chart.Children.RemoveRange(0, 1); // xoa chart cu
                CartesianChart ch = new CartesianChart();
                ch.AxisX.Add(new Axis
                {
                    Title = "Week (4 nearest weeks)",
                    Labels = new[] { Week01_start.ToString("dd/MM/yyyy")+" - "+Week01_end.ToString("dd/MM/yyyy"), Week02_start.ToString("dd/MM/yyyy") + " - " + Week02_end.ToString("dd/MM/yyyy"), Week03_start.ToString("dd/MM/yyyy") + " - " + Week03_end.ToString("dd/MM/yyyy"), Week04_start.ToString("dd/MM/yyyy") + " - " + Week04_end.ToString("dd/MM/yyyy")+"(Today)" }
                });
                ch.Series = new SeriesCollection
                {
                new ColumnSeries
                {
                    Title = "Total sales:",
                    Values = new ChartValues<decimal>{ revenue_week01, revenue_week02, revenue_week03, revenue_week04 }

                },
                new ColumnSeries
                {
                    Title = "Profit:",
                    Values = new ChartValues<decimal>{ profit_week01, profit_week02, profit_week03, profit_week04 }
                }
            };
                Chart.Children.Add(ch);


            }

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
                    Title = "Month (3 nearest months)",
                    Labels = new[] { month_2.ToString() +"/" + year_2.ToString(), month_1.ToString() + "/" + year_1.ToString(), currentmonth.ToString() + "/" + currentyear.ToString() }
                });
                ch.Series = new SeriesCollection
                {
                new ColumnSeries
                {
                    Title = "Total sales:",
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
                    Title = "Year (4 nearest years)",
                    Labels = new[] { (DateTime.Now.Year - 3).ToString(), (DateTime.Now.Year - 2).ToString(), (DateTime.Now.Year - 1).ToString(), DateTime.Now.Year.ToString() }
                });
                ch.Series = new SeriesCollection
                {
                new ColumnSeries
                {
                    Title = "Total sales:",
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
            var startDate = StartDate.Content.ToString();
            var endDate = EndDate.Content.ToString();
            if (startDate != "N/A" && endDate != "N/A")
            {
                if(checkAfter(startDate, endDate) == true)
                {
                    MessageBox.Show("The starting date must be before the ending date");
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
                        Chart.Children.RemoveRange(0, 1);
                        CartesianChart ch = new CartesianChart();

                        ch.AxisX.Add(new Axis
                        {
                            Title = "Date",
                            Labels = new[] {startDate +" - " + endDate }
                        });
                        ch.Series = new SeriesCollection
                        {

                            new ColumnSeries
                            {
                                Title = "Total sales :",
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

        private void Report_Loaded(object sender, RoutedEventArgs e)
        {
            Window window = Window.GetWindow(this);
            window.Closing += window_Closing;
        }
        
        void window_Closing(object sender, global::System.ComponentModel.CancelEventArgs e)
        {
            AppConfig.SetValue(AppConfig.Page, "2");
        }
        
        public string GetCurentDayOfTheWeek()
        {
            return DateTime.Now.ToString("dddd");
        }

        private void weeklyBooks_Click(object sender, RoutedEventArgs e)
        {
            var DayoftheWeek = new Dictionary<string, int>();
            DayoftheWeek.Add("Monday", 1);
            DayoftheWeek.Add("Tuesday", 2);
            DayoftheWeek.Add("Wednesday", 3);
            DayoftheWeek.Add("Thursday", 4);
            DayoftheWeek.Add("Friday", 5);
            DayoftheWeek.Add("Saturday", 6);
            DayoftheWeek.Add("Sunday", 7);

            string? connectionString = AppConfig.ConnectionString();
            var dao = new SqlDataAccess(connectionString!);
            if (dao.CanConnect())
            {
                dao.Connect();
                // Thao tác với CSDL ở đây
                var _bus = new Business(dao);

                // xu li cac ngay

                var Week03_end = DateTime.Now.AddDays(-DayoftheWeek[GetCurentDayOfTheWeek()]);
                var Week03_start = Week03_end.AddDays(-6);
                var Week02_end = Week03_start.AddDays(-1);
                var Week02_start = Week02_end.AddDays(-6);
                var Week01_end = Week02_start.AddDays(-1);
                var Week01_start = Week01_end.AddDays(-6);
                var Week04_start = Week03_end.AddDays(1);
                var Week04_end = DateTime.Now;
                // xu li doanh thu cac khoan thoi gian

                List<string> _dates = _bus.getAllPurchaseDay();

                // tuan 1
                var book_count_week01 = 0;
                List<string> _week01_dates = new List<string>();
                for (int i = 0; i < _dates.Count; ++i)
                {
                    if ((checkAfter(_dates[i], Week01_start.ToString("d/M/yyyy")) || _dates[i] == Week01_start.ToString("d/M/yyyy")) && (!checkAfter(_dates[i], Week01_end.ToString("d/M/yyyy"))))
                    {
                        _week01_dates.Add(_dates[i]);
                    }
                }
                for (int j = 0; j < _week01_dates.Count; ++j)
                {// use date as year to be param
                    book_count_week01 += _bus.AnnualBookCount(_week01_dates[j]);
                }

                // tuan 2
                var book_count_week02 = 0;

                List<string> _week02_dates = new List<string>();
                for (int i = 0; i < _dates.Count; ++i)
                {
                    if ((checkAfter(_dates[i], Week02_start.ToString("d/M/yyyy")) || _dates[i] == Week02_start.ToString("d/M/yyyy")) && (!checkAfter(_dates[i], Week02_end.ToString("d/M/yyyy"))))
                    {
                        _week02_dates.Add(_dates[i]);
                    }
                }
                for (int j = 0; j < _week02_dates.Count; ++j)
                {
                    book_count_week02 += _bus.AnnualBookCount(_week02_dates[j]); // use date as year to be param
                }

                // tuan 3
                var book_count_week03 = 0;
                List<string> _week03_dates = new List<string>();
                for (int i = 0; i < _dates.Count; ++i)
                {
                    if ((checkAfter(_dates[i], Week03_start.ToString("d/M/yyyy")) || _dates[i] == Week03_start.ToString("d/M/yyyy")) && (!checkAfter(_dates[i], Week03_end.ToString("d/M/yyyy"))))
                    {
                        _week03_dates.Add(_dates[i]);
                    }
                }
                for (int j = 0; j < _week03_dates.Count; ++j)
                {
                    book_count_week03 += _bus.AnnualBookCount(_week03_dates[j]); // use date as year to be param
                }

                // tuan 4
                var book_count_week04 = 0;
                List<string> _week04_dates = new List<string>();
                for (int i = 0; i < _dates.Count; ++i)
                {
                    if ((checkAfter(_dates[i], Week04_start.ToString("d/M/yyyy")) || _dates[i] == Week04_start.ToString("d/M/yyyy")) && (!checkAfter(_dates[i], Week04_end.ToString("d/M/yyyy"))))
                    {
                        _week04_dates.Add(_dates[i]);
                    }
                }
                for (int j = 0; j < _week04_dates.Count; ++j)
                {
                    book_count_week04 += _bus.AnnualBookCount(_week04_dates[j]); // use date as year to be param
                }

                Chart.Children.RemoveRange(0, 1); // xoa chart cu
                CartesianChart ch = new CartesianChart();
                ch.AxisX.Add(new Axis
                {
                    Title = "Week (4 nearest weeks)",
                    Labels = new[] { Week01_start.ToString("dd/MM/yyyy") + " - " + Week01_end.ToString("dd/MM/yyyy"), Week02_start.ToString("dd/MM/yyyy") + " - " + Week02_end.ToString("dd/MM/yyyy"), Week03_start.ToString("dd/MM/yyyy") + " - " + Week03_end.ToString("dd/MM/yyyy"), Week04_start.ToString("dd/MM/yyyy") + " - " + Week04_end.ToString("dd/MM/yyyy") + "(Today)" }
                });
                ch.Series = new SeriesCollection
                {
                new ColumnSeries
                {
                    Title = "Sold books:",
                    Values = new ChartValues<decimal>{ book_count_week01, book_count_week02, book_count_week03, book_count_week04 }

                }
            };
                Chart.Children.Add(ch);


            }
        }

        private void monthlyBooks_Click(object sender, RoutedEventArgs e)
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
                    Title = "Month (3 nearest months)",
                    Labels = new[] { month_2.ToString() + "/" + year_2.ToString(), month_1.ToString() + "/" + year_1.ToString(), currentmonth.ToString() + "/" + currentyear.ToString() }
                });
                ch.Series = new SeriesCollection
                {
                new ColumnSeries
                {
                    Title = "Sold books:",
                    Values = new ChartValues<decimal>{ _bus.MonthlyBookCount(month_2.ToString(),year_2.ToString()), _bus.MonthlyBookCount(month_1.ToString(), year_1.ToString()), _bus.MonthlyBookCount(currentmonth.ToString(), currentyear.ToString()) }

                }
            };
                Chart.Children.Add(ch);
            }
        }

        private void annualBooks_Click(object sender, RoutedEventArgs e)
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
                    Title = "Year (4 nearest years)",
                    Labels = new[] { (DateTime.Now.Year - 3).ToString(), (DateTime.Now.Year - 2).ToString(), (DateTime.Now.Year - 1).ToString(), DateTime.Now.Year.ToString() }
                });
                ch.Series = new SeriesCollection
                {
                new ColumnSeries
                {
                    Title = "Sold books:",
                    Values = new ChartValues<decimal>{ _bus.AnnualBookCount((DateTime.Now.Year - 3).ToString()), _bus.AnnualBookCount((DateTime.Now.Year - 2).ToString()), _bus.AnnualBookCount((DateTime.Now.Year - 1).ToString()), _bus.AnnualBookCount(DateTime.Now.Year.ToString()) }

                }
            };
                Chart.Children.Add(ch);
            }
        }

        private void datedateBooks_Click(object sender, RoutedEventArgs e)
        {
            var startDate = StartDate.Content.ToString();
            var endDate = EndDate.Content.ToString();
            if (startDate != "N/A" && endDate != "N/A")
            {
                if (checkAfter(startDate, endDate) == true)
                {
                    MessageBox.Show("The starting date must be before the ending date");
                }
                else
                {
                    var book_count = 0;
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
                        for (int i = 0; i < _dates.Count; ++i)
                        {
                            if ((checkAfter(_dates[i], startDate) || _dates[i] == startDate) && (!checkAfter(_dates[i], endDate)))
                            {
                                _validDates.Add(_dates[i]);
                            }
                        }
                        for (int j = 0; j < _validDates.Count; ++j)
                        {
                            book_count += _bus.AnnualBookCount(_validDates[j]); // use date as year to be param

                        }
                        Chart.Children.RemoveRange(0, 1);
                        CartesianChart ch = new CartesianChart();

                        ch.AxisX.Add(new Axis
                        {
                            Title = "Date",
                            Labels = new[] { startDate + " - " + endDate }
                        });
                        ch.Series = new SeriesCollection
                        {
                            new ColumnSeries
                            {
                                Title = "",
                                Values = new ChartValues<decimal>{ 0}

                            },
                            new ColumnSeries
                            {
                                Title = "Sold books",
                                Values = new ChartValues<decimal>{ book_count}

                            },
                            new ColumnSeries
                            {
                                IsEnabled = false,   
                                Title = "",
                                Values = new ChartValues<decimal>{ 0 }
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
    }
}
