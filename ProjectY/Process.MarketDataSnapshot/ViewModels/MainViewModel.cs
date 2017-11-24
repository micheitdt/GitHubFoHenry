using CommonLibrary.Model;
using MarketDataApi;
using ServiceStack.Redis;
using System;
using System.IO;
using System.Text;
using CommonLibrary;

namespace Process.MarketDataSnapshot.ViewModels
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
            Utility.SaveLog(DateTime.Now.ToString("HH:mm:ss:ttt") + "   初始化");
            if (Utility.TestConn(DefaultSettings.Instance.REDIS_DB_IP, DefaultSettings.Instance.REDIS_DB_PORT))
            {
                //連接redis
                _client = new RedisClient(DefaultSettings.Instance.REDIS_DB_IP, DefaultSettings.Instance.REDIS_DB_PORT);
                Utility.SaveLog(DateTime.Now.ToString("HH:mm:ss:ttt") + "   盤中資料方法-錄制期/現貨日盤商品訊息");
                api = new MarketDataApi.MarketDataApi(DefaultSettings.Instance.UDP_IP, DefaultSettings.Instance.UDP_PORT);
                api.TaifexI022Received += Api_TaifexI022Received;  /// <- 期貨I020[成交試撮]回呼事件
                api.TaifexI082Received += Api_TaifexI082Received;  /// <- 期貨I020[委買委賣試撮]回呼事件
                api.TaifexI020Received += api_TaifexI020Received;  /// <- 期貨I020[成交]回呼事件
                api.TaifexI080Received += api_TaifexI080Received;  /// <- 期貨I080[委買委賣]回呼事件
                api.TseFormat6Received += api_TseFormat6Received;  /// <- 上市現貨格式6(Format6)回呼事件
                api.TpexFormat6Received += api_TpexFormat6Received; /// <- 上櫃現貨格式6(Format6)回呼事件
                api.TseFormat17Received += api_TseFormat17Received;  /// <- 上市現貨格式6(Format17)回呼事件
                api.TpexFormat17Received += api_TpexFormat17Received; /// <- 上櫃現貨格式6(Format17)回呼事件
                api.Sub(AdapterCode.TAIFEX_FUTURES_NIGHT, "I022");
                api.Sub(AdapterCode.TAIFEX_FUTURES_NIGHT, "I082");
                api.Sub(AdapterCode.TAIFEX_OPTIONS_NIGHT, "I022");
                api.Sub(AdapterCode.TAIFEX_OPTIONS_NIGHT, "I082");
                api.Sub(AdapterCode.TAIFEX_FUTURES_DAY, "I022");
                api.Sub(AdapterCode.TAIFEX_FUTURES_DAY, "I082");
                api.Sub(AdapterCode.TAIFEX_OPTIONS_DAY, "I022");
                api.Sub(AdapterCode.TAIFEX_OPTIONS_DAY, "I082");
                api.Sub(AdapterCode.TAIFEX_FUTURES_NIGHT, "I020");
                api.Sub(AdapterCode.TAIFEX_FUTURES_NIGHT, "I080");
                api.Sub(AdapterCode.TAIFEX_OPTIONS_NIGHT, "I020");
                api.Sub(AdapterCode.TAIFEX_OPTIONS_NIGHT, "I080");
                api.Sub(AdapterCode.TAIFEX_FUTURES_DAY, "I020");
                api.Sub(AdapterCode.TAIFEX_FUTURES_DAY, "I080");
                api.Sub(AdapterCode.TAIFEX_OPTIONS_DAY, "I020");
                api.Sub(AdapterCode.TAIFEX_OPTIONS_DAY, "I080");
                api.Sub(AdapterCode.TSE, "6");
                api.Sub(AdapterCode.TSE, "17");
                api.Sub(AdapterCode.TPEX, "6");
                api.Sub(AdapterCode.TPEX, "17");
            }
            else
            {
                Utility.SaveLog(DateTime.Now.ToString("HH:mm:ss:ttt") + "    Redis连接失敗:" + DefaultSettings.Instance.REDIS_DB_IP + ":" + DefaultSettings.Instance.REDIS_DB_PORT + "  無法抓取盤前資料");
            }
        }

        #region func
        #endregion

        #region Event
        //------------------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// 期貨I022[資訊]回呼事件
        /// </summary>
        private void Api_TaifexI022Received(object sender, MarketDataApi.MarketDataApi.TaifexI022ReceivedEventArgs e)
        {
            Utility.SetRedisDB(_client, Parameter.I022_HASH_KEY, e.PacketData.B_ProdId, e.PacketData);
            Utility.SaveLog(DateTime.Now.ToString("HH:mm:ss:ttt") + string.Format("Redis新增{0}：{1}", "I022", e.PacketData.B_ProdId));
        }
        //------------------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// 期貨I082[資訊]回呼事件
        /// </summary>
        private void Api_TaifexI082Received(object sender, MarketDataApi.MarketDataApi.TaifexI082ReceivedEventArgs e)
        {
            Utility.SetRedisDB(_client, Parameter.I082_HASH_KEY, e.PacketData.B_ProdId, e.PacketData);
            Utility.SaveLog(DateTime.Now.ToString("HH:mm:ss:ttt") + string.Format("Redis新增{0}：{1}", "I082", e.PacketData.B_ProdId));
        }
        //------------------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// 期貨I080[委買委賣]回呼事件
        /// </summary>
        void api_TaifexI080Received(object sender, MarketDataApi.MarketDataApi.TaifexI080ReceivedEventArgs e)
        {
            App.Current.Dispatcher.Invoke((Action)(() =>
            {
                Utility.SetRedisDB(_client, Parameter.I080_HASH_KEY, e.PacketData.B_ProdId, e.PacketData);
                Utility.SaveLog(DateTime.Now.ToString("HH:mm:ss:ttt") + string.Format("Redis新增{0}：{1}", "I080", e.PacketData.B_ProdId));
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
                Utility.SetRedisDB(_client, Parameter.I020_HASH_KEY, e.PacketData.B_ProdId, e.PacketData);
                Utility.SaveLog(DateTime.Now.ToString("HH:mm:ss:ttt") + string.Format("Redis新增{0}：{1}", "I020", e.PacketData.B_ProdId));
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
                Utility.SetRedisDB(_client, Parameter.TPEX_FORMAT6_HASH_KEY, e.PacketData.StockID, e.PacketData);
                Utility.SaveLog(DateTime.Now.ToString("HH:mm:ss:ttt") + string.Format("Redis新增{0}：{1}", "上櫃格式6", e.PacketData.StockID));
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
                Utility.SetRedisDB(_client, Parameter.TSE_FORMAT6_HASH_KEY, e.PacketData.StockID, e.PacketData);
                Utility.SaveLog(DateTime.Now.ToString("HH:mm:ss:ttt") + string.Format("Redis新增{0}：{1}", "上市格式6", e.PacketData.StockID));
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
                Utility.SetRedisDB(_client, Parameter.TPEX_FORMAT17_HASH_KEY, e.PacketData.StockID, e.PacketData);
                Utility.SaveLog(DateTime.Now.ToString("HH:mm:ss:ttt") + string.Format("Redis新增{0}：{1}", "上櫃格式17", e.PacketData.StockID));
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
                Utility.SetRedisDB(_client, Parameter.TSE_FORMAT17_HASH_KEY, e.PacketData.StockID, e.PacketData);
                Utility.SaveLog(DateTime.Now.ToString("HH:mm:ss:ttt") + string.Format("Redis新增{0}：{1}", "上市格式17", e.PacketData.StockID));
            }));
        }
        #endregion
    }
}
