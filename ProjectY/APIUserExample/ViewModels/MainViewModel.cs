using MarketDataApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Diagnostics;
using System.ComponentModel;
using System.Collections.ObjectModel;
using CommonLibrary.Model;
using System.IO;
using System.Collections.Concurrent;
using System.Data;
using System.Windows.Data;

namespace APIUserExample.ViewModels
{
    public class MainViewModel: INotifyPropertyChanged
    {
        #region Fields
        public static readonly List<string> MarketList = new List<string>() { "0-上市", "1-上櫃", "2-期貨AM盤", "3-選擇權AM盤", "4-期貨PM盤", "5-選擇權PM盤" };
        public static readonly List<string> TypeList = new List<string>() { "I010", "I020", "I080", "I022", "I082", "1", "6", "17" };

        private static MainViewModel _instance;
        private string _ipAddress = "203.66.93.83";
        private string _ipPort = "6688";
        private string _reqipAddress = "203.66.93.83";
        private string _reqipPort = "5588";
        private string _selectMarket = "0-上市";
        private string _selectType = "6";
        private string _symbolNo = "";
        private string _selectSubscribe = string.Empty;
        private ObservableCollection<string> _subscribeList = new ObservableCollection<string>();
        private ObservableCollection<Quotes> _quotesList = new ObservableCollection<Quotes>();
        private ObservableCollection<Quotes> _quotesSnapshotList = new ObservableCollection<Quotes>();
        private ObservableCollection<SymbolTse> _symbolTseDictionary = new ObservableCollection<SymbolTse>();
        private ObservableCollection<SymbolTpex> _symbolTpexDictionary = new ObservableCollection<SymbolTpex>();
        private ObservableCollection<SymbolTaifex> _symbolTaifexDictionary = new ObservableCollection<SymbolTaifex>();
        MdApi api;
        private long _gridSeq = 1;
        
        private ObservableCollection<string> _statusMessageList = new ObservableCollection<string>();
        #endregion

