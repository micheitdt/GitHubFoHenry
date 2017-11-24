using CommonLibrary.Model;
using MarketDataApi;
using ServiceStack.Redis;
using System;
using System.IO;
using System.Text;
using CommonLibrary;

namespace Process.BaseMarketData.ViewModels
{
    public class MainViewModel
    {
        private static MainViewModel _instance;
        MarketDataApi.MarketDataApi api;
        RedisClient _client;

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
            Utility.SaveLog(DateTime.Now.ToString("HH:mm:ss:ttt") + "初始化");
            if (Utility.TestConn(DefaultSettings.Instance.REDIS_DB_IP, DefaultSettings.Instance.REDIS_DB_PORT))
            {
                //連接redis
                _client = new RedisClient(DefaultSettings.Instance.REDIS_DB_IP, DefaultSettings.Instance.REDIS_DB_PORT);
                switch (DefaultSettings.Instance.LOAD_MD_MODE)
                {
                    case "0":
                        {
                            Utility.SaveLog(DateTime.Now.ToString("HH:mm:ss:ttt") + "盤前資料方法-讀檔");
                            LoadTSEData();
                            LoadTPEXData();
                            LoadTAIFEXData();
                            System.Windows.MessageBox.Show("已把檔案轉存Redis資料庫內", "注意", System.Windows.MessageBoxButton.OK);
                            Environment.Exit(0);
                            break;
                        }
                    case "1":
                        {
                            Utility.SaveLog(DateTime.Now.ToString("HH:mm:ss:ttt") + "盤前資料方法-連接proxy接收訊息");
                            api = new MarketDataApi.MarketDataApi(DefaultSettings.Instance.UDP_IP, DefaultSettings.Instance.UDP_PORT);
                            api.TaifexI010Received += api_TaifexI010Received; /// <- 期貨I010[盘前]回呼事件
                            api.TseFormat1Received += api_TseFormat1Received; /// <- 上櫃現貨格式1(盘前)回呼事件
                            api.TpexFormat1Received += api_TpexFormat1Received;/// <- 上市現貨格式1(盘前)回呼事件
                            //盤前訊息訂閱
                            api.Sub(AdapterCode.TAIFEX_FUTURES_DAY, "I010");
                            api.Sub(AdapterCode.TAIFEX_OPTIONS_DAY, "I010");
                            api.Sub(AdapterCode.TAIFEX_FUTURES_NIGHT, "I010");
                            api.Sub(AdapterCode.TAIFEX_OPTIONS_NIGHT, "I010");
                            api.Sub(AdapterCode.TSE, "1");
                            api.Sub(AdapterCode.TPEX, "1");
                            break;
                        }
                    default:
                        Utility.SaveLog(DateTime.Now.ToString("HH:mm:ss:ttt") + "LoadTSEData(): ErrMsg = LOAD_MD_MODE無法辨視.");
                        break;
                }
            }
            else
            {
                Utility.SaveLog(DateTime.Now.ToString("HH:mm:ss:ttt") + "    Redis连接失敗:" + DefaultSettings.Instance.REDIS_DB_IP + ":" + DefaultSettings.Instance.REDIS_DB_PORT + "  無法抓取盤前資料");
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
                        MarketDataApi.Model.PacketTSE.Format1 data = new MarketDataApi.Model.PacketTSE.Format1(Encoding.Default.GetBytes(line));
                        AddSymbolTseDictionary(data);
                    }
                }
            }
            catch (Exception err)
            {
                Utility.SaveLog(DateTime.Now.ToString("HH:mm:ss:ttt") + string.Format("LoadTSEData(): ErrMsg = {0}.", err.Message));
            }
        }
        private void AddSymbolTseDictionary(MarketDataApi.Model.PacketTSE.Format1 data)
        {
            if (string.IsNullOrEmpty(data.StockID))
            {
                return;
            }
            SymbolTse temp = new SymbolTse(data);
            if (temp.StkCountMark == "NE" || temp.StkCountMark == "AL")//NE:開盤-收盤；AL開盤前 資料末筆
            {
                Utility.SaveLog(DateTime.Now.ToString("HH:mm:ss:ttt") + string.Format("接收末筆TSE：{0}{1}筆資料", temp.StkCountMark, data.StockID));
            }
            else
            {
                Utility.SetRedisDB(_client, Parameter.TSE_FORMAT1_HASH_KEY, data.StockID, data);
                Utility.SaveLog(DateTime.Now.ToString("HH:mm:ss:ttt") + string.Format("Redis新增TSE：{0}    {1}", data.StockID, data.StockName));
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
                        MarketDataApi.Model.PacketTPEX.Format1 data = new MarketDataApi.Model.PacketTPEX.Format1(Encoding.Default.GetBytes(line));
                        AddSymbolTpexDictionary(data);
                    }
                }
            }
            catch (Exception err)
            {
                Utility.SaveLog(DateTime.Now.ToString("HH:mm:ss:ttt") + string.Format("LoadData(): ErrMsg = {0}.", err.Message));
            }
        }
        private void AddSymbolTpexDictionary(MarketDataApi.Model.PacketTPEX.Format1 data)
        {
            if (string.IsNullOrEmpty(data.StockID))
            {
                return;
            }
            SymbolTpex temp = new SymbolTpex(data);
            if (temp.StkCountMark == "NE" || temp.StkCountMark == "AL")//NE:開盤-收盤；AL開盤前 資料末筆
            {
                Utility.SaveLog(DateTime.Now.ToString("HH:mm:ss:ttt") + string.Format("接收末筆TPEX：{0}{1}筆資料", temp.StkCountMark, data.StockID));
            }
            else
            {
                Utility.SetRedisDB(_client, Parameter.TPEX_FORMAT1_HASH_KEY, data.StockID, data);
                Utility.SaveLog(DateTime.Now.ToString("HH:mm:ss:ttt") + string.Format("Redis新增TPEX：{0}    {1}", data.StockID, data.StockName));
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
                        MarketDataApi.Model.PacketTAIFEX.I010 data = new MarketDataApi.Model.PacketTAIFEX.I010(Encoding.Default.GetBytes(line), 0);
                        AddSymbolTaifexDictionary(data);
                    }
                }
            }
            catch (Exception err)
            {
                Utility.SaveLog(DateTime.Now.ToString("HH:mm:ss:ttt") + string.Format("LoadData(): ErrMsg = {0}.", err.Message));
            }
        }
        private void AddSymbolTaifexDictionary(MarketDataApi.Model.PacketTAIFEX.I010 data)
        {
            if (string.IsNullOrEmpty(data.B_ProdId))
            {
                return;
            }
            Utility.SetRedisDB(_client, Parameter.I010_HASH_KEY, data.B_ProdId, data);
            Utility.SaveLog(DateTime.Now.ToString("HH:mm:ss:ttt") + string.Format("Redis新增(1期貨.2選擇權{0})：{1}", data.H_TransmissionCode, data.B_ProdId));
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
        #endregion
    }
}
