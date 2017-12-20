using MarketDataApi;
using System;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace EasyQuteChartExample.ViewModels
{
    public class MainViewModel: INotifyPropertyChanged
    {
        #region Fields
        private static MainViewModel _instance;
        private string _selectMarketA = "";
        private string _selectMarketB = "";
        private ObservableCollection<MarketDataApi.PacketPATS.Format2> _patsFormat2List = new ObservableCollection<MarketDataApi.PacketPATS.Format2>();
        private MdApi _api;
        private string _symbolNoA = "";
        private string _symbolNoB = "";
        private decimal _quotesA = 0;
        private decimal _quotesB = 0;
        private decimal _quotesAnswer = 0;
        #endregion

        public MainViewModel()
        {
            DefaultSettings.Instance.Initialize();//讀設定檔

            _api = new MdApi(DefaultSettings.Instance.SUP_IP, DefaultSettings.Instance.SUP_PORT, DefaultSettings.Instance.SERVICE_IP, DefaultSettings.Instance.SERVICE_PORT);
            _api.PatsFormat2Received += api_PatsFormat2Received; ; /// <- pats(行情)格式2回呼事件
        }

        #region prop
        /// <summary>
        /// 主畫面操作
        /// </summary>
        public static MainViewModel Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new MainViewModel();
                }
                return _instance;
            }
        }
        /// <summary>
        /// 行情API
        /// </summary>
        public MdApi MDAPI
        {
            get
            {
                return _api;
            }
        }
        /// <summary>
        /// 所選交易所.商品A
        /// </summary>
        public string SelectMarketA
        {
            get
            {
                return _selectMarketA;
            }
            set
            {
                _selectMarketA = value;
                OnPropertyChanged("SelectMarketA");
            }
        }
        /// <summary>
        /// 所選交易所.商品B
        /// </summary>
        public string SelectMarketB
        {
            get
            {
                return _selectMarketB;
            }
            set
            {
                _selectMarketB = value;
                OnPropertyChanged("SelectMarketB");
            }
        }
        /// <summary>
        /// 漲跌幅商品代碼左
        /// </summary>
        public string SymbolNoA
        {
            get
            {
                return _symbolNoA;
            }
            set
            {
                _symbolNoA = value;
                OnPropertyChanged("SymbolNoA");
            }
        }
        /// <summary>
        /// 漲跌幅商品代碼右
        /// </summary>
        public string SymbolNoB
        {
            get
            {
                return _symbolNoB;
            }
            set
            {
                _symbolNoB = value;
                OnPropertyChanged("SymbolNoB");
            }
        }
        /// <summary>
        /// 漲跌幅率左
        /// </summary>
        public decimal QuotesA
        {
            get
            {
                return _quotesA;
            }
            set
            {
                _quotesA = value;
                QuotesAnswer = _quotesA - _quotesB;
                OnPropertyChanged("QuotesA");
            }
        }
        /// <summary>
        /// 漲跌幅率右
        /// </summary>
        public decimal QuotesB
        {
            get
            {
                return _quotesB;
            }
            set
            {
                _quotesB = value;
                QuotesAnswer = _quotesA - _quotesB;
                OnPropertyChanged("QuotesB");
            }
        }
        /// <summary>
        /// 漲跌幅率差(左-右)
        /// </summary>
        public decimal QuotesAnswer
        {
            get
            {
                return _quotesAnswer;
            }
            set
            {
                _quotesAnswer = value;
                OnPropertyChanged("QuotesAnswer");
            }
        }
        /// <summary>
        /// 訂閱PATS成交資料
        /// </summary>
        public ObservableCollection<MarketDataApi.PacketPATS.Format2> PatsFormat2List
        {
            get
            {
                return _patsFormat2List;
            }
            set
            {
                _patsFormat2List = value;
                OnPropertyChanged("PatsFormat2List");
            }
        }
        /// <summary>
        /// PATS商品列表
        /// </summary>
        public ObservableCollection<string> PatsSymbolList
        {
            get
            {
                return new ObservableCollection<string>( DefaultSettings.Instance.PATS_SYMBOL);
            }
        }
        #endregion

        #region Event    
        //------------------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// pats回呼事件
        /// </summary>
        private void api_PatsFormat2Received(object sender, MdApi.PatsFormat2ReceivedEventArgs e)
        {
            App.Current.Dispatcher.Invoke((Action)(() =>
            {
                _patsFormat2List.Insert(0, e.PacketData);
            }));
        }
        #endregion
        
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string name)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
        #endregion
    }
}
