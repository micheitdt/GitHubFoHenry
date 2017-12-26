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
using NLog;
using System.IO;
using System.Collections.Concurrent;
using System.Data;
using ServiceStack.Redis;
using System.Windows.Data;
using NPOI.HSSF.UserModel;
using ESunnyPATSConverterApi;
using MarketDataApiExample.Model;
using System.Windows.Threading;

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

        #region Fields
        public static readonly List<string> MarketList = new List<string>() { "0-上市", "1-上櫃", "2-期貨AM盤", "3-選擇權AM盤", "4-期貨PM盤", "5-選擇權PM盤", "6-PATS" };
        public static readonly List<string> TypeList = new List<string>() { "I010", "I020", "I080", "1", "6", "17", "0", "2" };

        private static MainViewModel _instance;
        private string _ipAddress = "47.52.229.28";//"10.214.19.51";"203.66.93.83";"127.0.0.1";47.52.229.28;
        private string _ipPort = "6688";
        private string _reqipAddress = "47.52.229.28";//"10.214.19.51";"203.66.93.83";"127.0.0.1";47.52.229.28;
        private string _reqipPort = "5588";
        private string _selectMarket = "6-PATS";//"6-PATS";
        private string _selectType = "1";
        private string _symbolNo = "";//CME_ES.GE0.DEC07//all
        private string _selectSubscribe = string.Empty;
        private ObservableCollection<string> _subscribeList = new ObservableCollection<string>();
        private Dictionary<string, Quotes> _quotesList = new Dictionary<string, Quotes>();
        private List<Quotes> _historyQuotesList = new List<Quotes>();
        private ObservableCollection<Quotes> _quotesSnapshotList = new ObservableCollection<Quotes>();
        private Quotes _selectQuotes;
        private string _tSESymbolNoFilter = string.Empty;
        private string _tPEXSymbolNoFilter = string.Empty;
        private string _tAIFEXSymbolNoFilter = string.Empty;
        private ObservableCollection<SymbolTse> _symbolTseDictionary = new ObservableCollection<SymbolTse>();
        private SymbolTse _selectTse;
        private ObservableCollection<SymbolTpex> _symbolTpexDictionary = new ObservableCollection<SymbolTpex>();
        private SymbolTpex _selectTpex;
        private ObservableCollection<SymbolTaifex> _symbolTaifexDictionary = new ObservableCollection<SymbolTaifex>();
        private SymbolTaifex _selectTaifex;
        MdApi api;
        private long _packetSize = 0;
        private DateTime _subTime;
        private double _packetTraffic = 0;

        private ObservableCollection<MarketDataApi.PacketPATS.Format0> _patsFormat0List = new ObservableCollection<MarketDataApi.PacketPATS.Format0>();
        private ObservableCollection<MarketDataApi.PacketPATS.Format1> _patsFormat1List = new ObservableCollection<MarketDataApi.PacketPATS.Format1>();
        private ObservableCollection<MarketDataApi.PacketPATS.Format2> _patsFormat2List = new ObservableCollection<MarketDataApi.PacketPATS.Format2>();
        private MarketDataApi.PacketPATS.Format0 _selectPats;
        private ObservableCollection<string> _statusMessageList = new ObservableCollection<string>();
        #endregion

        public MainViewModel()
        {
            try
            {
                DefaultSettings.Instance.Initialize();//讀設定檔
                //for (uint i = 0; i < (UInt32.MaxValue / 8); i++)
                //{
                //    getConvertTime1(i);
                //    getConvertTime2(i);
                //}
            }
            catch(Exception ex)
            {
                _logger.Error(ex, string.Format("MainViewModel(): ErrMsg = {0}.", ex.Message));
            }
        }
        //test int效能
        private void getConvertTime1(uint i)
        {
            byte[] data = new byte[10];

            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();//引用stopwatch物件
            sw.Reset();
            sw.Start(); ;

            int count = Common.Utility.SetIntToDynamicBytes(ref data, 0, i);

            sw.Stop();
            Debug.WriteLine("int to byte array :" + sw.Elapsed.TotalMilliseconds.ToString() + "i :" + i );

            sw.Reset();
            sw.Start();

            var value = Common.Utility.GetIntToDynamicBytes(data, 0);

            sw.Stop();
            Debug.WriteLine("byte array to int :" + sw.Elapsed.TotalMilliseconds.ToString() + "value:" + value);
            if( i != value)
            {
                System.Windows.MessageBox.Show(i + "error" + value);
            }
        }
        //test double效能
        private void getConvertTime2(uint i)
        {
            //EX1:( P=100, B=90, T=1~0.1E7 )= 100。價 > 基 & 正值
            //EX2:( P=-90, B=-100, T=1~0.1E7 )= 100。價 > 基 & 負值
            //EX3:( P=100, B=100, T=1~0.1E7 )= 100。價 = 基 & 正值
            //EX4:( P=-100, B=-100, T=1~0.1E7 )= 100。價 = 基 & 負值
            //EX5:( P=10.215, B=10, T=1~0.1E7 )= 100。價 > 基 & 正小數值
            //EX6:( P=10.215, B=-10, T=1~0.1E7 )= 100。價 > 基 & 負小數值
            //EX7:( P=-11.215, B=-10, T=1~0.1E7 )= 100。價 < 基 & 負小數值
            //EX8:( P=-11.215, B=651, T=1~0.1E7 )= 100。價 < 基 & 負小數值
            byte[] data = new byte[10];
            Stopwatch sw = new Stopwatch();//引用stopwatch物件
            sw.Reset();
            sw.Start(); ;
            uint count = 0;
            Common.Utility.SetDoubleToDynamicBytes(ref data, 0, -11.0215m, -10m, (decimal)(1/Math.Pow(10,i)) , ref count);//10

            sw.Stop();
            Debug.WriteLine("int to byte array :" + sw.Elapsed.TotalMilliseconds.ToString());

            sw.Reset();
            sw.Start();
            
            decimal price = Common.Utility.GetDoubleToDynamicBytes(ref data, 0, -10m, (decimal)(1 / Math.Pow(10, i)), ref count);//10

            sw.Stop();
            Debug.WriteLine("byte array to int :" + sw.Elapsed.TotalMilliseconds.ToString());
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
        public Dictionary<string,Quotes> QuotesList
        {
            get
            {
                return _quotesList;
            }
            set
            {
                _quotesList = value;
                OnPropertyChanged("QuotesValueList");
            }
        }
        public ObservableCollection<Quotes> QuotesValueList
        {
            get
            {
                return new ObservableCollection<Quotes>(_quotesList.Values);
            }
        }
        /// <summary>
        /// 歷史行情
        /// </summary>
        public List<Quotes> HistoryQuotesList
        {
            get
            {
                return _historyQuotesList;
            }
            set
            {
                _historyQuotesList = value;
                OnPropertyChanged("HistoryQuotesList");
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
                if (_tAIFEXSymbolNoFilter == value)
                    return;

                _tAIFEXSymbolNoFilter = value;
                OnPropertyChanged("TAIFEXSymbolNoFilter");

                SymbolTaifexDictionary = SymbolTaifexList.GetFilterSymbolNo(value);
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
                if (_tSESymbolNoFilter == value)
                    return;
                
                _tSESymbolNoFilter = value;
                OnPropertyChanged("TSESymbolNoFilter");

                SymbolTseDictionary = SymbolTseList.GetFilterSymbolNo(value);
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
                if (_tPEXSymbolNoFilter == value)
                    return;

                _tPEXSymbolNoFilter = value;
                OnPropertyChanged("TPEXSymbolNoFilter");

                SymbolTpexDictionary = SymbolTpexList.GetFilterSymbolNo(value);
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
        /// <summary>
        /// 所選行情
        /// </summary>
        public Quotes SelectQuotes
        {
            get
            {
                return _selectQuotes;
            }
            set
            {
                _selectQuotes = value;
            }
        }
        /// <summary>
        /// 所選盤前TSE
        /// </summary>
        public SymbolTse SelectTse
        {
            get
            {
                return _selectTse;
            }
            set
            {
                _selectTse = value;
            }
        }
        /// <summary>
        /// 所選盤前TPEX
        /// </summary>
        public SymbolTpex SelectTpex
        {
            get
            {
                return _selectTpex;
            }
            set
            {
                _selectTpex = value;
            }
        }
        /// <summary>
        /// 所選盤前TAIFEX
        /// </summary>
        public SymbolTaifex SelectTaifex
        {
            get
            {
                return _selectTaifex;
            }
            set
            {
                _selectTaifex = value;
            }
        }
        /// <summary>
        /// 所選盤前PATS
        /// </summary>
        public MarketDataApi.PacketPATS.Format0 SelectPats
        {
            get
            {
                return _selectPats;
            }
            set
            {
                _selectPats = value;
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
                        ClearContent
                    );
                }
                return _clearContentCommand;
            }
        }
        private ICommand _gridDoubleClickCommand;
        public ICommand GridDoubleClickCommand
        {
            get
            {
                if (_gridDoubleClickCommand == null)
                {
                    _gridDoubleClickCommand = new RelayCommand(
                        QuotesGridDoubleClick
                    );
                }
                return _gridDoubleClickCommand;
            }
        }

        private ICommand _gridTseDoubleClickCommand;
        public ICommand GridTseDoubleClickCommand
        {
            get
            {
                if (_gridTseDoubleClickCommand == null)
                {
                    _gridTseDoubleClickCommand = new RelayCommand(
                        TseGridDoubleClick
                    );
                }
                return _gridTseDoubleClickCommand;
            }
        }

        private ICommand _gridTpexDoubleClickCommand;
        public ICommand GridTpexDoubleClickCommand
        {
            get
            {
                if (_gridTpexDoubleClickCommand == null)
                {
                    _gridTpexDoubleClickCommand = new RelayCommand(
                        TpexGridDoubleClick
                    );
                }
                return _gridTpexDoubleClickCommand;
            }
        }

        private ICommand _gridTaifexDoubleClickCommand;
        public ICommand GridTaifexDoubleClickCommand
        {
            get
            {
                if (_gridTaifexDoubleClickCommand == null)
                {
                    _gridTaifexDoubleClickCommand = new RelayCommand(
                        TaifexGridDoubleClick
                    );
                }
                return _gridTaifexDoubleClickCommand;
            }
        }

        private ICommand _outputExcelCommand;
        public ICommand OutputExcelCommand
        {
            get
            {
                if (_outputExcelCommand == null)
                {
                    _outputExcelCommand = new RelayCommand(
                        OutputExcel
                    );
                }
                return _outputExcelCommand;
            }
        }

        private ICommand _gridPatsDoubleClickCommand;
        public ICommand GridPatsDoubleClickCommand
        {
            get
            {
                if (_gridPatsDoubleClickCommand == null)
                {
                    _gridPatsDoubleClickCommand = new RelayCommand(
                        PATSGridDoubleClick
                    );
                }
                return _gridPatsDoubleClickCommand;
            }
        }

        private ICommand _singleSubCommand;
        public ICommand SingleSubCommand
        {
            get
            {
                if (_singleSubCommand == null)
                {
                    _singleSubCommand = new RelayCommand(
                        SingleSupscribe
                    );
                }
                return _singleSubCommand;
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

        private ICommand _allSubPacketCommand;
        public ICommand AllSubPacketCommand
        {
            get
            {
                if (_allSubPacketCommand == null)
                {
                    _allSubPacketCommand = new RelayCommand(
                        AllSubPacket
                    );
                }
                return _allSubPacketCommand;
            }
        }
        #endregion

        #region Command func
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
                api.PatsFormat0Received += api_PatsFormat0Received; ; /// <- pats(盤前)格式0回呼事件
                api.PatsFormat1Received += api_PatsFormat1Received; ; /// <- pats(行情)格式1回呼事件
                api.PatsFormat2Received += api_PatsFormat2Received; ; /// <- pats(行情)格式1回呼事件
                //test
                //api.QuotesByteArrayReceived += Api_QuotesByteArrayReceived;
                StatusMessageList.Insert(0, DateTime.Now.ToString("HH:mm:ss:ttt") + "    " + "連接" + IPAddress + ":" + IPPort + "," + ReqIPAddress + ":" + ReqIPPort);

                GetPreSymbol();
            }
            catch (Exception err)
            {
                _logger.Error(err, string.Format("ConncetMarketData(): ErrMsg = {0}.", err.Message));
                StatusMessageList.Insert(0, DateTime.Now.ToString("HH:mm:ss:ttt") + "    " + string.Format("ConncetMarketData(): ErrMsg = {0}.", err.Message));
            }
        }
        //test
        //private void Api_QuotesByteArrayReceived(object sender, MdApi.QuotesByteArrayEventArgs e)
        //{
        //    _packetSize += e.PacketData.Length;
        //    _packetTraffic = (_packetSize / ((DateTime.Now - _subTime).TotalSeconds)) / 1024d / 1024d;
        //    _logger.Debug(string.Format("訂閱時間:{0},  封包大小:{1},   每秒接收(mb/s):{2}", _subTime, _packetSize, _packetTraffic));
        //}

        //------------------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// 註冊報價
        /// </summary>
        private void SupscribeSymbol()
        {
            if (api == null || string.IsNullOrEmpty(SelectMarket) || string.IsNullOrEmpty(SelectType))
            {
                return;
            }
            string subSymbol = SelectMarket + ";" + SelectType + ";" + SymbolNo;
            if (Rtn_adapterCode(SelectMarket) == AdapterCode.GLOBAL_PATS && SymbolNo != "all")
            {
                //訂閱易盛商品轉換PATS商品
                string[] subSymbolAry = SymbolNo.Split('.');
                if (subSymbolAry.Count() == 3)
                {
                    string exchangeNo = string.Empty;
                    string commondityNo = string.Empty;
                    string contractNo = string.Empty;
                    string msg = string.Empty;
                    //顯示轉換易盛2PATS
                    if (ESunnyPATSMapConverter.Instance.ESunnyConverterToPATS(subSymbolAry[0], subSymbolAry[1], subSymbolAry[2], ref exchangeNo, ref commondityNo, ref contractNo, ref msg))
                    {
                        api.Sub(Rtn_adapterCode(SelectMarket), SelectType, string.Format("{0}.{1}.{2}", exchangeNo, commondityNo, contractNo));
                        SubscribeList.Insert(0, subSymbol);
                        StatusMessageList.Insert(0, msg);
                        StatusMessageList.Insert(0, string.Format("{0}    訂閱成功:   {1}", DateTime.Now.ToString("HH:mm:ss:ttt"), subSymbol));
                    }
                    else
                    {
                        StatusMessageList.Insert(0, msg);
                        StatusMessageList.Insert(0, string.Format("{0}    訂閱失敗:   {1}", DateTime.Now.ToString("HH:mm:ss:ttt"), subSymbol));
                    }
                    //顯示轉換PATS2易盛
                    //if (ESunnyPATSMapConverter.Instance.PATSConverterToESunny(subSymbolAry[0], subSymbolAry[1], subSymbolAry[2], ref exchangeNo, ref commondityNo, ref contractNo, ref msg))
                    //{
                    //    GeneralSupscribeSymbol(SelectMarket, SelectType, SymbolNo);
                    //    StatusMessageList.Insert(0, msg);
                    //    StatusMessageList.Insert(0, string.Format("{0}    订阅成功:   {1}", DateTime.Now.ToString("HH:mm:ss:ttt"), subSymbol));
                    //}
                    //else
                    //{
                    //    StatusMessageList.Insert(0, msg);
                    //    StatusMessageList.Insert(0, string.Format("{0}    订阅失败:   {1}", DateTime.Now.ToString("HH:mm:ss:ttt"), subSymbol));
                    //}
                }
            }
            else
            {
                GeneralSupscribeSymbol(SelectMarket, SelectType, SymbolNo);
            }
        }
        //------------------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// 取消註冊報價
        /// </summary>
        private void UnSupscribeSymbol()
        {
            if (string.IsNullOrEmpty(SelectMarket) || string.IsNullOrEmpty(SelectType))
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
        private void ClearContent()
        {
            QuotesList.Clear();
        }
        //------------------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// 行情訂閱
        /// </summary>
        private void QuotesGridDoubleClick()
        {
            if (_selectQuotes == null)
                return;
            switch (_selectQuotes.Market)
            {
                case "上市":
                    GeneralSupscribeSymbol("0-上市", _selectQuotes.TypeNo, _selectQuotes.SymbolNo);
                    break;
                case "上櫃":
                    GeneralSupscribeSymbol("0-上櫃", _selectQuotes.TypeNo, _selectQuotes.SymbolNo);
                    break;
                case "期貨":
                    GeneralSupscribeSymbol("2-期貨AM盤", _selectQuotes.TypeNo, _selectQuotes.SymbolNo);
                    GeneralSupscribeSymbol("4-期貨PM盤", _selectQuotes.TypeNo, _selectQuotes.SymbolNo);
                    break;
                case "選擇權":
                    GeneralSupscribeSymbol("3-選擇權AM盤", _selectQuotes.TypeNo, _selectQuotes.SymbolNo);
                    GeneralSupscribeSymbol("5-選擇權PM盤", _selectQuotes.TypeNo, _selectQuotes.SymbolNo);
                    break;
            }

        }
        //------------------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// 盤前TSE訂閱
        /// </summary>
        private void TseGridDoubleClick()
        {
            if (_selectTse == null)
                return;
            GeneralSupscribeSymbol("0-上市", "6", _selectTse.SymbolNo);
            GeneralSupscribeSymbol("0-上市", "17", _selectTse.SymbolNo);
        }
        //------------------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// 盤前TPEX訂閱
        /// </summary>
        private void TpexGridDoubleClick()
        {
            if (_selectTpex == null)
                return;
            GeneralSupscribeSymbol("1-上櫃", "6", _selectTpex.SymbolNo);
            GeneralSupscribeSymbol("1-上櫃", "17", _selectTpex.SymbolNo);
        }
        //------------------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// 盤前TAIFEX訂閱
        /// </summary>
        private void TaifexGridDoubleClick()
        {
            if (_selectTaifex == null)
                return;
            if (_selectTaifex.TypeNo == "期貨")
            {
                GeneralSupscribeSymbol("2-期貨AM盤", "I020", _selectTaifex.SymbolNo);
                GeneralSupscribeSymbol("2-期貨AM盤", "I080", _selectTaifex.SymbolNo);
                GeneralSupscribeSymbol("4-期貨PM盤", "I020", _selectTaifex.SymbolNo);
                GeneralSupscribeSymbol("4-期貨PM盤", "I080", _selectTaifex.SymbolNo);
            }
            else
            {
                GeneralSupscribeSymbol("3-選擇權AM盤", "I020", _selectTaifex.SymbolNo);
                GeneralSupscribeSymbol("3-選擇權AM盤", "I080", _selectTaifex.SymbolNo);
                GeneralSupscribeSymbol("5-選擇權PM盤", "I020", _selectTaifex.SymbolNo);
                GeneralSupscribeSymbol("5-選擇權PM盤", "I080", _selectTaifex.SymbolNo);
            }
        }
        //------------------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// 盤前PATS訂閱
        /// </summary>
        private void PATSGridDoubleClick()
        {
            if (SelectPats == null)
                return;
            GeneralSupscribeSymbol("6-PATS", "1", string.Format("{0}.{1}.{2}", SelectPats.Exchange, SelectPats.Commodity, SelectPats.Contract));
        }
        //------------------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// 測試單一訂閱
        /// </summary>
        private void SingleSupscribe()
        {
            System.Threading.ThreadStart ts = delegate
            {
                try
                {
                    //單一商品訂閱
                    foreach (SymbolTaifex data in _symbolTaifexDictionary)
                    {
                        api.Sub(AdapterCode.TAIFEX_FUTURES_DAY, "I020", data.SymbolNo);
                        System.Threading.Thread.Sleep(1);
                        api.Sub(AdapterCode.TAIFEX_FUTURES_DAY, "I080", data.SymbolNo);
                        System.Threading.Thread.Sleep(1);
                        api.Sub(AdapterCode.TAIFEX_OPTIONS_DAY, "I020", data.SymbolNo);
                        System.Threading.Thread.Sleep(1);
                        api.Sub(AdapterCode.TAIFEX_OPTIONS_DAY, "I080", data.SymbolNo);
                        System.Threading.Thread.Sleep(1);
                        //GeneralSupscribeSymbol("4-期貨PM盤", "I020", data.SymbolNo);
                        //GeneralSupscribeSymbol("4-期貨PM盤", "I080", data.SymbolNo);
                        //GeneralSupscribeSymbol("5-選擇權PM盤", "I020", data.SymbolNo);
                        //GeneralSupscribeSymbol("6-選擇權PM盤", "I080", data.SymbolNo);
                    }
                    foreach (SymbolTpex data in _symbolTpexDictionary)
                    {
                        api.Sub(AdapterCode.TPEX, "6", data.SymbolNo);
                        System.Threading.Thread.Sleep(1);
                        api.Sub(AdapterCode.TPEX, "17", data.SymbolNo);
                        System.Threading.Thread.Sleep(1);
                        //GeneralSupscribeSymbol("1-上櫃", "6", data.SymbolNo);
                        //GeneralSupscribeSymbol("1-上櫃", "17", data.SymbolNo);
                    }
                    foreach (SymbolTse data in _symbolTseDictionary)
                    {
                        api.Sub(AdapterCode.TSE, "6", data.SymbolNo);
                        System.Threading.Thread.Sleep(1);
                        api.Sub(AdapterCode.TSE, "17", data.SymbolNo);
                        System.Threading.Thread.Sleep(1);
                        //GeneralSupscribeSymbol("0-上市", "6", data.SymbolNo);
                        //GeneralSupscribeSymbol("0-上市", "17", data.SymbolNo);
                    }
                }
                catch (Exception err)
                {
                    _logger.Error(err, string.Format("ConncetMarketData(): ErrMsg = {0}.", err.Message));
                    StatusMessageList.Insert(0, DateTime.Now.ToString("HH:mm:ss:ttt") + "    " + string.Format("ConncetMarketData(): ErrMsg = {0}.", err.Message));
                }
            };
            new System.Threading.Thread(ts).Start();
        }
        /// <summary>
        /// 取得快照
        /// </summary>
        private void GetSnapshot()
        {
            if (api == null || string.IsNullOrEmpty(SelectMarket) || string.IsNullOrEmpty(SelectType) || string.IsNullOrEmpty(SymbolNo))
            {
                System.Windows.MessageBox.Show("連接錯誤或無商品資料");
                return;
            }

            string subSymbol = SelectMarket + ";" + SelectType + ";" + SymbolNo;
            var ret = api.GetSnapshot(Rtn_adapterCode(SelectMarket), SelectType, SymbolNo);
            if (SelectMarket == "6-PATS")
            {
                string[] subSymbolAry = SymbolNo.Split(' ');
                string exchangeNo = string.Empty;
                string commondityNo = string.Empty;
                string contractNo = string.Empty;
                string msg = string.Empty;
                //顯示轉換易盛2PATS
                if (ESunnyPATSMapConverter.Instance.ESunnyConverterToPATS(subSymbolAry[0], subSymbolAry[1], subSymbolAry[2], ref exchangeNo, ref commondityNo, ref contractNo, ref msg))
                {
                    ret = api.GetSnapshot(Rtn_adapterCode(SelectMarket), SelectType, string.Format("{0}.{1}.{2}", exchangeNo, commondityNo, contractNo));                    
                }
                else
                {
                    StatusMessageList.Insert(0, DateTime.Now.ToString("HH:mm:ss:ttt") + "    " + "轉換失敗:" + subSymbol);
                    return;
                }
            }
            if (ret == null)
            {
                StatusMessageList.Insert(0, DateTime.Now.ToString("HH:mm:ss:ttt") + "    " + "取得快照失敗:" + subSymbol);
                return;
            }

            Quotes model = new Quotes();
            switch (SelectMarket.Substring(0, 1))
            {
                case "0":
                    switch (SelectType)
                    {
                        case "6":
                            model.SetTseData(0, ret as MarketDataApi.PacketTSE.Format6);
                            QuotesSnapshotList.Insert(0, model);
                            break;
                        case "17":
                            model.SetTseData(0, ret as MarketDataApi.PacketTSE.Format17);
                            QuotesSnapshotList.Insert(0, model);
                            break;
                    }
                    break;
                case "1":
                    switch (SelectType)
                    {
                        case "6":
                            model.SetTpexData(0, ret as MarketDataApi.PacketTPEX.Format6);
                            QuotesSnapshotList.Insert(0, model);
                            break;
                        case "17":
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
                        case "I020":
                            model.SetI020Data(0, ret as MarketDataApi.PacketTAIFEX.I020);
                            QuotesSnapshotList.Insert(0, model);
                            break;
                        case "I080":
                            model.SetI080Data(0, ret as MarketDataApi.PacketTAIFEX.I080);
                            QuotesSnapshotList.Insert(0, model);
                            break;
                    }
                    break;
                case "6":
                    switch (SelectType)
                    {
                        case "1":
                            _patsFormat1List.Insert(0, ret as MarketDataApi.PacketPATS.Format1);
                            break;
                        case "2":
                            _patsFormat2List.Insert(0, ret as MarketDataApi.PacketPATS.Format2);
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
            try
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
                StatusMessageList.Insert(0, DateTime.Now.ToString("HH:mm:ss:ttt") + "    " + "讀取盤前資料成功!!!");
            }
            catch (Exception ex)
            {
                StatusMessageList.Insert(0, DateTime.Now.ToString("HH:mm:ss:ttt") + "    " + "讀取盤前資料失敗!!!" + ex.Message);
            }
        }
        /// <summary>
        /// 計算全訂閱封包流量
        /// </summary>
        private void AllSubPacket()
        {
            _packetSize = 0;
            _subTime = DateTime.Now;

            api.Sub(AdapterCode.TAIFEX_FUTURES_DAY, "I020");
            api.Sub(AdapterCode.TAIFEX_FUTURES_DAY, "I080");
            api.Sub(AdapterCode.TAIFEX_OPTIONS_DAY, "I020");
            api.Sub(AdapterCode.TAIFEX_OPTIONS_DAY, "I080");
            api.Sub(AdapterCode.TAIFEX_FUTURES_NIGHT, "I020");
            api.Sub(AdapterCode.TAIFEX_FUTURES_NIGHT, "I080");
            api.Sub(AdapterCode.TAIFEX_OPTIONS_NIGHT, "I020");
            api.Sub(AdapterCode.TAIFEX_OPTIONS_NIGHT, "I080");
            api.Sub(AdapterCode.TPEX, "6");
            api.Sub(AdapterCode.TPEX, "17");
            api.Sub(AdapterCode.TSE, "6");
            api.Sub(AdapterCode.TSE, "17");
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
                model.SetI080Data(0, e.PacketData);

                _historyQuotesList.Add(model);

                if (QuotesList.ContainsKey(model.MainKey))
                {
                    QuotesList[model.MainKey] = model;
                }
                else
                {
                    QuotesList.Add(model.MainKey, model);
                }
                OnPropertyChanged("QuotesValueList");
                //_logger.Debug(fData);
                //測試-商品更新個數
                //int count = QuotesList.Values.GroupBy(x => x.SymbolNo).Count();
                //StatusMessageList.Insert(0, DateTime.Now.ToString("HH:mm:ss:ttt") + "    " + "個數:" + count);
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
                String fData;
                fData = string.Format("成交I020,{0},{1}({2})({3}),{4}", e.PacketData.B_ProdId
                    , e.PacketData.B_FirstMatchPrice
                    , e.PacketData.B_FirstMatchQnty
                    , e.PacketData.B_MatchTotalQty
                    , e.PacketData.H_InformationTime);
                Quotes model = new Quotes();
                model.SetI020Data(0, e.PacketData);

                _historyQuotesList.Add(model);

                if (QuotesList.ContainsKey(model.MainKey))
                {
                    QuotesList[model.MainKey] = model;
                }
                else
                {
                    QuotesList.Add(model.MainKey, model);
                }
                OnPropertyChanged("QuotesValueList");
                //_logger.Debug(fData);

                //測試-商品更新個數
                //int count = QuotesList.Values.GroupBy(x => x.SymbolNo).Count();
                //StatusMessageList.Insert(0, DateTime.Now.ToString("HH:mm:ss:ttt") + "    " + "個數:" + count);
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
                model.SetI082Data(0, e.PacketData);

                _historyQuotesList.Add(model);

                if (QuotesList.ContainsKey(model.MainKey))
                {
                    QuotesList[model.MainKey] = model;
                }
                else
                {
                    QuotesList.Add(model.MainKey, model);
                }
                OnPropertyChanged("QuotesValueList");
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
                model.SetI022Data(0, e.PacketData);

                _historyQuotesList.Add(model);

                if (QuotesList.ContainsKey(model.MainKey))
                {
                    QuotesList[model.MainKey] = model;
                }
                else
                {
                    QuotesList.Add(model.MainKey, model);
                }
                OnPropertyChanged("QuotesValueList");
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
                String fData;
                fData = string.Format("Tpex(上櫃),{0},{1}({2}),Bid1 {3}({4}),Ask1 {5}({6})"
                    , e.PacketData.StockID
                    , e.PacketData.LastPrice
                    , e.PacketData.TotalVolume
                        , (e.PacketData.BidData.Count == 0) ? 0 : e.PacketData.BidData[0].Price
                        , (e.PacketData.BidData.Count == 0) ? 0 : e.PacketData.BidData[0].Volume
                        , (e.PacketData.AskData.Count == 0) ? 0 : e.PacketData.AskData[0].Price
                        , (e.PacketData.AskData.Count == 0) ? 0 : e.PacketData.AskData[0].Volume);
                Quotes model = new Quotes();
                model.SetTpexData(0,  e.PacketData);

                _historyQuotesList.Add(model);

                if (QuotesList.ContainsKey(model.MainKey))
                {
                    QuotesList[model.MainKey] = model;
                }
                else
                {
                    QuotesList.Add(model.MainKey, model);
                }
                OnPropertyChanged("QuotesValueList");
                //_logger.Debug(fData);
                //測試-商品更新個數
                //int count = QuotesList.Values.GroupBy(x => x.SymbolNo).Count();
                //StatusMessageList.Insert(0, DateTime.Now.ToString("HH:mm:ss:ttt") + "    " + "個數:" + count);
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
                String fData;
                fData = string.Format("TSE(上市),{0},{1}({2}),Bid1 {3}({4}),Ask1 {5}({6})"
                    , e.PacketData.StockID
                    , e.PacketData.LastPrice
                    , e.PacketData.TotalVolume
                    , (e.PacketData.BidData.Count == 0) ? 0 : e.PacketData.BidData[0].Price
                    , (e.PacketData.BidData.Count == 0) ? 0 : e.PacketData.BidData[0].Volume
                    , (e.PacketData.AskData.Count == 0) ? 0 : e.PacketData.AskData[0].Price
                    , (e.PacketData.AskData.Count == 0) ? 0 : e.PacketData.AskData[0].Volume);
                Quotes model = new Quotes();
                model.SetTseData(0, e.PacketData);

                _historyQuotesList.Add(model);

                if (QuotesList.ContainsKey(model.MainKey))
                {
                    QuotesList[model.MainKey] = model;
                }
                else
                {
                    QuotesList.Add(model.MainKey, model);
                }
                OnPropertyChanged("QuotesValueList");
                //_logger.Debug(fData);
                //測試-商品更新個數
                //int count = QuotesList.Values.GroupBy(x => x.SymbolNo).Count();
                //StatusMessageList.Insert(0, DateTime.Now.ToString("HH:mm:ss:ttt") + "    " + "個數:" + count);
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
                String fData;
                fData = string.Format("Tpex(上櫃),{0},{1}({2}),Bid1 {3}({4}),Ask1 {5}({6})"
                    , e.PacketData.StockID
                    , e.PacketData.LastPrice
                    , e.PacketData.TotalVolume
                        , (e.PacketData.BidData.Count == 0) ? 0 : e.PacketData.BidData[0].Price
                        , (e.PacketData.BidData.Count == 0) ? 0 : e.PacketData.BidData[0].Volume
                        , (e.PacketData.AskData.Count == 0) ? 0 : e.PacketData.AskData[0].Price
                        , (e.PacketData.AskData.Count == 0) ? 0 : e.PacketData.AskData[0].Volume);
                Quotes model = new Quotes();
                model.SetTpexData(0, e.PacketData);

                _historyQuotesList.Add(model);

                if (QuotesList.ContainsKey(model.MainKey))
                {
                    QuotesList[model.MainKey] = model;
                }
                else
                {
                    QuotesList.Add(model.MainKey, model);
                }
                OnPropertyChanged("QuotesValueList");
                //_logger.Debug(fData);
                //測試-商品更新個數
                //int count = QuotesList.Values.GroupBy(x => x.SymbolNo).Count();
                //StatusMessageList.Insert(0, DateTime.Now.ToString("HH:mm:ss:ttt") + "    " + "個數:" + count);
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
                String fData;
                fData = string.Format("TSE(上市),{0},{1}({2}),Bid1 {3}({4}),Ask1 {5}({6})"
                    , e.PacketData.StockID
                    , e.PacketData.LastPrice
                    , e.PacketData.TotalVolume
                    , (e.PacketData.BidData.Count == 0) ? 0 : e.PacketData.BidData[0].Price
                    , (e.PacketData.BidData.Count == 0) ? 0 : e.PacketData.BidData[0].Volume
                    , (e.PacketData.AskData.Count == 0) ? 0 : e.PacketData.AskData[0].Price
                    , (e.PacketData.AskData.Count == 0) ? 0 : e.PacketData.AskData[0].Volume);
                Quotes model = new Quotes();
                model.SetTseData(0, e.PacketData);

                _historyQuotesList.Add(model);

                if (QuotesList.ContainsKey(model.MainKey))
                {
                    QuotesList[model.MainKey] = model;
                }
                else
                {
                    QuotesList.Add(model.MainKey, model);
                }
                OnPropertyChanged("QuotesValueList");
                //_logger.Debug(fData);
                //測試-商品更新個數
                //int count = QuotesList.Values.GroupBy(x => x.SymbolNo).Count();
                //StatusMessageList.Insert(0, DateTime.Now.ToString("HH:mm:ss:ttt") + "    " + "個數:" + count);
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
                AddSymbolTseDictionary(e.PacketData);
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
                AddSymbolTpexDictionary(e.PacketData);
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
                AddSymbolTaifexDictionary(e.PacketData);
            }));
        }
        //------------------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// pats盤前回呼事件
        /// </summary>
        private void api_PatsFormat0Received(object sender, MdApi.PatsFormat0ReceivedEventArgs e)
        {
            App.Current.Dispatcher.Invoke((Action)(() =>
            {
                if(e.PacketData.Exchange == "End")
                {
                    StatusMessageList.Insert(0, DateTime.Now.ToString("HH:mm:ss:ttt") + "    PATS商品資料傳送完成");
                    return;
                }
                _patsFormat0List.Insert(0,e.PacketData);

                //TEST-全訂
                //api.Sub(AdapterCode.TAIFEX_GLOBAL_PATS, "1", string.Format("{0}.{1}.{2}", e.PacketData.Exchange, e.PacketData.Commodity, e.PacketData.Contract));
                //api.Sub(AdapterCode.TAIFEX_GLOBAL_PATS, "2", string.Format("{0}.{1}.{2}", e.PacketData.Exchange, e.PacketData.Commodity, e.PacketData.Contract));
            }));
        }
        //------------------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// pats回呼事件
        /// </summary>
        private void api_PatsFormat1Received(object sender, MdApi.PatsFormat1ReceivedEventArgs e)
        {
            App.Current.Dispatcher.Invoke((Action)(() =>
            {
                _patsFormat1List.Insert(0,e.PacketData);

                //TEST-商品更新個數
                //int count = _patsFormat1List.GroupBy(x => new { x.ExchangeNo, x.CommodityNo, x.ContractDate }).Count();
                //StatusMessageList.Insert(0, DateTime.Now.ToString("HH:mm:ss:ttt") + "    " + "個數:" + count);
            }));
        }
        //------------------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// pats回呼事件
        /// </summary>
        private void api_PatsFormat2Received(object sender, MdApi.PatsFormat2ReceivedEventArgs e)
        {
            App.Current.Dispatcher.Invoke((Action)(() =>
            {
                _patsFormat2List.Insert(0,e.PacketData);
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
                case "6":
                    adapterCode = AdapterCode.GLOBAL_PATS; break; /// <- PATS
                default:
                    adapterCode = AdapterCode.TSE;
                    break;
            }
            return adapterCode;
        }

        #region 讀盤前檔
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

            if (string.IsNullOrEmpty(data.StockID) || SymbolTseList.AllSymbolTseList.ContainsKey(data.StockID))
            {
                return;
            }

            SymbolTseList.AddSymbolTseData(new SymbolTse(data));
            SymbolTseDictionary = SymbolTseList.GetAllSymbolTseCollection();
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

            if (string.IsNullOrEmpty(data.StockID) || SymbolTpexList.AllSymbolTpexList.ContainsKey(data.StockID))
            {
                return;
            }

            SymbolTpexList.AddSymbolTpexData(new SymbolTpex(data));
            SymbolTpexDictionary = SymbolTpexList.GetAllSymbolTpexCollection();
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

            if (string.IsNullOrEmpty(data.B_ProdId) || SymbolTaifexList.AllSymbolTaifexList.ContainsKey(data.B_ProdId))
            {
                return;
            }

            SymbolTaifexList.AddSymbolTalfexData(new SymbolTaifex(data));
            SymbolTaifexDictionary = SymbolTaifexList.GetAllSymbolTaifexCollection();
        }
        #endregion

        /// <summary>
        /// 一般訂閱
        /// </summary>
        private void GeneralSupscribeSymbol(string market, string type, string symbolno)
        {
            if (api == null)
            {
                System.Windows.MessageBox.Show("未連接或連接錯誤");
                return;
            }
            string subSymbol = market + ";" + type + ";" + symbolno;
            if (string.IsNullOrEmpty(symbolno))
            {
                api.Sub(Rtn_adapterCode(market), type);
                SubscribeList.Insert(0, market + ";" + type);
            }
            else
            {
                api.Sub(Rtn_adapterCode(market), type, symbolno);
                SubscribeList.Insert(0, subSymbol);
            }
            StatusMessageList.Insert(0, DateTime.Now.ToString("HH:mm:ss:ttt") + "    " + "訂閱:" + subSymbol);
        }
        /// <summary>
        /// 寫資料至excel
        /// </summary>
        private void OutputExcel()
        {
            string sFilePath = Environment.CurrentDirectory + "\\" + DefaultSettings.Instance.SAVE_FILE_NAME;//本機路徑
            // 建立新的 Excel 工作簿
            HSSFWorkbook hssfworkbook = new HSSFWorkbook();
            
            HSSFSheet sheet = (HSSFSheet)hssfworkbook.CreateSheet("行情");

            //創建單元格設置對象
            HSSFCellStyle cellStyle = (HSSFCellStyle)hssfworkbook.CreateCellStyle();
            //創建設置字體對象
            HSSFFont font = (HSSFFont)hssfworkbook.CreateFont();
            font.FontHeightInPoints = 16;//設置字體大小
            font.Boldweight = short.MaxValue; //加粗
            cellStyle.SetFont(font);

            #region  寫入資料到工作表中
            //創建Excel行和單元格
            for (int k = 0; k < _quotesList.Count + 2; k++)
            {
                HSSFRow newRow = (HSSFRow)sheet.CreateRow(k);
                for (int j = 0; j < 28 + 1; j++)
                {
                    newRow.CreateCell(j);
                }
            }
            sheet.GetRow(1).GetCell(0).SetCellValue("序號");
            sheet.GetRow(1).GetCell(1).SetCellValue("商品代號");
            sheet.GetRow(1).GetCell(2).SetCellValue("市場");
            sheet.GetRow(1).GetCell(3).SetCellValue("類別");
            sheet.GetRow(1).GetCell(4).SetCellValue("撮合價");
            sheet.GetRow(1).GetCell(5).SetCellValue("撮合量");
            sheet.GetRow(1).GetCell(6).SetCellValue("總量");
            sheet.GetRow(1).GetCell(7).SetCellValue("時間");
            sheet.GetRow(1).GetCell(8).SetCellValue("賣一價");
            sheet.GetRow(1).GetCell(9).SetCellValue("賣一量");
            sheet.GetRow(1).GetCell(10).SetCellValue("賣二價");
            sheet.GetRow(1).GetCell(11).SetCellValue("賣二量");
            sheet.GetRow(1).GetCell(12).SetCellValue("賣三價");
            sheet.GetRow(1).GetCell(13).SetCellValue("賣三量");
            sheet.GetRow(1).GetCell(14).SetCellValue("賣四價");
            sheet.GetRow(1).GetCell(15).SetCellValue("賣四量");
            sheet.GetRow(1).GetCell(16).SetCellValue("賣五價");
            sheet.GetRow(1).GetCell(17).SetCellValue("賣五量");
            sheet.GetRow(1).GetCell(18).SetCellValue("買一價");
            sheet.GetRow(1).GetCell(19).SetCellValue("買一量");
            sheet.GetRow(1).GetCell(20).SetCellValue("買二價");
            sheet.GetRow(1).GetCell(21).SetCellValue("買二量");
            sheet.GetRow(1).GetCell(22).SetCellValue("買三價");
            sheet.GetRow(1).GetCell(23).SetCellValue("買三量");
            sheet.GetRow(1).GetCell(24).SetCellValue("買四價");
            sheet.GetRow(1).GetCell(25).SetCellValue("買四量");
            sheet.GetRow(1).GetCell(26).SetCellValue("買五價");
            sheet.GetRow(1).GetCell(27).SetCellValue("買五量");
            sheet.GetRow(1).GetCell(0).CellStyle = cellStyle;
            sheet.GetRow(1).GetCell(1).CellStyle = cellStyle;
            sheet.GetRow(1).GetCell(2).CellStyle = cellStyle;
            sheet.GetRow(1).GetCell(3).CellStyle = cellStyle;
            sheet.GetRow(1).GetCell(4).CellStyle = cellStyle;
            sheet.GetRow(1).GetCell(5).CellStyle = cellStyle;
            sheet.GetRow(1).GetCell(6).CellStyle = cellStyle;
            sheet.GetRow(1).GetCell(7).CellStyle = cellStyle;
            sheet.GetRow(1).GetCell(8).CellStyle = cellStyle;
            sheet.GetRow(1).GetCell(9).CellStyle = cellStyle;
            sheet.GetRow(1).GetCell(10).CellStyle = cellStyle;
            sheet.GetRow(1).GetCell(11).CellStyle = cellStyle;
            sheet.GetRow(1).GetCell(12).CellStyle = cellStyle;
            sheet.GetRow(1).GetCell(13).CellStyle = cellStyle;
            sheet.GetRow(1).GetCell(14).CellStyle = cellStyle;
            sheet.GetRow(1).GetCell(15).CellStyle = cellStyle;
            sheet.GetRow(1).GetCell(16).CellStyle = cellStyle;
            sheet.GetRow(1).GetCell(17).CellStyle = cellStyle;
            sheet.GetRow(1).GetCell(18).CellStyle = cellStyle;
            sheet.GetRow(1).GetCell(19).CellStyle = cellStyle;
            sheet.GetRow(1).GetCell(20).CellStyle = cellStyle;
            sheet.GetRow(1).GetCell(21).CellStyle = cellStyle;
            sheet.GetRow(1).GetCell(22).CellStyle = cellStyle;
            sheet.GetRow(1).GetCell(23).CellStyle = cellStyle;
            sheet.GetRow(1).GetCell(24).CellStyle = cellStyle;
            sheet.GetRow(1).GetCell(25).CellStyle = cellStyle;
            sheet.GetRow(1).GetCell(26).CellStyle = cellStyle;
            sheet.GetRow(1).GetCell(27).CellStyle = cellStyle;
            int i = 1;
            foreach (Quotes quote in _quotesList.Values)
            {
                i = i + 1;
                sheet.GetRow(i).GetCell(0).SetCellValue(quote.Seq);
                sheet.GetRow(i).GetCell(1).SetCellValue(quote.SymbolNo);
                sheet.GetRow(i).GetCell(2).SetCellValue(quote.Market);
                sheet.GetRow(i).GetCell(3).SetCellValue(quote.TypeNo);
                sheet.GetRow(i).GetCell(4).SetCellValue(quote.MatchPrice);
                sheet.GetRow(i).GetCell(5).SetCellValue(quote.MatchQty);
                sheet.GetRow(i).GetCell(6).SetCellValue(quote.MatchTotalQty);
                sheet.GetRow(i).GetCell(7).SetCellValue(quote.Time);
                sheet.GetRow(i).GetCell(8).SetCellValue(quote.Bid1Price);
                sheet.GetRow(i).GetCell(9).SetCellValue(quote.Bid1Qty);
                sheet.GetRow(i).GetCell(10).SetCellValue(quote.Bid2Price);
                sheet.GetRow(i).GetCell(11).SetCellValue(quote.Bid2Qty);
                sheet.GetRow(i).GetCell(12).SetCellValue(quote.Bid3Price);
                sheet.GetRow(i).GetCell(13).SetCellValue(quote.Bid3Qty);
                sheet.GetRow(i).GetCell(14).SetCellValue(quote.Bid4Price);
                sheet.GetRow(i).GetCell(15).SetCellValue(quote.Bid4Qty);
                sheet.GetRow(i).GetCell(16).SetCellValue(quote.Bid5Price);
                sheet.GetRow(i).GetCell(17).SetCellValue(quote.Bid5Qty);
                sheet.GetRow(i).GetCell(18).SetCellValue(quote.Ask1Price);
                sheet.GetRow(i).GetCell(19).SetCellValue(quote.Ask1Qty);
                sheet.GetRow(i).GetCell(20).SetCellValue(quote.Ask2Price);
                sheet.GetRow(i).GetCell(21).SetCellValue(quote.Ask2Qty);
                sheet.GetRow(i).GetCell(22).SetCellValue(quote.Ask3Price);
                sheet.GetRow(i).GetCell(23).SetCellValue(quote.Ask3Qty);
                sheet.GetRow(i).GetCell(24).SetCellValue(quote.Ask4Price);
                sheet.GetRow(i).GetCell(25).SetCellValue(quote.Ask4Qty);
                sheet.GetRow(i).GetCell(26).SetCellValue(quote.Ask5Price);
                sheet.GetRow(i).GetCell(27).SetCellValue(quote.Ask5Qty);
            }
            #endregion

            // 儲存檔案
            FileStream file = new FileStream(sFilePath, FileMode.OpenOrCreate);
            hssfworkbook.Write(file);
            file.Close();
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
