using MarketDataApi;
using NLog;
using Service.Redis.GlobalMD.Model;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Redis.GlobalMD.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {

        private static MainViewModel _instance;
        MarketDataApi.MarketDataApi api;
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
        #endregion

        public MainViewModel()
        {
            //讀設定檔
            DefaultSettings.Instance.Initialize();
            //連接redis
            RedisClient client = new RedisClient(DefaultSettings.Instance.REDIS_DB_IP, DefaultSettings.Instance.REDIS_DB_PORT);
            if (DefaultSettings.Instance.IS_LOAD_FILE)
            {
                //取盤前資料方法1-讀檔
                LoadTSEData();
                LoadTPEXData();
                LoadTAIFEXData();
                foreach(KeyValuePair<string, SymbolTse> data in SymbolTseDictionary)
                {
                    client.SetAll<SymbolTse>(SymbolTseDictionary);
                    //client.GetAll<>
                    client.SetEntryInHash("I010", data.Key, data.Value.CurrencyCode);
                    client.SetEntryInHash("hashid", "商品代碼", "");
                    client.SetEntryInHash("hashid", "商品代碼", "");
                    var i010 = client.GetAllEntriesFromHash("hashid");

                }
            }
            else
            {
                //方法2-連接proxy接收訊息
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
            //StatusMessageList.Insert(0, DateTime.Now.ToString("HH:mm:ss:ttt") + "    " + "连接" + IPAddress);
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