        public MainViewModel()
        {
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
        /// 行情IP位址
        /// </summary>
        public string IPAddress
        {
            get
            {
                return _ipAddress;
            }
            set
            {
                _ipAddress = value;
            }
        }
        /// <summary>
        /// 行情IP port
        /// </summary>
        public string IPPort
        {
            get
            {
                return _ipPort;
            }
            set
            {
                _ipPort = value;
            }
        }
        /// <summary>
        /// 要求IP位址
        /// </summary>
        public string ReqIPAddress
        {
            get
            {
                return _reqipAddress;
            }
            set
            {
                _reqipAddress = value;
            }
        }
        /// <summary>
        /// 要求IP port
        /// </summary>
        public string ReqIPPort
        {
            get
            {
                return _reqipPort;
            }
            set
            {
                _reqipPort = value;
            }
        }
        /// <summary>
        /// 所選市場別(上市、上櫃、日選、夜選、日期、夜期)
        /// </summary>
        public string SelectMarket
        {
            get
            {
                return _selectMarket;
            }
            set
            {
                _selectMarket = value;
                OnPropertyChanged("SelectMarket");
            }
        }
        /// <summary>
        /// 所選類型(I020期權成交、I080期權委買賣、6証卷、17權証...)
        /// </summary>
        public string SelectType
        {
            get
            {
                return _selectType;
            }
            set
            {
                _selectType = value;
                OnPropertyChanged("SelectType");
            }
        }
        /// <summary>
        /// 商品代碼
        /// </summary>
        public string SymbolNo
        {
            get
            {
                return _symbolNo;
            }
            set
            {
                _symbolNo = value;
                OnPropertyChanged("SymbolNo");
            }
        }
        /// <summary>
        /// 訂閱行情內容
        /// </summary>
        public ObservableCollection<Quotes> QuotesList
        {
            get
            {
                return _quotesList;
            }
            set
            {
                _quotesList = value;
                OnPropertyChanged("QuotesList");
            }
        }
        /// <summary>
        /// 快照行情內容
        /// </summary>
        public ObservableCollection<Quotes> QuotesSnapshotList
        {
            get
            {
                return _quotesSnapshotList;
            }
            set
            {
                _quotesSnapshotList = value;
                OnPropertyChanged("QuotesSnapshotList");
            }
        }        
        /// <summary>
        /// 訂閱項目
        /// </summary>
        public ObservableCollection<string> SubscribeList
        {
            get
            {
                return _subscribeList;
            }
            set
            {
                _subscribeList = value;
                OnPropertyChanged("SubscribeList");
            }
        }
        /// <summary>
        /// 所選訂閱項目
        /// </summary>
        public string SelectSubscribe
        {
            get
            {
                return _selectSubscribe;
            }
            set
            {
                try
                {
                    if (string.IsNullOrEmpty(value) == false)
                    {
                        SelectMarket = value.Split(';')[0];
                        SelectType = value.Split(';')[1];
                        SymbolNo = value.Split(';')[2];
                    }
                }
                catch(Exception)
                { }
                _selectSubscribe = value;
            }
        }
        /// <summary>
        /// 現貨上市商品資料
        /// </summary>
        public ObservableCollection<SymbolTse> SymbolTseDictionary
        {
            get
            {
                return _symbolTseDictionary;
            }
            set
            {
                _symbolTseDictionary = value;
                OnPropertyChanged("SymbolTseDictionary");
            }
        }
        /// <summary>
        /// 現貨上櫃商品資料
        /// </summary>
        public ObservableCollection<SymbolTpex> SymbolTpexDictionary
        {
            get
            {
                return _symbolTpexDictionary;
            }
            set
            {
                _symbolTpexDictionary = value;
                OnPropertyChanged("SymbolTpexDictionary");
            }
        }
        /// <summary>
        /// 期貨商品資料
        /// </summary>
        public ObservableCollection<SymbolTaifex> SymbolTaifexDictionary
        {
            get
            {
                return _symbolTaifexDictionary;
            }
            set
            {
                _symbolTaifexDictionary = value;
                OnPropertyChanged("SymbolTaifexDictionary");
            }
        }        
        /// <summary>
        /// 狀態訊息
        /// </summary>
        public ObservableCollection<string> StatusMessageList
        {
            get
            {
                return _statusMessageList;
            }
            set
            {
                _statusMessageList = value;
                OnPropertyChanged("StatusMessageList");
            }
        }
        #endregion

        #region Command
        private ICommand _conncetCommand;
        public ICommand ConncetCommand
        {
            get
            {
                if (_conncetCommand == null)
                {
                    _conncetCommand = new RelayCommand(
                        ConncetMarketData
                    );
                }
                return _conncetCommand;
            }
        }

        private ICommand _supscribeCommand;
        public ICommand SupscribeCommand
        {
            get
            {
                if (_supscribeCommand == null)
                {
                    _supscribeCommand = new RelayCommand(
                        SupscribeSymbol
                    );
                }
                return _supscribeCommand;
            }
        }

        private ICommand _unsupscribeCommand;
        public ICommand UnsupscribeCommand
        {
            get
            {
                if (_unsupscribeCommand == null)
                {
                    _unsupscribeCommand = new RelayCommand(
                        UnSupscribeSymbol
                    );
                }
                return _unsupscribeCommand;
            }
        }

        private ICommand _clearContentCommand;
        public ICommand ClearContentCommand
        {
            get
            {
                if (_clearContentCommand == null)
                {
                    _clearContentCommand = new RelayCommand(
                        clearContent
                    );
                }
                return _clearContentCommand;
            }
        }

        private ICommand _getSnapshotCommand;
        public ICommand GetSnapshotCommand
        {
            get
            {
                if (_getSnapshotCommand == null)
                {
                    _getSnapshotCommand = new RelayCommand(
                        GetSnapshot
                    );
                }
                return _getSnapshotCommand;
            }
        }

        private ICommand _getPreSymbolCommand;
        public ICommand GetPreSymbolCommand
        {
            get
            {
                if (_getPreSymbolCommand == null)
                {
                    _getPreSymbolCommand = new RelayCommand(
                        GetPreSymbol
                    );
                }
                return _getPreSymbolCommand;
            }
        }


        /// <summary>
        /// 連接行情伺服器
        /// </summary>
        private void ConncetMarketData()
        {
            try
            {
                api = new MdApi(IPAddress, int.Parse(IPPort), ReqIPAddress, int.Parse(ReqIPPort));
                api.TaifexI020Received += api_TaifexI020Received;  /// <- 期貨I020[成交]回呼事件
                api.TaifexI022Received += api_TaifexI022Received;  /// <- 期貨I022試撮[成交]回呼事件
                api.TaifexI080Received += api_TaifexI080Received;  /// <- 期貨I080[委買委賣]回呼事件
                api.TaifexI082Received += api_TaifexI082Received;  /// <- 期貨I082試撮回呼事件
                api.TseFormat6Received += api_TseFormat6Received;  /// <- 上市現貨格式6(Format6)回呼事件
                api.TpexFormat6Received += api_TpexFormat6Received; /// <- 上櫃現貨格式6(Format6)回呼事件
                api.TseFormat17Received += api_TseFormat17Received;  /// <- 上市現貨格式17(Format17)回呼事件
                api.TpexFormat17Received += api_TpexFormat17Received; /// <- 上櫃現貨格式17(Format17)回呼事件
                api.TaifexI010Received += api_TaifexI010Received; /// <- 期貨I010[盘前]回呼事件
                api.TseFormat1Received += api_TseFormat1Received; /// <- 上櫃現貨格式1(盘前)回呼事件
                api.TpexFormat1Received += api_TpexFormat1Received;/// <- 上市現貨格式1(盘前)回呼事件
                StatusMessageList.Insert(0, DateTime.Now.ToString("HH:mm:ss:ttt") + "    " + "連接" + IPAddress + ":" + IPPort + "," + ReqIPAddress + ":" + ReqIPPort);
            }
            catch (Exception err)
            {
                StatusMessageList.Insert(0, DateTime.Now.ToString("HH:mm:ss:ttt") + "    " + string.Format("ConncetMarketData(): ErrMsg = {0}.", err.Message));
            }
        }
        //------------------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// 註冊報價
        /// </summary>
        private void SupscribeSymbol()
        {
            if (api == null)
            {
                MessageBox.Show("未連接或連接錯誤");
                return;
            }

            string subSymbol = SelectMarket + ";" + SelectType + ";" + SymbolNo;
            if (string.IsNullOrEmpty(SymbolNo))
            {
                api.Sub(Rtn_adapterCode(SelectMarket), SelectType);
                SubscribeList.Insert(0, SelectMarket + ";" + SelectType);
            }
            else
            {
                api.Sub(Rtn_adapterCode(SelectMarket), SelectType, SymbolNo);
                SubscribeList.Insert(0, subSymbol);
            }
            StatusMessageList.Insert(0, DateTime.Now.ToString("HH:mm:ss:ttt") + "    " + "訂閱:" + subSymbol);
        }
        //------------------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// 取消註冊報價
        /// </summary>
        private void UnSupscribeSymbol()
        {
            if(string.IsNullOrEmpty( SelectMarket) || string.IsNullOrEmpty(SelectType))
            {
                return;
            }
            if (string.IsNullOrEmpty(SymbolNo))
            {
                api.UnSub(Rtn_adapterCode(SelectMarket), SelectType);

                SubscribeList.Remove(SelectSubscribe);
                StatusMessageList.Insert(0, DateTime.Now.ToString("HH:mm:ss:ttt") + "    " + "取消訂閱:" + SelectMarket + ";" + SelectType);
            }
            else
            {
                api.UnSub(Rtn_adapterCode(SelectMarket), SelectType, SymbolNo);

                SubscribeList.Remove(SelectSubscribe);
                StatusMessageList.Insert(0, DateTime.Now.ToString("HH:mm:ss:ttt") + "    " + "取消訂閱:" + SelectMarket + ";" + SelectType + ";" + SymbolNo);
                SymbolNo = string.Empty;
            }
            SelectSubscribe = SubscribeList.FirstOrDefault();
        }
        //------------------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// 清除內容
        /// </summary>
        private void clearContent()
        {
            QuotesList.Clear();
            _gridSeq = 1;
        }
        //------------------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// 取得行情快照(現/期貨盤前、試撮、盤中)
        /// </summary>
        private void GetSnapshot()
        {
            if (api == null || string.IsNullOrEmpty(SelectMarket) || string.IsNullOrEmpty(SelectType) || string.IsNullOrEmpty(SymbolNo))
            {
                MessageBox.Show("連接錯誤或無商品資料");
                return;
            }
            string subSymbol = SelectMarket + ";" + SelectType + ";" + SymbolNo;
            var ret = api.GetSnapshot(Rtn_adapterCode(SelectMarket), SelectType, SymbolNo);

            if (ret == null)
            {
                StatusMessageList.Insert(0, DateTime.Now.ToString("HH:mm:ss:ttt") + "    " + "取得快照失敗:" + subSymbol);
                return;
            }
            Quotes model = new Quotes();
            switch (SelectMarket.Substring(0, 1))
            {
                case "0"://上市
                    switch(SelectType)
                    {
                        case "6"://一般
                            model.SetTseData(0, ret as MarketDataApi.PacketTSE.Format6);
                            QuotesSnapshotList.Insert(0, model);
                            break;
                        case "17"://權証
                            model.SetTseData(0, ret as MarketDataApi.PacketTSE.Format17);
                            QuotesSnapshotList.Insert(0, model);
                            break;
                    }
                    break;
                case "1"://上櫃
                    switch (SelectType)
                    {
                        case "6"://一般
                            model.SetTpexData(0, ret as MarketDataApi.PacketTPEX.Format6);
                            QuotesSnapshotList.Insert(0, model);
                            break;
                        case "17"://權証
                            model.SetTpexData(0, ret as MarketDataApi.PacketTPEX.Format17);
                            QuotesSnapshotList.Insert(0, model);
                            break;
                    }
                    break;
                case "2"://日期
                case "3"://日選
                case "4"://夜期
                case "5"://夜選
                    switch (SelectType)
                    {
                        case "I020"://成交
                            model.SetI020Data(0, ret as MarketDataApi.PacketTAIFEX.I020);
                            QuotesSnapshotList.Insert(0, model);
                            break;
                        case "I022"://試撮成交
                            model.SetI022Data(0, ret as MarketDataApi.PacketTAIFEX.I022);
                            QuotesSnapshotList.Insert(0, model);
                            break;
                        case "I080"://5檔
                            model.SetI080Data(0, ret as MarketDataApi.PacketTAIFEX.I080);
                            QuotesSnapshotList.Insert(0, model);
                            break;
                        case "I082"://試撮5檔
                            model.SetI082Data(0, ret as MarketDataApi.PacketTAIFEX.I082);
                            QuotesSnapshotList.Insert(0, model);
                            break;
                    }
                    break;
            }
            StatusMessageList.Insert(0, DateTime.Now.ToString("HH:mm:ss:ttt") + "    " + "取得快照成功:" + subSymbol);
        }
        /// <summary>
        /// 取得盤前資料
        /// </summary>
        private void GetPreSymbol()
        {
            foreach (MarketDataApi.PacketTAIFEX.I010 data in api.GetContracts(AdapterCode.TAIFEX_FUTURES_DAY))
            {
                SymbolTaifexList.AddSymbolTalfexData(new SymbolTaifex(data));
            }
            foreach (MarketDataApi.PacketTAIFEX.I010 data in api.GetContracts(AdapterCode.TAIFEX_OPTIONS_DAY))
            {
                SymbolTaifexList.AddSymbolTalfexData(new SymbolTaifex(data));
            }
            foreach (MarketDataApi.PacketTPEX.Format1 data in api.GetContracts(AdapterCode.TPEX))
            {
                SymbolTpexList.AddSymbolTpexData(new SymbolTpex(data));
            }
            foreach (MarketDataApi.PacketTSE.Format1 data in api.GetContracts(AdapterCode.TSE))
            {
                SymbolTseList.AddSymbolTseData(new SymbolTse(data));
            }
            SymbolTaifexDictionary = SymbolTaifexList.GetAllSymbolTaifexCollection();
            SymbolTaifexDictionary = SymbolTaifexList.GetAllSymbolTaifexCollection();
            SymbolTseDictionary = SymbolTseList.GetAllSymbolTseCollection();
            SymbolTpexDictionary = SymbolTpexList.GetAllSymbolTpexCollection();
        }
        #endregion

        #region Event
        //------------------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// 期貨I080[委買委賣]回呼事件
        /// </summary>
        void api_TaifexI080Received(object sender, MdApi.TaifexI080ReceivedEventArgs e)
        {
            /// <- Bid Asl 0~1表示1檔~5檔
            App.Current.Dispatcher.Invoke((Action)(() =>
            {
                Quotes model = new Quotes();
                model.SetI080Data(_gridSeq, e.PacketData);
                QuotesList.Insert(0, model);
                _gridSeq++;
            }));
        }
        //------------------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// 期貨I020[成交]回呼事件
        /// </summary>
        void api_TaifexI020Received(object sender, MdApi.TaifexI020ReceivedEventArgs e)
        {
            App.Current.Dispatcher.Invoke((Action)(() =>
            {
                Quotes model = new Quotes();
                model.SetI020Data(_gridSeq, e.PacketData);
                QuotesList.Insert(0, model);
                _gridSeq++;
            }));
        }
        //------------------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// 期貨I082
        /// </summary>
        void api_TaifexI082Received(object sender, MdApi.TaifexI082ReceivedEventArgs e)
        {
            App.Current.Dispatcher.Invoke((Action)(() =>
            {
                Quotes model = new Quotes();
                model.SetI082Data(_gridSeq, e.PacketData);
                QuotesList.Insert(0, model);
                _gridSeq++;
            }));
        }
        //------------------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// 期貨I022
        /// </summary>
        void api_TaifexI022Received(object sender, MdApi.TaifexI022ReceivedEventArgs e)
        {
            App.Current.Dispatcher.Invoke((Action)(() =>
            {
                Quotes model = new Quotes();
                model.SetI022Data(_gridSeq, e.PacketData);
                QuotesList.Insert(0, model);
                _gridSeq++;
            }));
        }
        //------------------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// 上櫃現貨格式6回呼事件
        /// </summary>
        void api_TpexFormat6Received(object sender, MdApi.TpexFormat6ReceivedEventArgs e)
        {
            App.Current.Dispatcher.Invoke((Action)(() =>
            {
                Quotes model = new Quotes();
                model.SetTpexData(_gridSeq,  e.PacketData);
                QuotesList.Insert(0, model);
                _gridSeq++;
            }));
        }
        //------------------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// 上市現貨格式6回呼事件
        /// </summary>
        void api_TseFormat6Received(object sender, MdApi.TseFormat6ReceivedEventArgs e)
        {
            App.Current.Dispatcher.Invoke((Action)(() =>
            {
                Quotes model = new Quotes();
                model.SetTseData(_gridSeq, e.PacketData);
                QuotesList.Insert(0,model);
                _gridSeq++;
            }));
        }
        //------------------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// 上櫃現貨格式17回呼事件
        /// </summary>
        void api_TpexFormat17Received(object sender, MdApi.TpexFormat17ReceivedEventArgs e)
        {
            App.Current.Dispatcher.Invoke((Action)(() =>
            {
                Quotes model = new Quotes();
                model.SetTpexData(_gridSeq, e.PacketData);
                QuotesList.Insert(0, model);
                _gridSeq++;
            }));
        }
        //------------------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// 上市現貨格式17回呼事件
        /// </summary>
        void api_TseFormat17Received(object sender, MdApi.TseFormat17ReceivedEventArgs e)
        {
            App.Current.Dispatcher.Invoke((Action)(() =>
            {
                Quotes model = new Quotes();
                model.SetTseData(_gridSeq, e.PacketData);
                QuotesList.Insert(0, model);
                _gridSeq++;
            }));
        }        
        //------------------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// 上市現貨格式1回呼事件
        /// </summary>
        void api_TseFormat1Received(object sender, MdApi.TseFormat1ReceivedEventArgs e)
        {
            App.Current.Dispatcher.Invoke((Action)(() =>
            {
            }));
        }
        //------------------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// 上櫃現貨格式1回呼事件
        /// </summary>
        void api_TpexFormat1Received(object sender, MdApi.TpexFormat1ReceivedEventArgs e)
        {
            App.Current.Dispatcher.Invoke((Action)(() =>
            {
            }));
        }
        //------------------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// 期貨I010[資訊]回呼事件
        /// </summary>
        void api_TaifexI010Received(object sender, MdApi.TaifexI010ReceivedEventArgs e)
        {
            App.Current.Dispatcher.Invoke((Action)(() =>
            {
            }));
        }
        //------------------------------------------------------------------------------------------------------------------------------------------
        #endregion

        #region func
        /// <summary>
        /// AdapterCode轉換
        /// </summary>
        AdapterCode Rtn_adapterCode(string Str)
        {
            AdapterCode adapterCode = AdapterCode.TSE; ;
            switch (Str.Substring(0, 1))
            {
                case "0":
                    adapterCode = AdapterCode.TSE; break;                  /// <- 上市
                case "1":
                    adapterCode = AdapterCode.TPEX; break;                 /// <- 上櫃
                case "2":
                    adapterCode = AdapterCode.TAIFEX_FUTURES_DAY; break;   /// <- 期貨AM盤
                case "3":
                    adapterCode = AdapterCode.TAIFEX_OPTIONS_DAY; break;   /// <- 選擇權AM盤
                case "4":
                    adapterCode = AdapterCode.TAIFEX_FUTURES_NIGHT; break; /// <- 期貨PM盤
                case "5":
                    adapterCode = AdapterCode.TAIFEX_OPTIONS_NIGHT; break; /// <- 選擇權PM盤
                default:
                    adapterCode = AdapterCode.TSE;
                    break;
            }
            return adapterCode;
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
