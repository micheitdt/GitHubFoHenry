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
using MarketDataApiExample.Model;
using NLog;
using System.IO;
using static CommonLibrary.Utility;
using System.Collections.Concurrent;
using System.Data;

namespace MarketDataApiExample.ViewModels
{
    public class MainViewModel: INotifyPropertyChanged
    {
        #region Logger
        /// <summary>
        /// 記錄器
        /// </summary>
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        #endregion Logger

        public MainViewModel()
        {
            //LoadTSEData();
            //LoadTPEXData();
            //LoadTAIFEXData();
            ////ConcurrentDictionary<string, SymbolTse>
            //foreach (KeyValuePair<string,SymbolTse> data in SymbolTseList.AllSymbolTseList)
            //{
            //    api.Sub(Rtn_adapterCode(SelectMarket), SelectType, SymbolNo);
            //}
            //foreach (KeyValuePair<string, SymbolTpex> data in SymbolTpexList.AllSymbolTpexList)
            //{
            //}
            //foreach (KeyValuePair<string, SymbolTaifex> data in SymbolTaifexList.AllSymbolTaifexList)
            //{
            //}

        }

        #region Fields
        public static readonly List<string> MarketList = new List<string>() { "0-上市", "1-上櫃", "2-期貨AM盤", "3-選擇權AM盤", "4-期貨PM盤", "5-選擇權PM盤", "6-PATS" };
        public static readonly List<string> TypeList = new List<string>() { "I010", "I020", "I080", "1", "6", "0" };

        private static MainViewModel _instance;
        private string _ipAddress = "10.214.217.45";//"127.0.0.1";
        private string _ipPort = "6688";
        private string _selectMarket = "2-期貨AM盤";//"6-PATS";
        private string _selectType = "I020";
        private string _symbolNo = "";//CME_ES.GE0.DEC07//all
        private string _selectSubscribe = string.Empty;
        private ObservableCollection<string> _subscribeList = new ObservableCollection<string>();
        private ObservableCollection<Quotes> _quotesList = new ObservableCollection<Quotes>();
        private string _tSESymbolNoFilter = string.Empty;
        private string _tPEXSymbolNoFilter = string.Empty;
        private string _tAIFEXSymbolNoFilter = string.Empty;
        private SymbolTseList _symbolTseDictionary = new SymbolTseList();
        private SymbolTpexList _symbolTpexDictionary = new SymbolTpexList();
        private SymbolTaifexList _symbolTaifexDictionary = new SymbolTaifexList();
        MarketDataApi.MarketDataApi api;
        private long _gridSeq = 1;
        
        private ObservableCollection<MarketDataApi.PacketPATS.Format0> _patsFormat0List = new ObservableCollection<MarketDataApi.PacketPATS.Format0>();
        private ObservableCollection<MarketDataApi.PacketPATS.Format1> _patsFormat1List = new ObservableCollection<MarketDataApi.PacketPATS.Format1>();
        private ObservableCollection<string> _statusMessageList = new ObservableCollection<string>();

        //行情資料
        private static DataTable _quotesDT;
        public static DataTable QuotesDT
        {
            get { return _quotesDT; }
            set { _quotesDT = value; }
        }
        #endregion

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
        /// IP位址
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
        /// IP port
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
        /// 所選類型(I020期權成交、I080期權委買賣、6証卷成交)
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
        public SymbolTseList SymbolTseDictionary
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
        public SymbolTpexList SymbolTpexDictionary
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
        public SymbolTaifexList SymbolTaifexDictionary
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
        /// 篩選期貨
        /// </summary>
        public string TAIFEXSymbolNoFilter
        {
            get
            {
                return _tAIFEXSymbolNoFilter;
            }
            set
            {
                _tAIFEXSymbolNoFilter = value;
                OnPropertyChanged("TAIFEXSymbolNoFilter");
            }
        }
        /// <summary>
        /// 篩選現貨上市
        /// </summary>
        public string TSESymbolNoFilter
        {
            get
            {
                return _tSESymbolNoFilter;
            }
            set
            {
                _tSESymbolNoFilter = value;
                OnPropertyChanged("TSESymbolNoFilter");
            }
        }
        /// <summary>
        /// 篩選現貨上櫃
        /// </summary>
        public string TPEXSymbolNoFilter
        {
            get
            {
                return _tPEXSymbolNoFilter;
            }
            set
            {
                _tPEXSymbolNoFilter = value;
                OnPropertyChanged("TPEXSymbolNoFilter");
            }
        }
        
