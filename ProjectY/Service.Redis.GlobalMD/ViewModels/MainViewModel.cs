﻿using CommonLibrary.Model;
using MarketDataApi;
using NLog;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLibrary;

namespace Service.Redis.GlobalMD.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {

        private static MainViewModel _instance;
        MarketDataApi.MarketDataApi api;
        RedisClient _client;
        private SymbolTseList _symbolTseDictionary = new SymbolTseList();
        private SymbolTpexList _symbolTpexDictionary = new SymbolTpexList();
        private SymbolTaifexList _symbolTaifexDictionary = new SymbolTaifexList();
        //錄制商品
        private long _gridSeq = 1;

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
        #endregion

        public MainViewModel()
        {
            //讀設定檔
            DefaultSettings.Instance.Initialize();
            _logger.Debug("初始化");
            if (Utility.TestConn(DefaultSettings.Instance.REDIS_DB_IP, DefaultSettings.Instance.REDIS_DB_PORT))
            {
                //連接redis
                _client = new RedisClient(DefaultSettings.Instance.REDIS_DB_IP, DefaultSettings.Instance.REDIS_DB_PORT);
                switch (DefaultSettings.Instance.LOAD_MD_MODE)
                {
                    case "0":
                        {
                            _logger.Debug("盤前資料方法-讀檔");
                            LoadTSEData();
                            LoadTPEXData();
                            LoadTAIFEXData();
                            //存取redis方法一
                            //foreach (KeyValuePair<string, SymbolTaifex> data in SymbolTaifexDictionary)
                            //{
                            //    System.Reflection.PropertyInfo[] propertyInfos = typeof(SymbolTaifex).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                            //    foreach (System.Reflection.PropertyInfo p in propertyInfos)
                            //    {
                            //        client.SetEntryInHash("I010", data.Key + "_" + p.Name, (p.GetValue(data.Value) == null) ? "" : p.GetValue(data.Value).ToString());
                            //    }
                            //}
                            //Dictionary<string, string> hashData = client.GetAllEntriesFromHash("I010");

                            //存取redis方法二
                            //_client.SetAll<SymbolTse>(SymbolTseDictionary);
                            //_client.SetAll<SymbolTpex>(SymbolTpexDictionary);
                            //_client.SetAll<SymbolTaifex>(SymbolTaifexDictionary);

                            //Dictionary<string,SymbolTse> returnValue1 = new Dictionary<string, SymbolTse>( _client.GetAll<SymbolTse>(SymbolTseDictionary.Keys));
                            //Dictionary<string, SymbolTpex> returnValue2 = new Dictionary<string, SymbolTpex>(_client.GetAll<SymbolTpex>(SymbolTpexDictionary.Keys));
                            //Dictionary<string, SymbolTaifex> returnValue3 = new Dictionary<string, SymbolTaifex>(_client.GetAll<SymbolTaifex>(SymbolTaifexDictionary.Keys));
                            System.Windows.MessageBox.Show("已把檔案轉存Redis資料庫內", "注意", System.Windows.MessageBoxButton.OK);
                            Environment.Exit(0);
                            break;
                        }
                    case "1":
                        {
                            _logger.Debug("盤前資料方法-連接proxy接收訊息");
                            api = new MarketDataApi.MarketDataApi(DefaultSettings.Instance.UDP_IP, DefaultSettings.Instance.UDP_PORT);
                            api.TaifexI010Received += api_TaifexI010Received; /// <- 期貨I010[盘前]回呼事件
                            api.TseFormat1Received += api_TseFormat1Received; /// <- 上櫃現貨格式1(盘前)回呼事件
                            api.TpexFormat1Received += api_TpexFormat1Received;/// <- 上市現貨格式1(盘前)回呼事件
                            //盤前訊息订阅
                            api.Sub(AdapterCode.TAIFEX_FUTURES_DAY, "I010");
                            api.Sub(AdapterCode.TAIFEX_OPTIONS_DAY, "I010");
                            api.Sub(AdapterCode.TSE, "1");
                            api.Sub(AdapterCode.TPEX, "1");
                            break;
                        }
                    case "2":
                        {
                            _logger.Debug("盤中資料方法-錄制期/現貨日盤商品訊息");
                            api = new MarketDataApi.MarketDataApi(DefaultSettings.Instance.UDP_IP, DefaultSettings.Instance.UDP_PORT);
                            //api.TaifexI020Received += api_TaifexI020Received;  /// <- 期貨I020[成交]回呼事件
                            //api.TaifexI080Received += api_TaifexI080Received;  /// <- 期貨I080[委買委賣]回呼事件
                            //api.TseFormat6Received += api_TseFormat6Received;  /// <- 上市現貨格式6(Format6)回呼事件
                            //api.TpexFormat6Received += api_TpexFormat6Received; /// <- 上櫃現貨格式6(Format6)回呼事件
                            //api.TseFormat17Received += api_TseFormat17Received;  /// <- 上市現貨格式6(Format17)回呼事件
                            //api.TpexFormat17Received += api_TpexFormat17Received; /// <- 上櫃現貨格式6(Format17)回呼事件
                            api.QuotesByteArrayReceived += Api_QuotesByteArrayReceived;/// <- 原始資料回呼事件
                            api.Sub(AdapterCode.TAIFEX_FUTURES_NIGHT, "I020");
                            api.Sub(AdapterCode.TAIFEX_FUTURES_NIGHT, "I080");
                            api.Sub(AdapterCode.TAIFEX_OPTIONS_NIGHT, "I020");
                            api.Sub(AdapterCode.TAIFEX_OPTIONS_NIGHT, "I080");
                            api.Sub(AdapterCode.TSE, "6");
                            api.Sub(AdapterCode.TSE, "17");
                            api.Sub(AdapterCode.TPEX, "6");
                            api.Sub(AdapterCode.TPEX, "17");
                            break;
                        }
                    default:
                        _logger.Error("LoadTSEData(): ErrMsg = LOAD_MD_MODE無法辨視.");
                        break;
                }
            }
            else
            {
                _logger.Debug(DateTime.Now.ToString("HH:mm:ss:ttt") + "    Redis连接失敗:" + DefaultSettings.Instance.REDIS_DB_IP + ":" + DefaultSettings.Instance.REDIS_DB_PORT + "  無法抓取盤前資料");
            }
        }

