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
using System.IO;
using System.Collections.Concurrent;
using System.Data;
using System.Windows.Data;
using QuoteChartExample.Model;
using System.Windows.Threading;

namespace QuoteChartExample.ViewModels
{
    public class MainViewModel: INotifyPropertyChanged
    {
        #region Fields
        public static readonly List<string> MarketList = new List<string>() { "0-上市", "1-上櫃", "2-期貨AM盤", "3-選擇權AM盤", "4-期貨PM盤", "5-選擇權PM盤"};
        public static readonly List<string> TypeList = new List<string>() { "I020", "6", "17"};

        private static MainViewModel _instance;
        private string _selectMarket = "0-上市";
        private string _selectType = "6";
        private string _symbolNo = "";
        private Dictionary<string, Quotes> _quotesList = new Dictionary<string, Quotes>();
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
        private string _symbolNoA = "2330";
        private string _symbolNoB = "0050";
        private decimal _quotesA = 0;
        private decimal _quotesB = 0;
        private decimal _quotesAnswer = 0;
        private ObservableCollection<string> _statusMessageList = new ObservableCollection<string>();
        #endregion

        public MainViewModel()
        {
            DefaultSettings.Instance.Initialize();//讀設定檔
            ConncetMarketData();
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
                OnPropertyChanged("QuotesList");
            }
        }
        /// <summary>
        /// 現貨上市商品資料篩選後
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
        /// 現貨上櫃商品資料篩選後
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
        /// 期貨商品資料篩選後
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
        /// 篩選期貨key
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
        /// 篩選現貨上市key
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
        /// 篩選現貨上櫃key
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
        #endregion

        #region Command
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

        private ICommand _supscribeACommand;
        public ICommand SupscribeACommand
        {
            get
            {
                if (_supscribeACommand == null)
                {
                    _supscribeACommand = new RelayCommand(
                        SupscribeASymbol
                    );
                }
                return _supscribeACommand;
            }
        }