        public ObservableCollection<MarketDataApi.PacketPATS.Format0> PatsFormat0List
        {
            get
            {
                return _patsFormat0List;
            }
            set
            {
                _patsFormat0List = value;
                OnPropertyChanged("PatsFormat0List");
            }
        }
        public ObservableCollection<MarketDataApi.PacketPATS.Format1> PatsFormat1List
        {
            get
            {
                return _patsFormat1List;
            }
            set
            {
                _patsFormat1List = value;
                OnPropertyChanged("PatsFormat1List");
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

        /// <summary>
        /// 連接行情伺服器
        /// </summary>
        private void ConncetMarketData()
        {
            try
            {
                api = new MarketDataApi.MarketDataApi(IPAddress, int.Parse(IPPort));
                api.TaifexI020Received += api_TaifexI020Received;  /// <- 期貨I020[成交]回呼事件
                api.TaifexI080Received += api_TaifexI080Received;  /// <- 期貨I080[委買委賣]回呼事件
                api.TseFormat6Received += api_TseFormat6Received;  /// <- 上市現貨格式6(Format6)回呼事件
                api.TpexFormat6Received += api_TpexFormat6Received; /// <- 上櫃現貨格式6(Format6)回呼事件
                api.TaifexI010Received += api_TaifexI010Received; /// <- 期貨I010[盘前]回呼事件
                api.TseFormat1Received += api_TseFormat1Received; /// <- 上櫃現貨格式1(盘前)回呼事件
                api.TpexFormat1Received += api_TpexFormat1Received;/// <- 上市現貨格式1(盘前)回呼事件
                api.PatsFormat0Received += api_PatsFormat0Received; ; /// <- pats(盤前)格式0回呼事件
                api.PatsFormat1Received += api_PatsFormat1Received; ; /// <- pats(行情)格式1回呼事件
                //盤前订阅测试
                //api.Sub(AdapterCode.TAIFEX_FUTURES_DAY, "I010");
                //api.Sub(AdapterCode.TAIFEX_OPTIONS_DAY, "I010");
                //api.Sub(AdapterCode.TSE, "1");
                //api.Sub(AdapterCode.TPEX, "1");

                StatusMessageList.Insert(0, DateTime.Now.ToString("HH:mm:ss:ttt") + "    " + "连接" + IPAddress);
            }
            catch (Exception err)
            {
                _logger.Error(err, string.Format("ConncetMarketData(): ErrMsg = {0}.", err.Message));
                StatusMessageList.Insert(0, DateTime.Now.ToString("HH:mm:ss:ttt") + "    " + string.Format("ConncetMarketData(): ErrMsg = {0}.", err.Message));
            }
        }

        //------------------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// 註冊報價
        /// </summary>
        private void SupscribeSymbol()
        {
            string subSymbol = SelectMarket + ";" + SelectType + ";" + SymbolNo;
            if (Rtn_adapterCode(SelectMarket) == AdapterCode.TAIFEX_GLOBAL_PATS && SymbolNo != "all")
            {
                //訂閱易盛商品轉換PATS商品
                string[] subSymbolAry = SymbolNo.Split('.');
                if (subSymbolAry.Count() == 3)
                {
                    string exchangeNo = string.Empty;
                    string commondityNo = string.Empty;
                    string contractNo = string.Empty;
                    //顯示轉換易盛2PATS
                    if (ESunnyConverterToPATS(subSymbolAry[0], subSymbolAry[1], subSymbolAry[2], ref exchangeNo, ref commondityNo, ref contractNo))
                    {
                        api.Sub(Rtn_adapterCode(SelectMarket), SelectType, SymbolNo);
                        SubscribeList.Insert(0, subSymbol);
                        StatusMessageList.Insert(0, string.Format("{0}    订阅成功:   {1}", DateTime.Now.ToString("HH:mm:ss:ttt"), subSymbol));
                    }
                    else
                    {
                        StatusMessageList.Insert(0, string.Format("{0}    订阅失败:   {1}", DateTime.Now.ToString("HH:mm:ss:ttt"), subSymbol));
                    }
                    //顯示轉換PATS2易盛
                    //if (PATSConverterToESunny(subSymbolAry[0], subSymbolAry[1], subSymbolAry[2], ref exchangeNo, ref commondityNo, ref contractNo))
                    //{
                    //    StatusMessageList.Insert(0, string.Format("{0}    订阅成功:   {1}", DateTime.Now.ToString("HH:mm:ss:ttt"), subSymbol));
                    //}
                    //else
                    //{
                    //    StatusMessageList.Insert(0, string.Format("{0}    订阅失败:   {1}", DateTime.Now.ToString("HH:mm:ss:ttt"), subSymbol));
                    //}
                }
            }
            else
            {
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
                StatusMessageList.Insert(0, DateTime.Now.ToString("HH:mm:ss:ttt") + "    " + "订阅:" + subSymbol);
            }
        }
        /// <summary>
        /// PATS轉易盛訂閱資料
        /// </summary>
        private bool PATSConverterToESunny(string exchange, string commondity, string contract, ref string exchangeNo, ref string commondityNo, ref string contractNo)
        {
            string orgData = exchange + "." + commondity + "." + contract;
            exchangeNo = string.Empty;
            commondityNo = string.Empty;
            bool isConverter = ESunnyPATSMapConverter.Instance.GetESExchangeNoCommondityNo(exchange, commondity, ref exchangeNo, ref commondityNo);
            if (isConverter)
            {
                contractNo = string.Empty;
                bool isContract = ESunnyPATSMapConverter.Instance.GetESContract((exchange == "LME"), contract, ref contractNo);
                if (isContract)
                {
                    string esummySymbol = exchangeNo + "." + commondityNo + "." + contractNo;
                    StatusMessageList.Insert(0, string.Format("{0}    PATS转换易盛成功:    PATS: 「{1}」,  ESunny:  「{2}」", DateTime.Now.ToString("HH:mm:ss:ttt"), orgData, esummySymbol));
                    return true;
                }
                else
                {
                    StatusMessageList.Insert(0, string.Format("{0}    PATS转换易盛失败:    PATS: 「{1}」,  ESunny:  「{2}」", DateTime.Now.ToString("HH:mm:ss:ttt"), orgData, exchangeNo + "." + commondityNo + "." + contractNo));
                    return false;
                }
            }
            else
            {
                StatusMessageList.Insert(0, string.Format("{0}    PATS转换易盛失败:    PATS: 「{1}」", DateTime.Now.ToString("HH:mm:ss:ttt"), orgData, exchangeNo + "." + commondityNo));
                return true;
            }
        }
        /// <summary>
        /// 易盛轉PATS訂閱資料
        /// </summary>
        private bool ESunnyConverterToPATS(string exchange, string commondity, string contract, ref string exchangeNo, ref string commondityNo, ref string contractNo)
        {
            string orgData = exchange + "." + commondity + "." + contract;
            exchangeNo = string.Empty;
            commondityNo = string.Empty;
            bool isConverter = ESunnyPATSMapConverter.Instance.GetPATSExchangeNoCommondityNo(exchange, commondity, ref exchangeNo, ref commondityNo);
            if (isConverter)
            {
                contractNo = string.Empty;
                bool isContract = ESunnyPATSMapConverter.Instance.GetPATSContract((exchange == "LME"), contract, ref contractNo);
                if (isContract)
                {
                    string patsSymbol = exchangeNo + "." + commondityNo + "." + contractNo;
                    StatusMessageList.Insert(0, string.Format("{0}    易盛转换PATS成功:    ESunny:  「{1}」,  PATS: 「{2}」", DateTime.Now.ToString("HH:mm:ss:ttt"), orgData, patsSymbol));
                    return true;
                }
                else
                {
                    StatusMessageList.Insert(0, string.Format("{0}    易盛转换PATS失败:    ESunny:  「{1}」,  PATS: 「{2}」", DateTime.Now.ToString("HH:mm:ss:ttt"), orgData, exchange + "." + commondityNo + "." + contract));
                    return false;
                }
            }
            else
            {
                StatusMessageList.Insert(0, string.Format("{0}    易盛转换PATS失败:    ESunny:  「{1}」,  PATS: 「{2}」", DateTime.Now.ToString("HH:mm:ss:ttt"), orgData, exchangeNo + "." + commondityNo));
                return false;
            }
        }
        //------------------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// 取消註冊報價
        /// </summary>
        private void UnSupscribeSymbol()
        {
            if (string.IsNullOrEmpty(SymbolNo))
            {
                api.UnSub(Rtn_adapterCode(SelectMarket), SelectType);

                SubscribeList.Remove(SelectSubscribe);
                StatusMessageList.Insert(0, DateTime.Now.ToString("HH:mm:ss:ttt") + "    " + "取消订阅:" + SelectMarket + ";" + SelectType);
            }
            else
            {
                api.UnSub(Rtn_adapterCode(SelectMarket), SelectType, SymbolNo);

                SubscribeList.Remove(SelectSubscribe);
                StatusMessageList.Insert(0, DateTime.Now.ToString("HH:mm:ss:ttt") + "    " + "取消订阅:" + SelectMarket + ";" + SelectType + ";" + SymbolNo);
            }
        }
        //------------------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// 取消註冊報價
        /// </summary>
        private void clearContent()
        {
            QuotesList.Clear();
            _gridSeq = 1;
        }
        #endregion

        #region Event
        //------------------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// 期貨I080[委買委賣]回呼事件
        /// </summary>
        void api_TaifexI080Received(object sender, MarketDataApi.MarketDataApi.TaifexI080ReceivedEventArgs e)
        {
            /// <- Bid Asl 0~1表示1檔~5檔
            App.Current.Dispatcher.Invoke((Action)(() =>
            {
                String fData;
                fData = string.Format("5檔I080,{0},Bid1 {1}({2}),Bid2 {3}({4}),Ask1 {5}({6}),Ask2 {7}({8})"
                    , e.PacketData.B_ProdId
                    , e.PacketData.B_BuyOrderBook[0].MatchPrice
                    , e.PacketData.B_BuyOrderBook[0].MatchQuantity
                    , e.PacketData.B_BuyOrderBook[1].MatchPrice
                    , e.PacketData.B_BuyOrderBook[1].MatchQuantity
                    , e.PacketData.B_SellOrderBook[0].MatchPrice
                    , e.PacketData.B_SellOrderBook[0].MatchQuantity
                    , e.PacketData.B_SellOrderBook[1].MatchPrice
                    , e.PacketData.B_SellOrderBook[1].MatchQuantity);
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
        void api_TaifexI020Received(object sender, MarketDataApi.MarketDataApi.TaifexI020ReceivedEventArgs e)
        {
            App.Current.Dispatcher.Invoke((Action)(() =>
            {
                String fData;
                fData = string.Format("成交I020,{0},{1}({2})({3}),{4}", e.PacketData.B_ProdId
                    , e.PacketData.B_FirstMatchPrice
                    , e.PacketData.B_FirstMatchQnty
                    , e.PacketData.B_MatchTotalQty
                    , e.PacketData.H_InformationTime);
                Quotes model = new Quotes();
                model.SetI020Data(_gridSeq, e.PacketData);
                QuotesList.Insert(0, model);
                _gridSeq++;
            }));
        }
        //------------------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// 上櫃現貨格式6回呼事件
        /// </summary>
        void api_TpexFormat6Received(object sender, MarketDataApi.MarketDataApi.TpexFormat6ReceivedEventArgs e)
        {
            App.Current.Dispatcher.Invoke((Action)(() =>
            {
                String fData;
                fData = string.Format("Tpex(上櫃),{0},{1}({2}),Bid1 {3}({4}),Ask1 {5}({6})"
                    , e.PacketData.StockID
                    , e.PacketData.LastPrice
                    , e.PacketData.TotalVolume
                    , e.PacketData.BidData[0].Price
                    , e.PacketData.BidData[0].Volume
                    , e.PacketData.AskData[0].Price
                    , e.PacketData.AskData[0].Volume);
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
        void api_TseFormat6Received(object sender, MarketDataApi.MarketDataApi.TseFormat6ReceivedEventArgs e)
        {
            App.Current.Dispatcher.Invoke((Action)(() =>
            {
                String fData;
                fData = string.Format("TSE(上市),{0},{1}({2}),Bid1 {3}({4}),Ask1 {5}({6})"
                    , e.PacketData.StockID
                    , e.PacketData.LastPrice
                    , e.PacketData.TotalVolume
                    , e.PacketData.BidData[0].Price
                    , e.PacketData.BidData[0].Volume
                    , e.PacketData.AskData[0].Price
                    , e.PacketData.AskData[0].Volume);
                Quotes model = new Quotes();
                model.SetTseData(_gridSeq, e.PacketData);
                QuotesList.Insert(0,model);
                _gridSeq++;
            }));
        }
        //------------------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// 上市現貨格式1回呼事件
        /// </summary>
        void api_TseFormat1Received(object sender, MarketDataApi.MarketDataApi.TseFormat1ReceivedEventArgs e)
        {
            App.Current.Dispatcher.Invoke((Action)(() =>
            {
                if (SymbolTseDictionary.ContainsKey(e.PacketData.StockID))
                {
                    return;
                }

                SymbolTseList.AddSymbolTseData(new SymbolTse(e.PacketData));
                SymbolTseDictionary = SymbolTseList.AllSymbolTseList;
            }));
        }
        //------------------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// 上櫃現貨格式1回呼事件
        /// </summary>
        void api_TpexFormat1Received(object sender, MarketDataApi.MarketDataApi.TpexFormat1ReceivedEventArgs e)
        {
            App.Current.Dispatcher.Invoke((Action)(() =>
            {
                if (SymbolTpexDictionary.ContainsKey(e.PacketData.StockID))
                {
                    return;
                }

                SymbolTpexList.AddSymbolTpexData(new SymbolTpex(e.PacketData));
                SymbolTpexDictionary = SymbolTpexList.AllSymbolTpexList;
            }));
        }
        //------------------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// 期貨I010[資訊]回呼事件
        /// </summary>
        void api_TaifexI010Received(object sender, MarketDataApi.MarketDataApi.TaifexI010ReceivedEventArgs e)
        {
            App.Current.Dispatcher.Invoke((Action)(() =>
            {
                AddSymbolTaifexDictionary(e.PacketData);
            }));
        }
        //------------------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// pats盤前回呼事件
        /// </summary>
        private void api_PatsFormat0Received(object sender, MarketDataApi.MarketDataApi.PatsFormat0ReceivedEventArgs e)
        {
            App.Current.Dispatcher.Invoke((Action)(() =>
            {
                if(e.PacketData.Exchange == "End")
                {
                    StatusMessageList.Insert(0, DateTime.Now.ToString("HH:mm:ss:ttt") + "    PATS商品資料傳送完成");
                    return;
                }
                _patsFormat0List.Add(e.PacketData);
            }));
        }
        //------------------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// pats回呼事件
        /// </summary>
        private void api_PatsFormat1Received(object sender, MarketDataApi.MarketDataApi.PatsFormat1ReceivedEventArgs e)
        {
            App.Current.Dispatcher.Invoke((Action)(() =>
            {
                _patsFormat1List.Add(e.PacketData);

                int count = _patsFormat1List.GroupBy(x => new { x.ExchangeNo, x.CommodityNo, x.ContractDate }).Count();
                StatusMessageList.Insert(0, DateTime.Now.ToString("HH:mm:ss:ttt") + "    " + "個數:" + count);
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
            switch (SelectMarket.Substring(0, 1))
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
                case "6":
                    adapterCode = AdapterCode.TAIFEX_GLOBAL_PATS; break; /// <- PATS
                default:
                    adapterCode = AdapterCode.TSE;
                    break;
            }
            return adapterCode;
        }

        private void LoadTSEData()
        {
            try
            {
                string path = System.Environment.CurrentDirectory + "\\TSE_1";
                if (File.Exists(path) == false)
                {
                    return;
                }
                using (var reader = File.OpenText(path))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null )
                    {
                        if(string.IsNullOrEmpty(line))
                        {
                            continue;
                        }
                        MarketDataApi.PacketTSE.Format1 data = new MarketDataApi.PacketTSE.Format1(Encoding.Default.GetBytes(line));
                        AddSymbolTseDictionary(data);
                    }
                }
            }
            catch (Exception err)
            {
                _logger.Error(err, string.Format("LoadTSEData(): ErrMsg = {0}.", err.Message));
            }
        }
        private void AddSymbolTseDictionary(MarketDataApi.PacketTSE.Format1 data)
        {

            if (string.IsNullOrEmpty(data.StockID) || SymbolTseDictionary.ContainsKey(data.StockID))
            {
                return;
            }

            SymbolTseList.AddSymbolTseData(new SymbolTse(data));
            SymbolTseDictionary = SymbolTseList.AllSymbolTseList;
        }

        private void LoadTPEXData()
        {
            try
            {
                string path = System.Environment.CurrentDirectory + "\\TPEX_1";
                if (File.Exists(path) == false)
                {
                    return;
                }
                using (var reader = File.OpenText(path))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (string.IsNullOrEmpty(line))
                        {
                            continue;
                        }
                        MarketDataApi.PacketTPEX.Format1 data = new MarketDataApi.PacketTPEX.Format1(Encoding.Default.GetBytes(line));
                        AddSymbolTpexDictionary(data);
                    }
                }
            }
            catch (Exception err)
            {
                _logger.Error(err, string.Format("LoadData(): ErrMsg = {0}.", err.Message));
            }
        }
        private void AddSymbolTpexDictionary(MarketDataApi.PacketTPEX.Format1 data)
        {

            if (string.IsNullOrEmpty(data.StockID) || SymbolTpexDictionary.ContainsKey(data.StockID))
            {
                return;
            }

            SymbolTpexList.AddSymbolTpexData(new SymbolTpex(data));
            SymbolTpexDictionary = SymbolTpexList.AllSymbolTpexList;
        }

        private void LoadTAIFEXData()
        {
            try
            {
                string path = System.Environment.CurrentDirectory + "\\TAIFEX_I010";
                if(File.Exists(path) == false)
                {
                    return;
                }
                using (var reader = File.OpenText(path))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (string.IsNullOrEmpty(line))
                        {
                            continue;
                        }
                        MarketDataApi.PacketTAIFEX.I010 data = new MarketDataApi.PacketTAIFEX.I010(Encoding.Default.GetBytes(line), 0);
                        AddSymbolTaifexDictionary(data);
                    }
                }
            }
            catch (Exception err)
            {
                _logger.Error(err, string.Format("LoadData(): ErrMsg = {0}.", err.Message));
            }
        }
        private void AddSymbolTaifexDictionary(MarketDataApi.PacketTAIFEX.I010 data)
        {

            if (string.IsNullOrEmpty(data.B_ProdId) || SymbolTaifexDictionary.ContainsKey(data.B_ProdId))
            {
                return;
            }

            SymbolTaifexList.AddSymbolTalfexData(new SymbolTaifex(data));
            SymbolTaifexDictionary = SymbolTaifexList.AllSymbolTaifexList;
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
    /// <summary>
    /// 綁定按鈕事件
    /// </summary>
    public class RelayCommand : ICommand
    {

        readonly Func<Boolean> _canExecute;
        readonly Action _execute;

        public RelayCommand(Action execute)
            : this(execute, null)
        {
        }

        public RelayCommand(Action execute, Func<Boolean> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");
            _execute = execute;
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {

                if (_canExecute != null)
                    CommandManager.RequerySuggested += value;
            }
            remove
            {

                if (_canExecute != null)
                    CommandManager.RequerySuggested -= value;
            }
        }

        [DebuggerStepThrough]
        public Boolean CanExecute(Object parameter)
        {
            return _canExecute == null ? true : _canExecute();
        }

        public void Execute(Object parameter)
        {
            _execute();
        }
    }
}
