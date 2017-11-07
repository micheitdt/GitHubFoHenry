using CommonLibrary.Model;
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
            _logger.Debug("Init");
            //連接redis
            _client = new RedisClient(DefaultSettings.Instance.REDIS_DB_IP, DefaultSettings.Instance.REDIS_DB_PORT);
            if (DefaultSettings.Instance.IS_LOAD_FILE)
            {
                _logger.Debug("取盤前資料方法-讀檔");
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
            }
            else
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
            SymbolTseList.AddSymbolTseData(temp);
            
            Utility.SetRedisDB(_client, Parameter.TSE_HASH_KEY, data.StockID, temp);
            _logger.Debug(string.Format("Redis新增：{0}    {1}", data.StockID, data.StockName));
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
            SymbolTpexList.AddSymbolTpexData(temp);
            
            Utility.SetRedisDB(_client, Parameter.TPEX_HASH_KEY,data.StockID, temp);
            _logger.Debug(string.Format("Redis新增：{0}    {1}", data.StockID, data.StockName));
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
            
            Utility.SetRedisDB(_client, Parameter.TAIFEX_HASH_KEY, data.B_ProdId, temp);
            _logger.Debug(string.Format("Redis新增：{0}", data.B_ProdId));
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