        #region func
        /// <summary>
        /// 讀現貨 上市
        /// </summary>
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
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (string.IsNullOrEmpty(line))
                        {
                            continue;
                        }
                        CommonLibrary.Model.PacketTSE.Format1 data = new CommonLibrary.Model.PacketTSE.Format1(Encoding.Default.GetBytes(line));
                        AddSymbolTseDictionary(data);
                    }
                }
            }
            catch (Exception err)
            {
                _logger.Error(err, string.Format("LoadTSEData(): ErrMsg = {0}.", err.Message));
            }
        }
        private void AddSymbolTseDictionary(CommonLibrary.Model.PacketTSE.Format1 data)
        {
            if (string.IsNullOrEmpty(data.StockID) || SymbolTseList.AllSymbolTseList.ContainsKey(data.StockID))
            {
                return;
            }
            SymbolTse temp = new SymbolTse(data);
            if (temp.StkCountMark == "NE" || temp.StkCountMark == "AL")//NE:開盤-收盤；AL開盤前 資料末筆
            {
                _logger.Debug(string.Format("接收末筆TSE：{0}{1}筆資料", temp.StkCountMark, data.StockID));
            }
            else
            {
                SymbolTseList.AddSymbolTseData(temp);
                Utility.SetRedisDB(_client, Parameter.TSE_FORMAT1_HASH_KEY, data.StockID, data);
                _logger.Debug(string.Format("Redis新增TSE：{0}    {1}", data.StockID, data.StockName));
            }
        }
        /// <summary>
        /// 讀現貨 上櫃
        /// </summary>
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
                        CommonLibrary.Model.PacketTPEX.Format1 data = new CommonLibrary.Model.PacketTPEX.Format1(Encoding.Default.GetBytes(line));
                        AddSymbolTpexDictionary(data);
                    }
                }
            }
            catch (Exception err)
            {
                _logger.Error(err, string.Format("LoadData(): ErrMsg = {0}.", err.Message));
            }
        }
        private void AddSymbolTpexDictionary(CommonLibrary.Model.PacketTPEX.Format1 data)
        {
            if (string.IsNullOrEmpty(data.StockID) || SymbolTpexList.AllSymbolTpexList.ContainsKey(data.StockID))
            {
                return;
            }
            SymbolTpex temp = new SymbolTpex(data);
            if (temp.StkCountMark == "NE" || temp.StkCountMark == "AL")//NE:開盤-收盤；AL開盤前 資料末筆
            {
                _logger.Debug(string.Format("接收末筆TPEX：{0}{1}筆資料", temp.StkCountMark, data.StockID));
            }
            else
            {
                SymbolTpexList.AddSymbolTpexData(temp);
                Utility.SetRedisDB(_client, Parameter.TPEX_FORMAT1_HASH_KEY, data.StockID, data);
                _logger.Debug(string.Format("Redis新增TPEX：{0}    {1}", data.StockID, data.StockName));
            }
        }
        /// <summary>
        /// 讀期權
        /// </summary>
        private void LoadTAIFEXData()
        {
            try
            {
                string path = System.Environment.CurrentDirectory + "\\TAIFEX_I010";
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
                        CommonLibrary.Model.PacketTAIFEX.I010 data = new CommonLibrary.Model.PacketTAIFEX.I010(Encoding.Default.GetBytes(line), 0);
                        AddSymbolTaifexDictionary(data);
                    }
                }
            }
            catch (Exception err)
            {
                _logger.Error(err, string.Format("LoadData(): ErrMsg = {0}.", err.Message));
            }
        }
        private void AddSymbolTaifexDictionary(CommonLibrary.Model.PacketTAIFEX.I010 data)
        {
            if (string.IsNullOrEmpty(data.B_ProdId) || SymbolTaifexList.AllSymbolTaifexList.ContainsKey(data.B_ProdId))
            {
                return;
            }

            SymbolTaifex temp = new SymbolTaifex(data);
            SymbolTaifexList.AddSymbolTalfexData(temp);
            
            Utility.SetRedisDB(_client, Parameter.I010_HASH_KEY, data.B_ProdId, data);
            string typeName = (data.H_TransmissionCode == "1") ? "期貨" : "選擇權";//4:選擇權
            _logger.Debug(string.Format("Redis新增{0}：{1}", typeName, data.B_ProdId));
        }
        #endregion

        #region Event
        //------------------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// 上市現貨格式1回呼事件
        /// </summary>
        private void api_TseFormat1Received(object sender, MarketDataApi.MarketDataApi.TseFormat1ReceivedEventArgs e)
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
        private void api_TpexFormat1Received(object sender, MarketDataApi.MarketDataApi.TpexFormat1ReceivedEventArgs e)
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
        private void api_TaifexI010Received(object sender, MarketDataApi.MarketDataApi.TaifexI010ReceivedEventArgs e)
        {
            App.Current.Dispatcher.Invoke((Action)(() =>
            {
                AddSymbolTaifexDictionary(e.PacketData);
            }));
        }
        //------------------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// 期貨I080[委買委賣]回呼事件
        /// </summary>
        void api_TaifexI080Received(object sender, MarketDataApi.MarketDataApi.TaifexI080ReceivedEventArgs e)
        {
            App.Current.Dispatcher.Invoke((Action)(() =>
            {
                Utility.SetRedisDB(_client, Parameter.I080_HASH_KEY, e.PacketData.B_ProdId + _gridSeq, e.PacketData);
                _logger.Debug(string.Format("Redis新增{0}：{1}", "I080" + _gridSeq, e.PacketData.B_ProdId));
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
                Utility.SetRedisDB(_client, Parameter.I020_HASH_KEY, e.PacketData.B_ProdId + _gridSeq, e.PacketData);
                _logger.Debug(string.Format("Redis新增{0}：{1}", "I020" + _gridSeq, e.PacketData.B_ProdId));
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
                Utility.SetRedisDB(_client, Parameter.TPEX_FORMAT6_HASH_KEY, e.PacketData.StockID + _gridSeq, e.PacketData);
                _logger.Debug(string.Format("Redis新增{0}：{1}", "上櫃格式6" + _gridSeq, e.PacketData.StockID));
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
                Utility.SetRedisDB(_client, Parameter.TSE_FORMAT6_HASH_KEY, e.PacketData.StockID + _gridSeq, e.PacketData);
                _logger.Debug(string.Format("Redis新增{0}：{1}", "上市格式6" + _gridSeq, e.PacketData.StockID));
                _gridSeq++;
            }));
        }
        //------------------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// 上櫃現貨格式17回呼事件
        /// </summary>
        void api_TpexFormat17Received(object sender, MarketDataApi.MarketDataApi.TpexFormat17ReceivedEventArgs e)
        {
            App.Current.Dispatcher.Invoke((Action)(() =>
            {
                Utility.SetRedisDB(_client, Parameter.TPEX_FORMAT17_HASH_KEY, e.PacketData.StockID + _gridSeq, e.PacketData);
                _logger.Debug(string.Format("Redis新增{0}：{1}", "上櫃格式17" + _gridSeq, e.PacketData.StockID));
                _gridSeq++;
            }));
        }
        //------------------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// 上市現貨格式17回呼事件
        /// </summary>
        void api_TseFormat17Received(object sender, MarketDataApi.MarketDataApi.TseFormat17ReceivedEventArgs e)
        {
            App.Current.Dispatcher.Invoke((Action)(() =>
            {
                Utility.SetRedisDB(_client, Parameter.TSE_FORMAT17_HASH_KEY, e.PacketData.StockID + _gridSeq, e.PacketData);
                _logger.Debug(string.Format("Redis新增{0}：{1}", "上市格式17" + _gridSeq, e.PacketData.StockID));
                _gridSeq++;
            }));
        }
        //------------------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// 行情原始資料回呼事件
        /// </summary>
        private void Api_QuotesByteArrayReceived(object sender, MarketDataApi.MarketDataApi.QuotesByteArrayEventArgs e)
        {
            App.Current.Dispatcher.Invoke((Action)(() =>
            {
                var data = Encoding.UTF8.GetString(e.KeyData);
                _client.Set<byte[]>(data+ _gridSeq, e.PacketData);
                _client.SetEntryInHash(Parameter.ORIGINAL_MD_HASH_KEY, data + _gridSeq, "");
                _logger.Debug(string.Format("Redis新增{0}", data + _gridSeq));
                _gridSeq++;
            }));
        }
        #endregion

        #region Logger
        /// <summary>
        /// 記錄器
        /// </summary>
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        #endregion Logger

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
