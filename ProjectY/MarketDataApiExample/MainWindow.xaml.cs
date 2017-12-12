using MarketDataApiExample.ViewModels;
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
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Threading;
using System.Data;
using System.Diagnostics;

namespace MarketDataApiExample
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            InitializeComponent();
            this.DataContext = MainViewModel.Instance;
        }

        private void BtnIPConnect_Click(object sender, RoutedEventArgs e)
        {
            this.BtnIPConnect.IsEnabled = false;
            this.MarketControlAry.IsEnabled = true;
            this.MainControl.IsEnabled = true;
        }

        private void DataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex()+1).ToString();
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
            _logger.Debug(e.ExceptionObject.ToString());
        }

        #region Chart
        DispatcherTimer _dispatcherTimer = new DispatcherTimer();
        
        private void CreateChart_Click(object sender, RoutedEventArgs e)
        {
            //_dispatcherTimer.Stop();

            //設定DataTable的欄位
            //SetDataTable();

            //設定Chart Control
            SetChart();
            //this.mainChart.DataSource = dt;
            this.mainChart.DataBind();//這時候先DataBind()是為了顯示空白的圖表

            _dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            _dispatcherTimer.Interval = new TimeSpan(0, 0, 5);
            _dispatcherTimer.Start();
        }        
        /// <summary>
        /// 設定DataTable的欄位
        /// </summary>
        private void SetDataTable()
        {
            //方式1
            //dt.Clear();
            //DataTable dt = new DataTable();
            //dt.Columns.Add("Quote");
            //dt.Columns.Add("StockMarketTime");
            //for(int y = 0; y <10;y++)
            //{

            //dr["Quote"] = 10;// quotesA.MatchPrice;
            //string time = quotesA.Time.PadLeft(12, '0');
            //DateTime date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, int.Parse(time.Substring(0, 2)), int.Parse(time.Substring(2, 2)), int.Parse(time.Substring(4, 2)));
            //dr["StockMarketTime"] = date.ToOADate();

            //DataRow dr = dt.NewRow();
            //dr["StockMarketTime"] = DateTime.Now.AddSeconds(y).ToOADate();
            //dt.Rows.Add(dr);
            //}
        }

        /// <summary>
        /// 設定Chart Control
        /// </summary>
        private void SetChart()
        {
            ChartArea ca = new ChartArea("ChartArea1");
            ca.Area3DStyle.Enable3D = false;//開啟3D
            this.mainChart.ChartAreas.Add(ca);
            
            Legend lgQuote = new Legend("Legend1");
            lgQuote.IsTextAutoFit = true;
            lgQuote.Docking = Docking.Bottom;
            this.mainChart.Legends.Add(lgQuote);

            Series seQuote = new Series("SeriesQuote");
            seQuote.ChartArea = "ChartArea1";
            seQuote.ChartType = SeriesChartType.Line;
            seQuote.IsVisibleInLegend = true;
            seQuote.Legend = "Legend1";
            seQuote.LegendText = "漲跌幅率";
            seQuote.YValueMembers = "Quote";
            seQuote.MarkerStyle = MarkerStyle.Circle;
            
            ca.AxisX.LabelStyle.Format = "hh:mm:ss";
            ca.AxisX.Interval = 5;
            ca.AxisX.IntervalType = DateTimeIntervalType.Seconds;
            ca.AxisX.IntervalOffset = 5;

            ca.AxisX.Minimum = DateTime.Now.AddSeconds(-5).ToOADate();//new DateTime(2017, 12, 12, 9, 0, 0).ToOADate();
            ca.AxisX.Maximum = DateTime.Now.AddSeconds(50).ToOADate();//new DateTime(2017, 12, 12, 17, 0, 0).ToOADate();
            seQuote.XValueType = ChartValueType.DateTime;

            this.mainChart.Series.Add(seQuote);
        }
        /// <summary>
        /// N秒更新
        /// </summary>
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            //即時
            Model.Quotes quoteA = MainViewModel.Instance.QuotesList.Values.FirstOrDefault(x => x.SymbolNo == MainViewModel.Instance.SymbolNoA);
            Model.Quotes quoteB = MainViewModel.Instance.QuotesList.Values.FirstOrDefault(x => x.SymbolNo == MainViewModel.Instance.SymbolNoB);
            if (quoteA == null || quoteA.MatchPrice == 0 || quoteB == null || quoteB.MatchPrice == 0 || quoteA.Market != quoteB.Market)
            {
                return;
            }
            double quotesValue = 0d;
            switch (quoteA.Market)
            {
                case "期貨":
                case "選擇權":
                    {
                        var preQuotesA = Model.SymbolTaifexList.GetAllSymbolTaifexCollection().FirstOrDefault(x => x.SymbolNo == MainViewModel.Instance.SymbolNoA);
                        var preQuotesB = Model.SymbolTaifexList.GetAllSymbolTaifexCollection().FirstOrDefault(x => x.SymbolNo == MainViewModel.Instance.SymbolNoB);

                        quotesValue = (((double)(quoteA.MatchPrice - preQuotesA.ReferencePrice)) / 100d) - (((double)(quoteB.MatchPrice - preQuotesB.ReferencePrice)) / 100d);
                        break;
                    }
                case "上櫃":
                    {
                        var preQuotesA = Model.SymbolTpexList.GetAllSymbolTpexCollection().FirstOrDefault(x => x.SymbolNo == MainViewModel.Instance.SymbolNoA);
                        var preQuotesB = Model.SymbolTpexList.GetAllSymbolTpexCollection().FirstOrDefault(x => x.SymbolNo == MainViewModel.Instance.SymbolNoB);

                        quotesValue = (((double)(quoteA.MatchPrice - preQuotesA.ReferencePrice)) / 100d) - (((double)(quoteB.MatchPrice - preQuotesB.ReferencePrice)) / 100d);
                        break;
                    }
                case "上市":
                    {
                        var preQuotesA = Model.SymbolTseList.GetAllSymbolTseCollection().FirstOrDefault(x => x.SymbolNo == MainViewModel.Instance.SymbolNoA);
                        var preQuotesB = Model.SymbolTseList.GetAllSymbolTseCollection().FirstOrDefault(x => x.SymbolNo == MainViewModel.Instance.SymbolNoB);

                        quotesValue = (((double)(quoteA.MatchPrice - preQuotesA.ReferencePrice)) / 100d) - (((double)(quoteB.MatchPrice - preQuotesB.ReferencePrice)) / 100d);
                        break;
                    }
                default:
                    return;
            }

            this.mainChart.ChartAreas[0].AxisY.Interval = quotesValue / 100d;
            this.mainChart.ChartAreas[0].AxisY.IntervalOffset = quotesValue / 100d;
            this.mainChart.ChartAreas[0].AxisY.Minimum = (quotesValue - 1 <= - 10)? -10 : quotesValue -1;
            this.mainChart.ChartAreas[0].AxisY.Maximum = (quotesValue + 1 >= 10) ? 10 : quotesValue +1;

            this.mainChart.Series[0].Points.AddXY(DateTime.Now.ToOADate(), quotesValue);
            this.mainChart.ChartAreas[0].AxisX.Maximum = DateTime.Now.AddSeconds(10).ToOADate();

            switch(this.mainChart.Series[0].Points.Count)
            {
                case 50:
                this.mainChart.ChartAreas[0].AxisX.LabelStyle.Format = "hh:mm:ss";
                this.mainChart.ChartAreas[0].AxisX.Interval = 10;
                this.mainChart.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Seconds;
                this.mainChart.ChartAreas[0].AxisX.IntervalOffset = 10;
                    break;
                case 100:
                    this.mainChart.ChartAreas[0].AxisX.LabelStyle.Format = "hh:mm:ss";
                    this.mainChart.ChartAreas[0].AxisX.Interval = 20;
                    this.mainChart.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Seconds;
                    this.mainChart.ChartAreas[0].AxisX.IntervalOffset = 20;
                    break;
                case 150:
                    this.mainChart.ChartAreas[0].AxisX.LabelStyle.Format = "hh:mm:ss";
                    this.mainChart.ChartAreas[0].AxisX.Interval = 30;
                    this.mainChart.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Seconds;
                    this.mainChart.ChartAreas[0].AxisX.IntervalOffset = 30;
                    break;
                case 200:
                    this.mainChart.ChartAreas[0].AxisX.LabelStyle.Format = "hh:mm:ss";
                    this.mainChart.ChartAreas[0].AxisX.Interval = 40;
                    this.mainChart.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Seconds;
                    this.mainChart.ChartAreas[0].AxisX.IntervalOffset = 40;
                    break;
                case 250:
                    this.mainChart.ChartAreas[0].AxisX.LabelStyle.Format = "hh:mm:ss";
                    this.mainChart.ChartAreas[0].AxisX.Interval = 50;
                    this.mainChart.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Seconds;
                    this.mainChart.ChartAreas[0].AxisX.IntervalOffset = 50;
                    break;
                case 300:
                    this.mainChart.ChartAreas[0].AxisX.LabelStyle.Format = "hh:mm";
                    this.mainChart.ChartAreas[0].AxisX.Interval = 1;
                    this.mainChart.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Minutes;
                    this.mainChart.ChartAreas[0].AxisX.IntervalOffset = 1;
                    break;
                case 900:
                    this.mainChart.ChartAreas[0].AxisX.LabelStyle.Format = "hh:mm";
                    this.mainChart.ChartAreas[0].AxisX.Interval = 30;
                    this.mainChart.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Minutes;
                    this.mainChart.ChartAreas[0].AxisX.IntervalOffset = 30;
                    break;
                case 1800:
                    this.mainChart.ChartAreas[0].AxisX.LabelStyle.Format = "hh:mm";
                    this.mainChart.ChartAreas[0].AxisX.Interval = 1;
                    this.mainChart.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Hours;
                    this.mainChart.ChartAreas[0].AxisX.IntervalOffset = 1;
                    break;
            }

            //因為DataSource在Form Load就設了,所以這裡只要重新DataBind()就可以更新顯示資料,沒重DataBind之前,新資料不會顯示上去
            this.mainChart.DataBind();

            // Forcing the CommandManager to raise the RequerySuggested event
            CommandManager.InvalidateRequerySuggested();
        }
        #endregion
    }
}