        private ICommand _supscribeBCommand;
        public ICommand SupscribeBCommand
        {
            get
            {
                if (_supscribeBCommand == null)
                {
                    _supscribeBCommand = new RelayCommand(
                        SupscribeBSymbol
                    );
                }
                return _supscribeBCommand;
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
                api = new MdApi(DefaultSettings.Instance.SUP_IP, DefaultSettings.Instance.SUP_PORT, DefaultSettings.Instance.SERVICE_IP, DefaultSettings.Instance.SERVICE_PORT);
                api.TaifexI020Received += api_TaifexI020Received;  /// <- 期貨I020[成交]回呼事件
                api.TaifexI022Received += api_TaifexI022Received;  /// <- 期貨I022試撮[成交]回呼事件
                api.TaifexI080Received += api_TaifexI080Received;  /// <- 期貨I080[委買委賣]回呼事件
                api.TaifexI082Received += api_TaifexI082Received;  /// <- 期貨I082試撮回呼事件
                api.TseFormat6Received += api_TseFormat6Received;  /// <- 上市現貨格式6(Format6)回呼事件
                api.TpexFormat6Received += api_TpexFormat6Received; /// <- 上櫃現貨格式6(Format6)回呼事件
                api.TseFormat17Received += api_TseFormat17Received;  /// <- 上市現貨格式17(Format17)回呼事件
                api.TpexFormat17Received += api_TpexFormat17Received; /// <- 上櫃現貨格式17(Format17)回呼事件
                StatusMessageList.Insert(0, DateTime.Now.ToString("HH:mm:ss:ttt") + "    " + "連接" + DefaultSettings.Instance.SUP_IP + ":" + DefaultSettings.Instance.SUP_PORT + "," + DefaultSettings.Instance.SERVICE_IP + ":" + DefaultSettings.Instance.SERVICE_PORT);

                GetPreSymbol();
            }
            catch (Exception err)
            {
                StatusMessageList.Insert(0, DateTime.Now.ToString("HH:mm:ss:ttt") + "    " + string.Format("ConncetMarketData(): ErrMsg = {0}.", err.Message));
            }
        }
        //------------------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// 盤前TSE切換
        /// </summary>
        private void TseGridDoubleClick()
        {
            try
            {
                if (_selectTse == null)
                    return;
                SelectMarket = "0-上市";
                SelectType = _selectTse.WarrantCode == "Y" ? "17" : "6";
                SymbolNo = _selectTse.SymbolNo;
            }
            catch(Exception)
            { }
        }
        //------------------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// 盤前TPEX切換
        /// </summary>
        private void TpexGridDoubleClick()
        {
            try
            {
                if (_selectTpex == null)
                    return;
                SelectMarket = "1-上櫃";
                SelectType = _selectTpex.WarrantCode == "Y" ? "17" : "6";
                SymbolNo = _selectTpex.SymbolNo;
            }
            catch (Exception)
            { }
        }
        //------------------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// 盤前TAIFEX切換
        /// </summary>
        private void TaifexGridDoubleClick()
        {
            if (_selectTaifex == null)
                return;
            if (_selectTaifex.TypeNo == "期貨")
            {
                SelectMarket = "2-期貨AM盤";
                SelectType = "I020";
                SymbolNo = _selectTaifex.SymbolNo;
            }
            else
            {
                SelectMarket = "3-選擇權AM盤";
                SelectType = "I020";
                SymbolNo = _selectTaifex.SymbolNo;
            }
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
        /// 訂閱設定圖表商品A
        /// </summary>
        private void SupscribeASymbol()
        {
            if (api == null || string.IsNullOrEmpty(SelectMarket) || string.IsNullOrEmpty(SelectType))
            {
                return;
            }
            if (Rtn_adapterCode(SelectMarket) != AdapterCode.GLOBAL_PATS)
            {
                var ret = api.GetSnapshot(Rtn_adapterCode(SelectMarket), SelectType, SymbolNo);
                if (ret == null)
                {
                    StatusMessageList.Insert(0, DateTime.Now.ToString("HH:mm:ss:ttt") + "    " + "設定圖表商品A:" + SymbolNo + "失敗");
                    return;
                }
                else
                {
                    QuotesA = 0;
                    SymbolNoA = SymbolNo;
                    GeneralSupscribeSymbol(SelectMarket, SelectType, SymbolNo);
                    Quotes model = new Quotes();
                    switch (SelectMarket.Substring(0, 1))
                    {
                        case "0":
                            switch (SelectType)
                            {
                                case "6":
                                    {
                                        model.SetTseData(0, ret as MarketDataApi.PacketTSE.Format6);
                                        if (QuotesList.ContainsKey(model.MainKey))
                                        {
                                            QuotesList[model.MainKey] = model;
                                        }
                                        else
                                        {
                                            QuotesList.Add(model.MainKey, model);
                                        }
                                        var preQuotesA = Model.SymbolTseList.GetAllSymbolTseCollection().FirstOrDefault(x => x.SymbolNo == SymbolNoA);
                                        QuotesA = ((((decimal)(model.MatchPrice - preQuotesA.ReferencePrice))) / ((decimal)preQuotesA.ReferencePrice));
                                    }
                                    break;
                                case "17":
                                    {
                                        model.SetTseData(0, ret as MarketDataApi.PacketTSE.Format17);
                                        if (QuotesList.ContainsKey(model.MainKey))
                                        {
                                            QuotesList[model.MainKey] = model;
                                        }
                                        else
                                        {
                                            QuotesList.Add(model.MainKey, model);
                                        }
                                        var preQuotesA = Model.SymbolTseList.GetAllSymbolTseCollection().FirstOrDefault(x => x.SymbolNo == SymbolNoA);
                                        QuotesA = ((((decimal)(model.MatchPrice - preQuotesA.ReferencePrice))) / ((decimal)preQuotesA.ReferencePrice));
                                    }
                                    break;
                            }
                            break;
                        case "1":
                            switch (SelectType)
                            {
                                case "6":
                                    {
                                        model.SetTpexData(0, ret as MarketDataApi.PacketTPEX.Format6);
                                        if (QuotesList.ContainsKey(model.MainKey))
                                        {
                                            QuotesList[model.MainKey] = model;
                                        }
                                        else
                                        {
                                            QuotesList.Add(model.MainKey, model);
                                        }
                                        var preQuotesA = Model.SymbolTpexList.GetAllSymbolTpexCollection().FirstOrDefault(x => x.SymbolNo == SymbolNoA);
                                        QuotesA = ((((decimal)(model.MatchPrice - preQuotesA.ReferencePrice))) / ((decimal)preQuotesA.ReferencePrice));
                                    }
                                    break;
                                case "17":
                                    {
                                        model.SetTpexData(0, ret as MarketDataApi.PacketTPEX.Format17);
                                        if (QuotesList.ContainsKey(model.MainKey))
                                        {
                                            QuotesList[model.MainKey] = model;
                                        }
                                        else
                                        {
                                            QuotesList.Add(model.MainKey, model);
                                        }
                                        var preQuotesA = Model.SymbolTpexList.GetAllSymbolTpexCollection().FirstOrDefault(x => x.SymbolNo == SymbolNoA);
                                        QuotesA = ((((decimal)(model.MatchPrice - preQuotesA.ReferencePrice))) / ((decimal)preQuotesA.ReferencePrice));
                                    }
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
                                    if (QuotesList.ContainsKey(model.MainKey))
                                    {
                                        QuotesList[model.MainKey] = model;
                                    }
                                    else
                                    {
                                        QuotesList.Add(model.MainKey, model);
                                    }
                                    var preQuotesA = Model.SymbolTaifexList.GetAllSymbolTaifexCollection().FirstOrDefault(x => x.SymbolNo == SymbolNoA);
                                    QuotesA = ((((decimal)(model.MatchPrice - preQuotesA.ReferencePrice))) / ((decimal)preQuotesA.ReferencePrice));
                                    break;
                            }
                            break;
                    }
                    StatusMessageList.Insert(0, DateTime.Now.ToString("HH:mm:ss:ttt") + "    " + "設定圖表商品A:" + SymbolNo + "成功");
                }
            }
        }
        /// <summary>
        /// 訂閱設定圖表商品B
        /// </summary>
        private void SupscribeBSymbol()
        {
            if (api == null || string.IsNullOrEmpty(SelectMarket) || string.IsNullOrEmpty(SelectType))
            {
                return;
            }
            if (Rtn_adapterCode(SelectMarket) != AdapterCode.GLOBAL_PATS)
            {
                var ret = api.GetSnapshot(Rtn_adapterCode(SelectMarket), SelectType, SymbolNo);
                if (ret == null)
                {
                    StatusMessageList.Insert(0, DateTime.Now.ToString("HH:mm:ss:ttt") + "    " + "設定圖表商品B:" + SymbolNo + "失敗");
                    return;
                }
                else
                {
                    QuotesB = 0;
                    SymbolNoB = SymbolNo;
                    GeneralSupscribeSymbol(SelectMarket, SelectType, SymbolNo);
                    Quotes model = new Quotes();
                    switch (SelectMarket.Substring(0, 1))
                    {
                        case "0":
                            switch (SelectType)
                            {
                                case "6":
                                    {
                                        model.SetTseData(0, ret as MarketDataApi.PacketTSE.Format6);
                                        if (QuotesList.ContainsKey(model.MainKey))
                                        {
                                            if (model.MatchPrice == 0)
                                            {
                                                model.MatchPrice = QuotesList[model.MainKey].MatchPrice;
                                                model.MatchQty = QuotesList[model.MainKey].MatchQty;
                                            }
                                            QuotesList[model.MainKey] = model;
                                        }
                                        else
                                        {
                                            QuotesList.Add(model.MainKey, model);
                                        }
                                        var preQuotesB = Model.SymbolTseList.GetAllSymbolTseCollection().FirstOrDefault(x => x.SymbolNo == SymbolNoB);
                                        QuotesB = ((((decimal)(model.MatchPrice - preQuotesB.ReferencePrice))) / ((decimal)preQuotesB.ReferencePrice));
                                    }
                                    break;
                                case "17":
                                    {
                                        model.SetTseData(0, ret as MarketDataApi.PacketTSE.Format17);
                                        if (QuotesList.ContainsKey(model.MainKey))
                                        {
                                            if (model.MatchPrice == 0)
                                            {
                                                model.MatchPrice = QuotesList[model.MainKey].MatchPrice;
                                                model.MatchQty = QuotesList[model.MainKey].MatchQty;
                                            }
                                            QuotesList[model.MainKey] = model;
                                        }
                                        else
                                        {
                                            QuotesList.Add(model.MainKey, model);
                                        }
                                        var preQuotesB = Model.SymbolTseList.GetAllSymbolTseCollection().FirstOrDefault(x => x.SymbolNo == SymbolNoB);
                                        QuotesB = ((((decimal)(model.MatchPrice - preQuotesB.ReferencePrice))) / ((decimal)preQuotesB.ReferencePrice));
                                    }
                                    break;
                            }
                            break;
                        case "1":
                            switch (SelectType)
                            {
                                case "6":
                                    {
                                        model.SetTpexData(0, ret as MarketDataApi.PacketTPEX.Format6);
                                        if (QuotesList.ContainsKey(model.MainKey))
                                        {
                                            if (model.MatchPrice == 0)
                                            {
                                                model.MatchPrice = QuotesList[model.MainKey].MatchPrice;
                                                model.MatchQty = QuotesList[model.MainKey].MatchQty;
                                            }
                                            QuotesList[model.MainKey] = model;
                                        }
                                        else
                                        {
                                            QuotesList.Add(model.MainKey, model);
                                        }
                                        var preQuotesB = Model.SymbolTpexList.GetAllSymbolTpexCollection().FirstOrDefault(x => x.SymbolNo == SymbolNoB);
                                        QuotesB = ((((decimal)(model.MatchPrice - preQuotesB.ReferencePrice))) / ((decimal)preQuotesB.ReferencePrice));
                                    }
                                    break;
                                case "17":
                                    {
                                        model.SetTpexData(0, ret as MarketDataApi.PacketTPEX.Format17);
                                        if (QuotesList.ContainsKey(model.MainKey))
                                        {
                                            if (model.MatchPrice == 0)
                                            {
                                                model.MatchPrice = QuotesList[model.MainKey].MatchPrice;
                                                model.MatchQty = QuotesList[model.MainKey].MatchQty;
                                            }
                                            QuotesList[model.MainKey] = model;
                                        }
                                        else
                                        {
                                            QuotesList.Add(model.MainKey, model);
                                        }
                                        var preQuotesB = Model.SymbolTpexList.GetAllSymbolTpexCollection().FirstOrDefault(x => x.SymbolNo == SymbolNoB);
                                        QuotesB = ((((decimal)(model.MatchPrice - preQuotesB.ReferencePrice))) / ((decimal)preQuotesB.ReferencePrice));
                                    }
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
                                    if (QuotesList.ContainsKey(model.MainKey))
                                    {
                                        QuotesList[model.MainKey] = model;
                                    }
                                    else
                                    {
                                        QuotesList.Add(model.MainKey, model);
                                    }
                                    var preQuotesB = Model.SymbolTaifexList.GetAllSymbolTaifexCollection().FirstOrDefault(x => x.SymbolNo == SymbolNoB);
                                    QuotesB = ((((decimal)(model.MatchPrice - preQuotesB.ReferencePrice))) / ((decimal)preQuotesB.ReferencePrice));
                                    break;
                            }
                            break;
                    }
                    StatusMessageList.Insert(0, DateTime.Now.ToString("HH:mm:ss:ttt") + "    " + "設定圖表商品B:" + SymbolNo + "成功");
                }
            }
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
        /// 期貨I020[成交]回呼事件
        /// </summary>
        void api_TaifexI020Received(object sender, MdApi.TaifexI020ReceivedEventArgs e)
        {
            App.Current.Dispatcher.Invoke((Action)(() =>
            {
                Quotes model = new Quotes();
                model.SetI020Data(0, e.PacketData);

                if (QuotesList.ContainsKey(model.MainKey))
                {
                    QuotesList[model.MainKey] = model;
                }
                else
                {
                    QuotesList.Add(model.MainKey, model);
                }
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

                if (QuotesList.ContainsKey(model.MainKey))
                {
                    QuotesList[model.MainKey] = model;
                }
                else
                {
                    QuotesList.Add(model.MainKey, model);
                }
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

                if (QuotesList.ContainsKey(model.MainKey))
                {
                    QuotesList[model.MainKey] = model;
                }
                else
                {
                    QuotesList.Add(model.MainKey, model);
                }
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
                model.SetTpexData(0,  e.PacketData);

                if (QuotesList.ContainsKey(model.MainKey))
                {
                    if(model.MatchPrice == 0)
                    {
                        model.MatchPrice = QuotesList[model.MainKey].MatchPrice;
                        model.MatchQty = QuotesList[model.MainKey].MatchQty;
                    }
                    QuotesList[model.MainKey] = model;
                }
                else
                {
                    QuotesList.Add(model.MainKey, model);
                }
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
                model.SetTseData(0, e.PacketData);

                if (QuotesList.ContainsKey(model.MainKey))
                {
                    if (model.MatchPrice == 0)
                    {
                        model.MatchPrice = QuotesList[model.MainKey].MatchPrice;
                        model.MatchQty = QuotesList[model.MainKey].MatchQty;
                    }
                    QuotesList[model.MainKey] = model;
                }
                else
                {
                    QuotesList.Add(model.MainKey, model);
                }
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
                model.SetTpexData(0, e.PacketData);

                if (QuotesList.ContainsKey(model.MainKey))
                {
                    if (model.MatchPrice == 0)
                    {
                        model.MatchPrice = QuotesList[model.MainKey].MatchPrice;
                        model.MatchQty = QuotesList[model.MainKey].MatchQty;
                    }
                    QuotesList[model.MainKey] = model;
                }
                else
                {
                    QuotesList.Add(model.MainKey, model);
                }
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
                model.SetTseData(0, e.PacketData);

                if (QuotesList.ContainsKey(model.MainKey))
                {
                    if (model.MatchPrice == 0)
                    {
                        model.MatchPrice = QuotesList[model.MainKey].MatchPrice;
                        model.MatchQty = QuotesList[model.MainKey].MatchQty;
                    }
                    QuotesList[model.MainKey] = model;
                }
                else
                {
                    QuotesList.Add(model.MainKey, model);
                }
            }));
        }        
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
            }
            else
            {
                api.Sub(Rtn_adapterCode(market), type, symbolno);
            }
            StatusMessageList.Insert(0, DateTime.Now.ToString("HH:mm:ss:ttt") + "    " + "訂閱:" + subSymbol);
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
