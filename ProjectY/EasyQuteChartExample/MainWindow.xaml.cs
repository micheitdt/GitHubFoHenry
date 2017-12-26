using Common;
using EasyQuteChartExample.ViewModels;
using MarketDataApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace EasyQuteChartExample
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer _dispatcherTimer = new DispatcherTimer();
        private double _maxValue = 0.01;
        private double _minValue = -0.01;
        private MarketDataApi.PacketPATS.Format1 preQuotesA;
        private MarketDataApi.PacketPATS.Format1 preQuotesB;

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = MainViewModel.Instance;
            this.Closing += MainWindow_Closing;

            CheckChartAreasData();
        }

        void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
        }
        /// <summary>
        /// 檢查圖表是否完成
        /// </summary>
        private void CheckChartAreasData()
        {
            _dispatcherTimer.Stop();
            InitSymbolA();
            InitSymbolB();
            //開始畫圖表
            if (this.mainChart.ChartAreas.Count == 0)
            {
                SetChart();

                _dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
                _dispatcherTimer.Interval = new TimeSpan(0, 0, 5);
            }
            else
            {
                this.mainChart.ChartAreas.Clear();
                this.mainChart.Legends.Clear();
                this.mainChart.Series.Clear();
                SetChart();
            }
            this.mainChart.DataBind();//這時候先DataBind()是為了顯示空白的圖表

            _dispatcherTimer.Start();
        }

        private bool InitSymbolA()
        {            
            preQuotesA = MainViewModel.Instance.MDAPI.GetSnapshot(AdapterCode.GLOBAL_PATS, "1", MainViewModel.Instance.SymbolNoA) as MarketDataApi.PacketPATS.Format1;
            MarketDataApi.PacketPATS.Format2 ret2 = MainViewModel.Instance.MDAPI.GetSnapshot(AdapterCode.GLOBAL_PATS, "2", MainViewModel.Instance.SymbolNoA) as MarketDataApi.PacketPATS.Format2;
            if (preQuotesA == null || ret2 == null || preQuotesA.OpeningPrice == 0)
            {
                return false;
            }

            MainViewModel.Instance.QuotesA = 0;
            //MainViewModel.Instance.MDAPI.Sub(AdapterCode.GLOBAL_PATS, "1", MainViewModel.Instance.SymbolNoA);
            MainViewModel.Instance.MDAPI.Sub(AdapterCode.GLOBAL_PATS, "2", MainViewModel.Instance.SymbolNoA);

            MainViewModel.Instance.QuotesA = ((((decimal)(ret2.LastPrice - preQuotesA.OpeningPrice))) / ((decimal)preQuotesA.OpeningPrice));
            return true;
        }

        private bool InitSymbolB()
        {
            preQuotesB = MainViewModel.Instance.MDAPI.GetSnapshot(AdapterCode.GLOBAL_PATS, "1", MainViewModel.Instance.SymbolNoB) as MarketDataApi.PacketPATS.Format1;
            MarketDataApi.PacketPATS.Format2 ret2 = MainViewModel.Instance.MDAPI.GetSnapshot(AdapterCode.GLOBAL_PATS, "2", MainViewModel.Instance.SymbolNoB) as MarketDataApi.PacketPATS.Format2;
            if (preQuotesB == null || ret2 == null || preQuotesB.OpeningPrice == 0)
            {
                return false;
            }

            MainViewModel.Instance.QuotesB = 0;
            //MainViewModel.Instance.MDAPI.Sub(AdapterCode.GLOBAL_PATS, "1", MainViewModel.Instance.SymbolNoB);
            MainViewModel.Instance.MDAPI.Sub(AdapterCode.GLOBAL_PATS, "2", MainViewModel.Instance.SymbolNoB);

            MainViewModel.Instance.QuotesB = ((((decimal)(ret2.LastPrice - preQuotesB.OpeningPrice))) / ((decimal)preQuotesB.OpeningPrice));
            return true;
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

            this.mainChart.ChartAreas[0].AxisY.Interval = 0.01;
            this.mainChart.ChartAreas[0].AxisY.IntervalOffset = 0.01;
            this.mainChart.ChartAreas[0].AxisY.Minimum = -0.1;
            this.mainChart.ChartAreas[0].AxisY.Maximum = 0.1;
            this.mainChart.ChartAreas[0].AxisY.LabelStyle.Format = "P2";

            ca.AxisX.LabelStyle.Format = "hh:mm:ss";
            ca.AxisX.Interval = 5;
            ca.AxisX.IntervalType = DateTimeIntervalType.Seconds;
            ca.AxisX.IntervalOffset = 5;
            ca.AxisX.Minimum = DateTime.Now.ToOADate();//new DateTime(2017, 12, 12, 9, 0, 0).ToOADate();
            ca.AxisX.Maximum = DateTime.Now.AddSeconds(50).ToOADate();//new DateTime(2017, 12, 12, 17, 0, 0).ToOADate();
            seQuote.XValueType = ChartValueType.DateTime;

            this.mainChart.Series.Add(seQuote);
        }
        /// <summary>
        /// N秒更新
        /// </summary>
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                //即時
                MarketDataApi.PacketPATS.Format2 quoteA = MainViewModel.Instance.PatsFormat2List.FirstOrDefault(x => string.Format("{0}.{1}.{2}",x.ExchangeNo,x.CommodityNo,x.ContractDate) == MainViewModel.Instance.SymbolNoA);
                MarketDataApi.PacketPATS.Format2 quoteB = MainViewModel.Instance.PatsFormat2List.FirstOrDefault(x => string.Format("{0}.{1}.{2}", x.ExchangeNo, x.CommodityNo, x.ContractDate) == MainViewModel.Instance.SymbolNoB);
                if (quoteA == null || quoteA.LastPrice == 0)
                {
                    Utility.SaveLog(DateTime.Now.ToString("HH:mm:ss:ttt") + "    " + "比對商品A錯誤或未有商品資料");
                    return;
                }
                if (quoteB == null || quoteB.LastPrice == 0)
                {
                    Utility.SaveLog(DateTime.Now.ToString("HH:mm:ss:ttt") + "    " + "比對商品B錯誤或未有商品資料");
                    return;
                }
                decimal quotesValue = 0;
                MainViewModel.Instance.QuotesA = ((((decimal)(quoteA.LastPrice - preQuotesA.OpeningPrice))) / ((decimal)preQuotesA.OpeningPrice));
                MainViewModel.Instance.QuotesB = ((((decimal)(quoteB.LastPrice - preQuotesB.OpeningPrice))) / ((decimal)preQuotesB.OpeningPrice));
                quotesValue = MainViewModel.Instance.QuotesA - MainViewModel.Instance.QuotesB;

                var data = Math.Round((double)quotesValue, 2);
                _minValue = Math.Round((_minValue < (data - 0.002)) ? _minValue : (data - 0.002), 2);
                _maxValue = Math.Round((_maxValue < (data + 0.002)) ? (data + 0.002) : _maxValue, 2);
                this.mainChart.ChartAreas[0].AxisY.Minimum = _minValue;
                this.mainChart.ChartAreas[0].AxisY.Maximum = _maxValue;
                this.mainChart.ChartAreas[0].AxisY.Interval = 0.01;
                this.mainChart.ChartAreas[0].AxisY.IntervalOffset = 0.01;
                if ((_maxValue - _minValue) != 0)
                {
                    double tick = Math.Round((_maxValue - _minValue) / 5, 2);
                    this.mainChart.ChartAreas[0].AxisY.Interval = tick;
                    this.mainChart.ChartAreas[0].AxisY.IntervalOffset = tick;
                }

                this.mainChart.Series[0].Points.AddXY(DateTime.Now.ToOADate(), quotesValue);
                this.mainChart.ChartAreas[0].AxisX.Maximum = DateTime.Now.AddSeconds(10).ToOADate();

                switch (this.mainChart.Series[0].Points.Count)
                {
                    case 20:
                        this.mainChart.ChartAreas[0].AxisX.LabelStyle.Format = "hh:mm:ss";
                        this.mainChart.ChartAreas[0].AxisX.Interval = 10;
                        this.mainChart.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Seconds;
                        this.mainChart.ChartAreas[0].AxisX.IntervalOffset = 10;
                        break;
                    case 60:
                        this.mainChart.ChartAreas[0].AxisX.LabelStyle.Format = "hh:mm:ss";
                        this.mainChart.ChartAreas[0].AxisX.Interval = 20;
                        this.mainChart.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Seconds;
                        this.mainChart.ChartAreas[0].AxisX.IntervalOffset = 20;
                        break;
                    case 90:
                        this.mainChart.ChartAreas[0].AxisX.LabelStyle.Format = "hh:mm:ss";
                        this.mainChart.ChartAreas[0].AxisX.Interval = 30;
                        this.mainChart.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Seconds;
                        this.mainChart.ChartAreas[0].AxisX.IntervalOffset = 30;
                        break;
                    case 120:
                        this.mainChart.ChartAreas[0].AxisX.LabelStyle.Format = "hh:mm:ss";
                        this.mainChart.ChartAreas[0].AxisX.Interval = 40;
                        this.mainChart.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Seconds;
                        this.mainChart.ChartAreas[0].AxisX.IntervalOffset = 40;
                        break;
                    case 150:
                        this.mainChart.ChartAreas[0].AxisX.LabelStyle.Format = "hh:mm:ss";
                        this.mainChart.ChartAreas[0].AxisX.Interval = 50;
                        this.mainChart.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Seconds;
                        this.mainChart.ChartAreas[0].AxisX.IntervalOffset = 50;
                        break;
                    case 180:
                        this.mainChart.ChartAreas[0].AxisX.LabelStyle.Format = "hh:mm";
                        this.mainChart.ChartAreas[0].AxisX.Interval = 1;
                        this.mainChart.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Minutes;
                        this.mainChart.ChartAreas[0].AxisX.IntervalOffset = 1;
                        break;
                    case 360:
                        this.mainChart.ChartAreas[0].AxisX.LabelStyle.Format = "hh:mm";
                        this.mainChart.ChartAreas[0].AxisX.Interval = 5;
                        this.mainChart.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Minutes;
                        this.mainChart.ChartAreas[0].AxisX.IntervalOffset = 5;
                        break;
                    case 1080:
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
            catch (Exception ex)
            {
                Utility.SaveLog(DateTime.Now.ToString("HH:mm:ss:ttt") + "    " + "圖表更新錯誤:" + ex.Message);
            }
        }
    }
}
